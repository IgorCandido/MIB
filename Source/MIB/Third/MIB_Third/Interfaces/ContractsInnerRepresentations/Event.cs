using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces.ContractsFromClients;
using Interfaces.RequestHandling;

namespace Interfaces.ContractsInnerRepresentations
{

    [Serializable]
    public class Event : OperationContract
    {

        public byte[] Data { get; set; }

        public Event(EventContract eventContract)
        {

            Data = eventContract.Data;

            ContentDescription = ContentDescriptionParser.Parse(eventContract.ContentDescription);

        }

        #region OperationContract

        public override void HandleRequestVisit(IHandleRequest handler)
        {

            handler.HandleRequest(this);

        }

        #endregion

    }
}

