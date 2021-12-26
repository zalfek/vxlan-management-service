using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Services
{
    public interface IKeyKeeper
    {
        public void PutKey(string key, IFormFile keyFile);
        public string GetKeyLocation(string key);

    }
}
