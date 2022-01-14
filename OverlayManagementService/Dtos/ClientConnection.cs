namespace OverlayManagementService.Dtos
{
    public class ClientConnection
    {
        public ClientConnection(string vNI, string groupId, string remoteIp, string localIp)
        {
            VNI = vNI;
            GroupId = groupId;
            RemoteIp = remoteIp;
            LocalIp = localIp;
        }

        public string VNI { get; set; }
        public string GroupId { get; set; }
        public string RemoteIp { get; set; }
        public string LocalIp { get; set; }

    }
}
