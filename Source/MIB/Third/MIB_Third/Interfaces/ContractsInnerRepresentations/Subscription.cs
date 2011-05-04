using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces.ContractsFromClients;
using Interfaces.RequestHandling;

namespace Interfaces.ContractsInnerRepresentations
{

    [Serializable]
    public class Subscription : OperationContract
    {

        private int _hashCode;

        public ClientInformationContract ClientInformation { get; set; }

        public bool Unsubscribe { get; set; }

        public bool Persist { get; set; }

        public Subscription(SubscriptionContract subscriptionContract, bool unsubscribe = false)
        {

            ClientInformation = subscriptionContract.ClientInformation;

            ContentDescription = ContentDescriptionParser.Parse(subscriptionContract.ContentDescription);

            _hashCode = (ContentDescriptionParser.GetContentDescriptionRepresentation(subscriptionContract.ContentDescription).ToString() 
                         + Persist ).GetHashCode();

            Unsubscribe = unsubscribe;

        }

        #region OperationContract

        public override void HandleRequestVisit(IHandleRequest handler)
        {

            handler.HandleRequest(this);

        }

        #endregion

        public override bool Equals(object obj)
        {

            Subscription arg;

            if( ( arg = obj as Subscription) != null )
            {

                return arg._hashCode == this._hashCode;

            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

    }
}
