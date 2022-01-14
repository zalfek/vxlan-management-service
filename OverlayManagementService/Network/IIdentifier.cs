namespace OverlayManagementService.Network
{
    public interface IIdentifier
    {
        public string ReserveVNI();
        public void ReleaseVNI(string vni);
    }
}
