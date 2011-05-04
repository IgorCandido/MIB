using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Transactions;
using Blackbox.Data;
using Blackbox.SubscriptionsTree;
using Interfaces.Contracts;
using Interfaces.ContractsFromClients;
using Interfaces.ContractsInnerRepresentations;
using Interfaces.RequestHandling;
using Interfaces.Services;
using Event = Interfaces.ContractsInnerRepresentations.Event;
using Subscription = Interfaces.ContractsInnerRepresentations.Subscription;

namespace Blackbox.Pipeline
{
    public class Pipeline : IHandleRequest
    {

        private SubscriptionTree _subscriptionTree;
        private Hashtable _emittersTable;

        private void Construct(Hashtable emittersTable, SubscriptionTree subscriptionTree)
        {
            _emittersTable = emittersTable;
            _subscriptionTree = subscriptionTree;
            
        }

        public Pipeline()
        {
            
            Construct(new Hashtable(), new SubscriptionTree());

        }

        public Pipeline(Hashtable emittersTable)
        {
            
            Construct(emittersTable, new SubscriptionTree());

        }

        public Pipeline(SubscriptionTree subscriptionTree)
        {

            Construct(new Hashtable(), subscriptionTree);

        }

        public Pipeline(Hashtable emittersTable, SubscriptionTree subscriptionTree)
        {

            Construct(emittersTable, subscriptionTree);

        }

        public void Process(OperationContract request)
        {
           
            //
            // Authenticate
            //

            Authenticate();

            //
            // Authorize
            //

            Authorize();

            //
            // Choose the operation type based on operation object concrete type
            //

            request.HandleRequestVisit(this);


        }

        
        #region Implementation of IHandleRequest

        //
        // Event handling
        //

        
        public void HandleRequest(Event @event)
        {

            //
            // Enlist on the sender transaction, if there is one
            //

            if(@event.propagationToken != null)
            {
             
                Transaction.Current = TransactionInterop.GetTransactionFromTransmitterPropagationToken(@event.propagationToken);
   
            }

            List<Subscription> matchingSubscriptions = null;

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
            {
                
                //
                // Match the event and store the information of matching clients
                //

                matchingSubscriptions = Match(@event);


                //
                // Persist event according to subscriptions needs, this information is stored on PipelineContext
                //

                Persist( matchingSubscriptions.Where(sub => sub.Persist).ToList(), @event );

                transactionScope.Complete();

            }

            //
            // Forward event to other brokers
            //

            Forward();

            //
            // Emit events according to matching clients information
            //
            
            Emit(@event, matchingSubscriptions);

        }

        
        //
        // Subscription handling
        //

        public void HandleRequest(Subscription subscription)
        {
            
            //
            // Handle the subscription
            //

            Subscribe(subscription);
                
            //
            // Route subscription to other brokers
            //

            Route();

        }

        #endregion

        #region Security

        private void Authenticate()
        {
            
        }
        
        private void Authorize()
        {
            
        }

        #endregion

        #region Event

        private List<Subscription> Match(Event @event)
        {

            List<Subscription> subscriptions = _subscriptionTree.Get(@event);

            return subscriptions;


        }

        private void Persist(List<Subscription> subscriptionsToPersist, Event @event)
        {

            if( subscriptionsToPersist.Count() != 0 )
            {
                
                MIB_DAL.SaveEventWithSubscriptions(subscriptionsToPersist, @event);

            }
            
        }

        private void Forward()
        {
            
        }

        private void Emit(Event @event, Subscription subscription)
        {

            IEmitter emitter;

            ClientInformationContract clientInformation;

            clientInformation = new ClientInformationContract()
            {
                EmitterType = subscription.ClientInformation.EmitterType,
                SpecificClientInformation = subscription.ClientInformation.SpecificClientInformation
            };

            

            if ((emitter = _emittersTable[clientInformation.EmitterType] as IEmitter) != null)
            {

                try
                {
                    using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
                    {

                        //
                        // Pass the transaction to the event, so the receiver can enrole on the transaction
                        //

                        @event.propagationToken = TransactionInterop.GetTransmitterPropagationToken(Transaction.Current);

                        emitter.Emit(clientInformation, @event);


                        try
                        {

                            //
                            // Confirm that event has been sent, so remove subscription from subscriptions to persist list of event
                            //

                            if (subscription.Persist)
                            {

                                MIB_DAL.EliminateTheInterestOfAnSubscriptionToAnEvent(@event, subscription);

                            }

                        }

                        //
                        // If the transaction has been aborted, the interest of the subscription to the event must be keeped
                        //

                        catch(Exception e)
                        {

                            //
                            // DO NOTHING, returning because the operation wasn't sucessfuly  
                            //

                            return;

                        }

                        transactionScope.Complete();

                    }
                }
                catch (TransactionAbortedException)
                {

                    //
                    // DO NOTHING, returning because the operation didn't commit the transaction 
                    //
                    
                }

                

            }      

        }

        private void Emit(Event @event, List<Subscription> subscriptions)
        {
            
            foreach (var subscription in subscriptions)
            {

               Emit(@event, subscription);

            }

        }

        #endregion

        #region Subscription

        private void Subscribe(Subscription subscription)
        {

            if (subscription.Unsubscribe)
            {

                _subscriptionTree.Remove(subscription);
                MIB_DAL.DeleteSubscription(subscription);

            }
            else
            {

                //
                // Subscription already exists, so its considered as an attemp to resume receive
                //

                if( _subscriptionTree.Add(subscription) && subscription.Persist )
                {

                    //
                    // Emit all event persisted to this subscription
                    //

                    MIB_DAL.GetEventsPersisted(subscription).ForEach( @event => Emit(@event, subscription ) );
                    
                }
                else
                {

                    MIB_DAL.SaveSubscription(subscription);  

                }
                

            }


        }

        private void Route()
        {
            
        }

        #endregion
    }

}
