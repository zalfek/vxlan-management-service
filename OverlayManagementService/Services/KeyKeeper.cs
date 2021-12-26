using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Services
{
    public class KeyKeeper : IKeyKeeper
    {

        private readonly string _keyLocation;
        private static KeyKeeper _instance = null;

       public KeyKeeper()
        {
            _keyLocation = Path.Combine(AppContext.BaseDirectory, "Resources");
            _instance = this;
        }

        public static KeyKeeper GetInstance()
        {
            return _instance;
        }

        public string GetKeyLocation(string key)
        {
            return Path.Combine(_keyLocation, key);
        }

        public async void PutKey(string key, IFormFile keyFile)
        {
            var filePath = Path.Combine(_keyLocation, key);
            await using var stream = new FileStream(filePath, FileMode.Create);
            await keyFile.CopyToAsync(stream);
        }
    }
}
