using System.ServiceModel;
using Second.Interfaces.Contracts;

namespace Second.Interfaces.ServiceInterfaces
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
