using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces.Contracts;
using Interfaces.ContractsFromClients;
using Interfaces.ContractsInnerRepresentations;

namespace Interfaces.RequestHandling
{
    public interface IHandleRequest
    {

        void HandleRequest(Event @event);

        void HandleRequest(Subscription subscription);

    }
}
