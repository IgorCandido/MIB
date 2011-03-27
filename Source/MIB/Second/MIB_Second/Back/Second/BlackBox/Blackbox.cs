using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;
using Second.Interfaces.Contracts;
using Second.Interfaces.Factories;
using Second.Interfaces.ServiceInterfaces;

namespace Second.BlackBox
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Blackbox : IBlackbox
    {

        Hashtable subscriptionsTable = new Hashtable(); 
        Hashtable emittersTable = new Hashtable();

        public Blackbox()
        {

            emittersTable["TCP"] = ProxyFactory.GetProxy<IEmitter>("tcpEmitter");

        }

        #region Implementation of IBlackbox

        public void Receive(string topic, object data)
        {
            List<ClientInformationContract> subscriptions;

            
            if ((subscriptions = (subscriptionsTable[topic]) as List<ClientInformationContract>) != null)
            {

                IEmitter emitter;

                foreach (ClientInformationContract clientInformation in subscriptions)
                {
                    
                     if( ( emitter = emittersTable[clientInformation.EmitterType] as IEmitter ) != null )
                     {
                         
                         emitter.Emit(clientInformation, data);

                     }

                }
                

            }
        }

        public void Subscribe(string topic, ClientInformationContract clientInformation)
        {

            List<ClientInformationContract> subscriptions;

            if ((subscriptions = (subscriptionsTable[topic]) as List<ClientInformationContract>) == null)
            {

                subscriptions = new List<ClientInformationContract>();

                subscriptionsTable[topic] = subscriptions;

            }

            if (!subscriptions.Contains(clientInformation))
            {

                subscriptions.Add(clientInformation);

            }
        }

        #endregion
    }
}
