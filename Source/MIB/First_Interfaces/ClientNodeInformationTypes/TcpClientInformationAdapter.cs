using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using First.TCP_Receiver_Emitter;
using First_Interfaces.Contracts;

namespace First_Interfaces.ClientNodeInformationTypes
{
    public static class TcpClientInformationAdapter
    {

        public static TcpClientInformation ConstructFromData(String emitterType, byte[] data)
        {

            TcpClientInformation tcpClientInformation = new TcpClientInformation();

            tcpClientInformation.emitterType = emitterType;

            BinaryReader reader = new BinaryReader(new MemoryStream(data));

            tcpClientInformation.hostname = reader.ReadString();
            tcpClientInformation.port = reader.ReadInt32();

            return tcpClientInformation;

        }

        public static ClientInformationContract SerializeToClientInformation(TcpClientInformation clientInformation)
        {

            ClientInformationContract clientInformationContract = new ClientInformationContract();

            clientInformationContract.EmitterType = clientInformation.emitterType;

            MemoryStream serializationStream = new MemoryStream();

            BinaryWriter writer = new BinaryWriter(serializationStream);

            writer.Write(clientInformation.hostname);
            writer.Write(clientInformation.port);

            writer.Flush();
            writer.Close();
            
            clientInformationContract.SpecificClientInformation = serializationStream.ToArray();

            return clientInformationContract;

        }

    }
}
