using System.ServiceModel;
using First.Interfaces;
using First_Interfaces;
using First_Interfaces.ServiceInterfaces;

namespace First_TcpReceiver
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerCall)]
    public class TcpReceiver : IReceiver
    {

        private readonly IBlackbox _blackbox = 
            ProxyFactory.GetProxy<IBlackbox>("blackbox") ;

        public TcpReceiver()
        {


        }
       
        #region Implementation of IReceiver

        [OperationBehavior(TransactionAutoComplete = true, TransactionScopeRequired = false)]
        public void Receive(EventContract eventContract)
        {
            
            _blackbox.Receive(eventContract.topic, eventContract.data);

        }


        [OperationBehavior(TransactionAutoComplete = true, TransactionScopeRequired = false)]
        public void Subscribe(SubscriptionContract subscriptionContract)
        {
            
            _blackbox.Subscribe(subscriptionContract.Topic, subscriptionContract.ClientInformation);

        }

        #endregion
    }
}
