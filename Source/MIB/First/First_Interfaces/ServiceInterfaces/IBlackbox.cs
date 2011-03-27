using System;
using System.ServiceModel;
using First.BlackBox;
using First_Interfaces.ClientNodeInformationTypes;
using First_Interfaces.Contracts;

namespace First_Interfaces.ServiceInterfaces
{
    [ServiceContract]
    public interface IBlackbox
    {

        [OperationContract]
        void Receive(String topic, object data);

        [OperationContract]
        void Subscribe(String topic, ClientInformationContract clientInformation);

    }
}
