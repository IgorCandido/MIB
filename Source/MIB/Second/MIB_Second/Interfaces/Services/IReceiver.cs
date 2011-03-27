using System.ServiceModel;
using Interfaces.Contracts;

namespace Interfaces.Services
{

    [ServiceContract]
    public interface IReceiver
    {

        [OperationContract]
        void Receive(EventContract eventContract);

        [OperationContract]
        void Subscribe(SubscriptionContract subscriptionContract);
    }
}
