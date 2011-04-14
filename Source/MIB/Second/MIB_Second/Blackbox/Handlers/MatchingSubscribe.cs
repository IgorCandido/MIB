using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blackbox.Pipeline;
using Blackbox.SubscriptionsTree;
using Interfaces.ContractsFromClients;
using Interfaces.ContractsInnerRepresentations;
using Interfaces.Services;

namespace Blackbox.Handlers
{
    public class MatchingSubscribe : IPipelineEventHandler
    {

        private readonly SubscriptionTree _subscriptionTree = new SubscriptionTree();
        private readonly Hashtable _emittersTable = new Hashtable();

        public MatchingSubscribe(Hashtable emitters)
        {

            _emittersTable = emitters;

        }

        List<ClientInformationContract> Match(Event @event, IPipelineContext pipelineContext)
        {

            List<Subscription> subscription = _subscriptionTree.Get(@event);

            return
                subscription.Select(
                    sub =>
                    new ClientInformationContract()
                        {
                            EmitterType = sub.ClientInformation.EmitterType,
                            SpecificClientInformation = sub.ClientInformation.SpecificClientInformation
                        }).ToList();

        }

        void Subscribe(Subscription subscription, IPipelineContext pipelineContext)
        {
           
            if( subscription.Unsubscribe )
            {

               _subscriptionTree.Remove(subscription);
    
            }
            else
            {

                _subscriptionTree.Add(subscription);

            }
            

        }

        void Emit(Event @event, IPipelineContext pipelineContext, List<ClientInformationContract> clients)
        {

            IEmitter emitter;

            foreach (var clientInformationContract in clients)
            {
                

                if( ( emitter = _emittersTable[clientInformationContract.EmitterType] as IEmitter ) != null )
                {
                    
                    emitter.Emit(clientInformationContract, @event);

                }

            }

        }

        #region Implementation of IPipelineEventHandler

        public void RegisterEventHandlers(Pipeline.Pipeline pipeline)
        {

            pipeline.Match += Match;
            pipeline.Subscribe += Subscribe;
            pipeline.Emit += Emit;

        }

        public void UnRegisterEventHandlers(Pipeline.Pipeline pipeline)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
