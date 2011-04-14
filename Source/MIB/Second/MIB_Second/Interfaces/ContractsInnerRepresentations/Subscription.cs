using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces.ContractsFromClients;
using Interfaces.RequestHandling;

namespace Interfaces.ContractsInnerRepresentations
{
    public class Subscription : OperationContract
    {

        public ClientInformationContract ClientInformation { get; set; }

        public bool Unsubscribe { get; set; }

        public Subscription(SubscriptionContract subscriptionContract, bool unsubscribe = false)
        {

            ClientInformation = subscriptionContract.ClientInformation;

            ContentDescription = ContentDescriptionParser.Parse(subscriptionContract.ContentDescription);

            Unsubscribe = unsubscribe;

        }

        #region OperationContract

        public override void HandleRequestVisit(IHandleRequest handler, object args)
        {

            handler.HandleRequest(this, args);

        }

        #endregion

        public override bool Equals(object obj)
        {

            Subscription arg;

            if( ( arg = obj as Subscription) != null )
            {

                return arg.ClientInformation.Equals(ClientInformation) && base.Equals(obj);

            }

            return base.Equals(obj);
        }

    }
}
