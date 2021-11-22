import bcc
import time
from pyroute2 import IPRoute, NetNS, IPDB, NSPopen

b = bcc.BPF(src_file="vxlan_hp.c", debug=0)
fin = b.load_func("handle_ingress", bcc.BPF.SCHED_CLS)
fout = b.load_func("handle_egress", bcc.BPF.SCHED_CLS)

ipr = IPRoute()
ipdb = IPDB(nl=ipr)

ifc = ipdb.interfaces.enp0s8

ipr.tc("add", "ingress", ifc.index, "ffff:")
ipr.tc("add-filter", "bpf", ifc.index, ":1", fd=fin.fd, name=fin.name, parent="ffff:", action="ok", classid=1)
ipr.tc("add", "sfq", ifc.index, "1:")
ipr.tc("add-filter", "bpf", ifc.index, ":1", fd=fout.fd, name=fout.name, parent="1:", action="ok", classid=1)

while True:
	time.sleep(5)