using System;
using System.Runtime.Serialization;
using Interfaces.RequestHandling;

namespace Interfaces.Contracts
{

    [Serializable]
    [DataContract]
    public class EventContract : OperationContract
    {

        [DataMember]
        public String Topic { get; set; }
        
        [DataMember]
        public byte[] Data { get; set; }

        #region Overrides of OperationContract

        public override void HandleRequestVisit(IHandleRequest handler, object args)
        {
            handler.HandleRequest(this, args);
        }

        #endregion
    }
}
