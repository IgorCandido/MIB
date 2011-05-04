using System.ServiceModel;
using Interfaces.Contracts;
using Interfaces.ContractsFromClients;

namespace Interfaces.Services
{
    [ServiceContract]
    public interface IEmitter
    {

        [OperationContract]
        void Emit(ClientInformationContract clientInformation, object data);

    }
}
