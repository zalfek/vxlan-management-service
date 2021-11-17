
using Microsoft.Extensions.Logging;


namespace OverlayManagementService.Network
{
    public class Bridge: IBridge
    {
        private readonly string _name;
        private readonly ILogger<Bridge> _logger;

    }
}
