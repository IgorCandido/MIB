using System;
using System.ServiceModel;
using Second.Interfaces.Contracts;

namespace Second.Interfaces.ServiceInterfaces
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
