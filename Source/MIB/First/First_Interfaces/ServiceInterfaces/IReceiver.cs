using System.ServiceModel;
using First.Interfaces;

namespace First_Interfaces.ServiceInterfaces
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
