using Microsoft.AspNetCore.Http;

namespace OverlayManagementService.Services
{
    public interface IKeyKeeper
    {
        public void PutKey(string key, IFormFile keyFile);
        public string GetKeyLocation(string key);

    }
}
