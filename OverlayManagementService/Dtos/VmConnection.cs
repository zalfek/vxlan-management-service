﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Dtos
{
    public class VmConnection
    {
        public string ManagementIp { get; set; }
        public string CommunicationIP { get; set; }
        public string GroupId { get; set; }
        public IFormFile KeyFile { get; set; }

    }
}
