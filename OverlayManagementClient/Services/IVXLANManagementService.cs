using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementClient.Services
{
    public interface IVXLANManagementService
    {


        public Task<OverlayNetwork> AddAsync(OverlayNetwork OverlayNetwork);
        public Task DeleteAsync(int id);
        public Task<OverlayNetwork> EditAsync(OverlayNetwork OverlayNetwork);
        public Task<IEnumerable<OverlayNetwork>> GetAsync();
        public Task<OverlayNetwork> GetAsync(int id);
       



    }
}
