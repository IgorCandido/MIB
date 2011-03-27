using System.ServiceModel;
using Second.Interfaces.Contracts;

namespace Second.Interfaces.ServiceInterfaces
{
    [ServiceContract]
    public interface IEmitter
    {

        [OperationContract]
        void Emit(ClientInformationContract clientInformation, object data);

    }
}
