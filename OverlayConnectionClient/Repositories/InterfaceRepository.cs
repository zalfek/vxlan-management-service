using Microsoft.Extensions.Logging;
using OverlayConnectionClient.Network;
using System.Collections.Generic;


namespace OverlayConnectionClient.Repositories
{
    /// <summary>
    /// Class encapsulates the logic reguired to store interface objects in in-memory database.
    /// </summary>
    public class InterfaceRepository : IInterfaceRepository
    {

        private readonly ILogger<IInterfaceRepository> _logger;
        private static IDictionary<string, ILinuxVXLANInterface> _dbMock;

        /// <summary>
        /// Constructior for InterfaceRepository
        /// </summary>
        public InterfaceRepository()
        {
            _logger = LoggerFactory.Create(logging => logging.AddConsole()).CreateLogger<IInterfaceRepository>();
            _dbMock = new Dictionary<string, ILinuxVXLANInterface>();
        }

        /// <summary>
        /// Method allows to remove the interface object from in memory database for the specific group id.
        /// </summary>
        /// <param name="groupId">Id of Azure Active Derictory group for which Network was deployed</param>
        public void DeleteInterface(string groupId)
        {
            _logger.LogInformation("Removing interface with network id: " + groupId + " from in-memory database");
            _dbMock.Remove(groupId);
        }

        /// <summary>
        /// Method allows to get the VXLAN Interface object from in memory database for the specific group id.
        /// </summary>
        /// <param name="groupId">Id of Azure Active Derictory group for which Network was deployed</param>
        public ILinuxVXLANInterface GetVXLANInterface(string groupId)
        {
            return _dbMock[groupId];
        }

        /// <summary>
        /// Method allows to save the interface object to in memory database for the specific group id.
        /// </summary>
        /// <param name="linuxVXLANInterface">LinuxVXLANInterface object</param>
        /// <param name="groupId">Id of Azure Active Derictory group for which Network was deployed</param>
        public ILinuxVXLANInterface SaveInterface(string groupId, ILinuxVXLANInterface linuxVXLANInterface)
        {
            if (_dbMock.ContainsKey(groupId))
            {
                _logger.LogInformation("updating interface with network id: " + groupId + " from database");
                _dbMock[groupId] = linuxVXLANInterface;
            }
            else
            {
                _logger.LogInformation("Adding interface with network id: " + groupId + " from database");
                _dbMock.Add(groupId, linuxVXLANInterface);
            }
            return linuxVXLANInterface;
        }

        /// <summary>
        /// Method allows to get all deployed Interface objects from in memory database.
        /// </summary>
        /// <returns>Dictionary with maping Azure AD group to Linux VXLAN Interface</returns>
        public IDictionary<string, ILinuxVXLANInterface> GetAllInterfaces()
        {
            return _dbMock;
        }

    }
}
