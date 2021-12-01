using System.Collections.Generic;

namespace OverlayManagementClient.Models
{
    public class Bridge
    {
        public string VNI { get; set; }
        public string Name { get; set; }
        public List<VXLANInterface> VXLANInterfaces { get; set; }
    }
}