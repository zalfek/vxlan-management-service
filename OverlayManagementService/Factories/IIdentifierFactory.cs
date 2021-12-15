using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverlayManagementService.Factories
{
    public interface IIdentifierFactory<T>
    {
        public T CreateAddressResolver();
    }
}
