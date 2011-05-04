using System;
using System.ServiceModel;
using Interfaces.Contracts;
using Interfaces.ContractsFromClients;
using Interfaces.ContractsInnerRepresentations;

namespace Interfaces.Services
{
    [ServiceContract]
    public interface IBlackbox
    {

        [OperationContract]
        void Process(OperationContract operation);
    }
}
