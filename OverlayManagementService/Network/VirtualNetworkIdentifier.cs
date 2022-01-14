
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace OverlayManagementService.Network
{

    /// <summary>
    /// CLass which encapsulates logic for generating an unique VNI.
    /// </summary>
    public class VirtualNetworkIdentifier : IIdentifier
    {
        private readonly ILogger<IIdentifier> _logger;
        private readonly string _backupPath;
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private static List<int> VNIs;
        private static int PreviousVNI;


        public VirtualNetworkIdentifier()
        {
            _backupPath = Path.Combine(AppContext.BaseDirectory, "Resources", "VniData.json");
            _jsonSerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            _logger = LoggerFactory.Create(logging => logging.AddConsole()).CreateLogger<IIdentifier>();
            VNIs = JsonConvert.DeserializeObject<List<int>>(File.ReadAllText(_backupPath), _jsonSerializerSettings);
            PreviousVNI = 0;
        }

        /// <summary>
        /// Method Generates and reserves unique VNI.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public string ReserveVNI()
        {
            if (VNIs.Count == 16000000) { throw new Exception("Maximum of Deployed networks is reached"); }
            while (VNIs.Contains(PreviousVNI))
            {
                ++PreviousVNI;
            }
            _logger.LogInformation("New VNI was generated:" + PreviousVNI.ToString());
            VNIs.Add(PreviousVNI);
            File.WriteAllText(_backupPath, JsonConvert.SerializeObject(VNIs, _jsonSerializerSettings));
            return PreviousVNI.ToString();
        }

        /// <summary>
        /// Method releases VNI.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ReleaseVNI(string vni)
        {
            VNIs.Remove(int.Parse(vni));
            _logger.LogInformation("VNI released:" + vni);
            File.WriteAllText(_backupPath, JsonConvert.SerializeObject(VNIs, _jsonSerializerSettings));
        }

    }
}
