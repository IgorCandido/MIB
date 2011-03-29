using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces.Contracts;

namespace Interfaces.RequestHandling
{
    public interface IHandleRequest
    {

        void HandleRequest(EventContract eventContract, object args);

        void HandleRequest(SubscriptionContract subscriptionContract, object args);

    }
}
