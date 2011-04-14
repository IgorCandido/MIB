using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces.Contracts;
using Interfaces.ContractsFromClients;
using Interfaces.ContractsInnerRepresentations;
using Interfaces.RequestHandling;

namespace Blackbox.Pipeline
{
    public class Pipeline : IHandleRequest
    {

        private List<IPipelineEventHandler> _eventHandlers;

        public event Action<IPipelineContext> Authentication;

        public event Action<IPipelineContext> Authorization;

        public event Func<Event,IPipelineContext, List<ClientInformationContract>> Match;

        public event Action<Event,IPipelineContext> Persist;

        public event Action<Event,IPipelineContext> Forward;

        public event Action<Event,IPipelineContext, List<ClientInformationContract>> Emit;

        public event Action<Subscription,IPipelineContext> Subscribe;

        public event Action<Subscription,IPipelineContext> Route;

        public void Process(OperationContract request)
        {

            IPipelineContext context = null;

            //
            // Authenticate
            //

            if ( Authentication != null )
            {

                Authentication(context);

            }

            //
            // Authorize
            //

            if( Authorization != null )
            {

                Authorization(context);

            }

            //
            // Choose the operation type based on operation object concrete type
            //

            request.HandleRequestVisit(this,context);


        }

        
        #region Implementation of IHandleRequest

        //
        // Event handling
        //

        public void HandleRequest(Event @event, object args)
        {

            IPipelineContext context = (IPipelineContext) args;

            List<ClientInformationContract> clients = null;

            //
            // Match the event and store the information of matching clients
            //

            if( Match != null )
            {

                clients = Match(@event,context);

            }

            //
            // Persist event according to subscriptions needs, this information is stored on PipelineContext
            //

            if( Persist != null )
            {

                Persist(@event,context);

            }

            //
            // Forward event to other brokers
            //

            if( Forward != null )
            {

                Forward(@event,context);

            }

            //
            // Emit events according to matching clients information
            //

            if( Emit != null)
            {

                Emit(@event,context, clients);

            }

        }

        //
        // Subscription handling
        //

        public void HandleRequest(Subscription subscription, object args)
        {

            IPipelineContext context = (IPipelineContext) args;

            //
            // Handle the subscription
            //

            if( Subscribe != null )
            {

                Subscribe(subscription,context);

            }

            //
            // Route subscription to other brokers
            //

            if( Route != null )
            {

                Route(subscription,context);

            }


        }

        #endregion
    }

}
