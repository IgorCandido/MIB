using System.ServiceModel;
using Interfaces.Contracts;
using Interfaces.ContractsFromClients;

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
