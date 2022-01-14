namespace OverlayManagementService.Network
{
    public interface IFirewall
    {
        public void AddException(string ip);
        public void RemoveException(string ip);
    }
}
