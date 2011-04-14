using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;
using Blackbox.Handlers;
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
        private Pipeline.Pipeline pipeline;

        public Blackbox()
        {

            emittersTable["TCP"] = ProxyFactory.GetProxy<IEmitter>("tcpEmitter");
            pipeline = Pipeline.PipelineFactory.CreatePipeline(new MatchingSubscribe(emittersTable));

        }

        #region Implementation of IBlackbox

        public void Process(OperationContract operation)
        {
            
            pipeline.Process(operation);

        }
        
        #endregion
    }
}
