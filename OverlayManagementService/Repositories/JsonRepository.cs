﻿using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OverlayManagementService.DataTransferObjects;
using OverlayManagementService.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverlayManagementService.Repositories
{
    public class JsonRepository : IRepository
    {

        private readonly ILogger<IRepository> _logger;
        private static IDictionary<string, IOverlayNetwork> _dbMock;
        public JsonRepository()
        {
            _logger = new LoggerFactory().CreateLogger<IRepository>();
            _dbMock = new Dictionary<string, IOverlayNetwork>();
        }

        public void DeleteOverlayNetwork(string membership)
        {
            _dbMock.Remove(membership);
        }

        public IOverlayNetwork GetOverlayNetwork(string membership)
        {
            return _dbMock[membership];
        }

        public IOverlayNetwork SaveOverlayNetwork(string membership, IOverlayNetwork overlayNetwork)
        {
            _dbMock.Add(membership, overlayNetwork);
            return overlayNetwork;
        }

        public IOverlayNetwork UpdateOverlayNetwork(string membership, IOverlayNetwork overlayNetwork)
        {
            _dbMock[membership] = overlayNetwork;
            return overlayNetwork;
        }
    }
}
