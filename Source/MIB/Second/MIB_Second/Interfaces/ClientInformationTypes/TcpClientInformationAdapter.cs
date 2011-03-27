using System;
using System.IO;
using Interfaces.Contracts;

namespace Interfaces.ClientInformationTypes
{
    public static class TcpClientInformationAdapter
    {

        public static TcpClientInformation ConstructFromData(String emitterType, byte[] data)
        {

            TcpClientInformation tcpClientInformation = new TcpClientInformation();

            tcpClientInformation.EmitterType = emitterType;

            BinaryReader reader = new BinaryReader(new MemoryStream(data));

            tcpClientInformation.Hostname = reader.ReadString();
            tcpClientInformation.Port = reader.ReadInt32();

            return tcpClientInformation;

        }

        public static ClientInformationContract SerializeToClientInformation(TcpClientInformation clientInformation)
        {

            ClientInformationContract clientInformationContract = new ClientInformationContract();

            clientInformationContract.EmitterType = clientInformation.EmitterType;

            MemoryStream serializationStream = new MemoryStream();

            BinaryWriter writer = new BinaryWriter(serializationStream);

            writer.Write(clientInformation.Hostname);
            writer.Write(clientInformation.Port);

            writer.Flush();
            writer.Close();
            
            clientInformationContract.SpecificClientInformation = serializationStream.ToArray();

            return clientInformationContract;

        }

    }
}
