using System.ServiceModel;
using First.BlackBox;
using First_Interfaces.ClientNodeInformationTypes;
using First_Interfaces.Contracts;

namespace First_Interfaces.ServiceInterfaces
{
    [ServiceContract]
    public interface IEmitter
    {

        [OperationContract]
        void Emit(ClientInformationContract clientInformation, object data);

    }
}
