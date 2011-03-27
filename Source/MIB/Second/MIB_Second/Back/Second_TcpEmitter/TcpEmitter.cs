using System.IO;
using System.Net.Sockets;
using Second.Interfaces.ServiceInterfaces;

namespace Second.TcpEmitter
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
            
            client.Connect(clientInformation.hostname, clientInformation.port);
            
            BinaryWriter writer = new BinaryWriter(client.GetStream()); 

            writer.Write((byte[])data);

            client.GetStream().Flush();
               
            client.Close();

        }
    }
}
