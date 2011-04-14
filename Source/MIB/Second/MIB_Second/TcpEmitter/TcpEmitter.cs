using System.IO;
using System.Net.Sockets;
using Interfaces.ClientInformationTypes;
using Interfaces.Contracts;
using Interfaces.ContractsFromClients;
using Interfaces.Services;

namespace TcpEmitter
{
    class TcpEmitter : IEmitter
    {
        #region Implementation of IEmitter

        public void Emit(ClientInformationContract clientInformation, object data)
        {
            
            if( clientInformation.EmitterType == "TCP" )
            {
                Emit(TcpClientInformationAdapter.ConstructFromData(
                                                clientInformation.EmitterType,
                                                clientInformation.SpecificClientInformation),
                     data);
            }

        }

        #endregion

        private void Emit(TcpClientInformation clientInformation, object data)
        {

            TcpClient client = new TcpClient();
            
            client.Connect(clientInformation.Hostname, clientInformation.Port);
            
            BinaryWriter writer = new BinaryWriter(client.GetStream()); 

            writer.Write((byte[])data);

            client.GetStream().Flush();
               
            client.Close();

        }
    }
}
