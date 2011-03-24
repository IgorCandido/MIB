using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using First.Interfaces;

namespace First.BlackBox
{
    public class Blackbox : IBlackbox
    {

        Hashtable subscriptionsTable = new Hashtable(); 
        Hashtable emittersTable = new Hashtable();

        #region Implementation of IBlackbox

        public void Receive(string topic, object data)
        {
            List<IClientNode> subscriptions;

            if ((subscriptions = (subscriptionsTable[topic]) as List<IClientNode>) != null)
            {

                IEmitter emitter;

                foreach (IClientNode clientInformation in subscriptions)
                {
                    
                     if( ( emitter = emittersTable[clientInformation.emitterType] as IEmitter ) != null )
                     {
                         
                         emitter.Emit(clientInformation, data);

                     }

                }
                

            }
        }

        public void Subscribe(string topic, IClientNode clientInformation)
        {
            
            List<IClientNode> subscriptions;

            if ((subscriptions = (subscriptionsTable[topic]) as List<IClientNode>) != null)
            {

                if (!subscriptions.Contains(clientInformation))
                {

                    subscriptions.Add(clientInformation);

                }

            }
        }

        #endregion
    }
}
