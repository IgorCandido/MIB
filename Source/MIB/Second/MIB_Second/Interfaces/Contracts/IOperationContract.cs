using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces.RequestHandling;

namespace Interfaces.Contracts
{

    //
    // This interface is the root of all operation contracts that blackbox receive from IReceivers
    //

    public abstract class OperationContract
    {

        public abstract void HandleRequestVisit(IHandleRequest handler, object args);
    }
}
