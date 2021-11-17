using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Network
{
    public class VXLANInterface : IVXLANInterface
    {
        public VXLANInterface(string name, IBridge bridge, string type, string remoteIp, string key, string openFlowPort, string sourcePort)
        {
            Name = name;
            Bridge = bridge;
            Type = type;
            RemoteIp = remoteIp;
            Key = key;
            OpenFlowPort = openFlowPort;
            SourcePort = sourcePort;
        }

        private string Name { get; set; }
        private IBridge Bridge { get; set; }
        private string Type { get; set; }
        private string RemoteIp { get; set; }
        private string Key { get; set; }
        private string OpenFlowPort { get; set; }
        private string SourcePort { get; set; }

        public void CleanUpVXLANInterface()
        {
            throw new NotImplementedException();
        }

        public void DeployVXLANInterface()
        {
            throw new NotImplementedException();
        }
    }
}
