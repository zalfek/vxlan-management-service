using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Factories
{
    public class IdentifierFactory<T> : IIdentifierFactory<T>
    {
        public T CreateAddressResolver()
        {
            return (T)Activator.CreateInstance(typeof(T), new object[] { });
        }
    }
}
