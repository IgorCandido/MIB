using System.ServiceModel;
using Interfaces.Contracts;

namespace Interfaces.Services
{
    [ServiceContract]
    public interface IEmitter
    {

        [OperationContract]
        void Emit(ClientInformationContract clientInformation, object data);

    }
}
