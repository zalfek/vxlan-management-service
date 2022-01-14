using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace OverlayManagementService.Services
{

    /// <summary>
    /// Singleton which allows to store SSH key files for access to Target devices and Open Virtual Switch.
    /// </summary>
    public class KeyKeeper : IKeyKeeper
    {

        private readonly string _keyLocation;
        private static KeyKeeper _instance = null;

        /// <summary>
        /// Constructor for KeyKeeper.
        /// </summary>
        /// <returns>new KeyKeeper object</returns>
        public KeyKeeper()
        {
            _keyLocation = Path.Combine(AppContext.BaseDirectory, "Resources");
            _instance = this;
        }

        /// <summary>
        /// Getter for KeyKeeper Instance.
        /// </summary>
        /// <returns>KeyKeeper instance</returns>
        public static KeyKeeper GetInstance()
        {
            return _instance;
        }

        /// <summary>
        /// Method allows to get the path to the specific ssh key file.
        /// </summary>
        /// <param name="key">Key under which ssh key was saved</param>
        /// <returns>Path to ssh key file</returns>
        public string GetKeyLocation(string key)
        {
            return Path.Combine(_keyLocation, key);
        }

        /// <summary>
        /// Method allows to save new ssh key file.
        /// </summary>
        /// <param name="key">key to use as filename for the ssh key file</param>
        /// <param name="keyFile">ssh key file</param>
        public async void PutKey(string key, IFormFile keyFile)
        {
            var filePath = Path.Combine(_keyLocation, key);
            await using var stream = new FileStream(filePath, FileMode.Create);
            await keyFile.CopyToAsync(stream);
        }
    }
}
