using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;
using Blackbox.Data;
using Blackbox.SubscriptionsTree;
using Interfaces.Contracts;
using Interfaces.ContractsFromClients;
using Interfaces.ContractsInnerRepresentations;
using Interfaces.Factories;
using Interfaces.Services; 

namespace Blackbox
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Blackbox : IBlackbox
    {

        private Hashtable emittersTable = new Hashtable();
        private SubscriptionTree subscriptionTree;
        private Pipeline.Pipeline pipeline;

        private SubscriptionTree BuildSubscriptionTreeFromDataBase()
        {
            
            return new SubscriptionTree(MIB_DAL.GetAllSubscriptions());

        }

        public Blackbox()
        {

            subscriptionTree = BuildSubscriptionTreeFromDataBase();

            emittersTable["TCP"] = ProxyFactory.GetProxy<IEmitter>("tcpEmitter");
            pipeline = new Pipeline.Pipeline(emittersTable, subscriptionTree);

       } 

        #region Implementation of IBlackbox

        public void Process(OperationContract operation)
        {
            
            pipeline.Process(operation);

        }
        
        #endregion
    }
}
