using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementClient.Models
{
    public class OvsRegistration
    {
        public string PrivateIP { get; set; }
        public string PublicIP { get; set; }
        public string Key { get; set; }
        public string ManagementIp { get; set; }
        public IFormFile KeyFile { get; set; }

    }
}
