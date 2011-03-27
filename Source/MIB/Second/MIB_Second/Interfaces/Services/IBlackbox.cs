using System;
using System.ServiceModel;
using Interfaces.Contracts;

namespace Interfaces.Services
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
