using System;
using System.Runtime.Serialization;
using Interfaces.RequestHandling;

namespace Interfaces.Contracts
{
    [Serializable]
    [DataContract]
    public class SubscriptionContract : OperationContract
    {

        [DataMember]
        public String Topic { get; set; }

        [DataMember]
        public ClientInformationContract ClientInformation { get; set; }

        public override string ToString()
        {
            return "topic: " + Topic + ";";
        }

        #region OperationContract

        public override void HandleRequestVisit(IHandleRequest handler, object args)
        {

            handler.HandleRequest(this, args);

        }

        #endregion
    }
}
