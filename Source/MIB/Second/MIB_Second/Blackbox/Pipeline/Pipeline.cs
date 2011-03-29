using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces.Contracts;
using Interfaces.RequestHandling;

namespace Blackbox.Pipeline
{
    class Pipeline : IHandleRequest
    {

        public event Action<IPipelineContext> Authentication;

        public event Action<IPipelineContext> Authorization;

        public event Func<IPipelineContext, List<ClientInformationContract>> Match;

        public event Action<IPipelineContext> Persist;

        public event Action<IPipelineContext> Forward;

        public event Action<IPipelineContext, List<ClientInformationContract>> Emit;

        public event Action<IPipelineContext> Subscribe;

        public event Action<IPipelineContext> Route;

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

        //
        // Create default pipeline according to TODO: Definir critério para criar pipeline
        //

        public static Pipeline CreateDefaultPipeline()
        {

            return null;

        }

        #region Implementation of IHandleRequest

        //
        // Event handling
        //

        public void HandleRequest(EventContract eventContract, object args)
        {

            IPipelineContext context = (IPipelineContext) args;

            List<ClientInformationContract> clients = null;

            //
            // Match the event and store the information of matching clients
            //

            if( Match != null )
            {

                clients = Match(context);

            }

            //
            // Persist event according to subscriptions needs, this information is stored on PipelineContext
            //

            if( Persist != null )
            {

                Persist(context);

            }

            //
            // Forward event to other brokers
            //

            if( Forward != null )
            {

                Forward(context);

            }

            //
            // Emit events according to matching clients information
            //

            if( Emit != null)
            {

                Emit(context, clients);

            }

        }

        //
        // Subscription handling
        //

        public void HandleRequest(SubscriptionContract subscriptionContract, object args)
        {

            IPipelineContext context = (IPipelineContext) args;

            //
            // Handle the subscription
            //

            if( Subscribe != null )
            {

                Subscribe(context);

            }

            //
            // Route subscription to other brokers
            //

            if( Route != null )
            {

                Route(context);

            }


        }

        #endregion
    }

}
