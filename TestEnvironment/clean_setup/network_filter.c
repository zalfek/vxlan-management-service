#include <bcc/proto.h>

struct peer4_t {
	u32 vxlan_key;
	u16 udp_port;
};

BPF_HASH(host2nat, u64, struct peer4_t); // mac -> nat

int handle_ingress(struct __sk_buff *skb){
	u8 *cursor = 0;
	struct ethernet_t *eth = cursor_advance(cursor, sizeof(*eth));

	struct ip_t *ip = cursor_advance(cursor, sizeof(*ip));
	if(ip->nextp != 17){
		return 1;
	}
	struct udp_t *udp = cursor_advance(cursor, sizeof(*udp));
	if(udp->dport != 4789){
		return 1;
	}

	// handle NAT
	struct vxlan_t *vxlan = cursor_advance(cursor, sizeof(*vxlan));
	struct ethernet_t *eth_in = cursor_advance(cursor, sizeof(*eth_in));
	
	u64 src = eth_in->src;
	if(src == 0){
		return 0;
	}
	
	struct peer4_t peer = {};
	peer.vxlan_key = vxlan->key;
	peer.udp_port = udp->sport;

	host2nat.update(&src, &peer);
	return 1;
}

int handle_egress(struct __sk_buff *skb){
	u8 *cursor = 0;
	struct ethernet_t *eth = cursor_advance(cursor, sizeof(*eth));

	struct ip_t *ip = cursor_advance(cursor, sizeof(*ip));
	if(ip->nextp != 17){
		return 1;
	}
	struct udp_t *udp = cursor_advance(cursor, sizeof(*udp));
	if(udp->dport != 4789){
		return 1;
	}

	struct vxlan_t *vxlan = cursor_advance(cursor, sizeof(*vxlan));
	struct ethernet_t *eth_in = cursor_advance(cursor, sizeof(*eth_in));
	u64 dst = eth_in->dst;

	// NAT ?
	struct peer4_t *nat = host2nat.lookup(&dst);
	if(nat != NULL && nat->vxlan_key == vxlan->key){
		udp->dport = nat->udp_port;
		udp->sport = 4789;
	}
	return 1;
}