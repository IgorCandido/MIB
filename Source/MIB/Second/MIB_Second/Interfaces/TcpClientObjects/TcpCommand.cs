using System;
using System.Runtime.Serialization;
using Interfaces.Tcp;

namespace Interfaces.Tcp
{

    public enum Commands
    {
        Event, Subscription
    }

}

namespace Interfaces.TcpClientObjects
{
    [Serializable]
    public class TcpCommand : ISerializable
    {

        public Commands Command { get; set; }

        public byte[] Payload { get; set; }

        public TcpCommand()
        {
            
        }

        protected TcpCommand(SerializationInfo info, StreamingContext context)
        {

            Command = (Commands) info.GetValue("Command", typeof (Commands));
            Payload = (byte[]) info.GetValue("Payload", typeof (byte[]));

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {

            info.AddValue("Command", Command);
            info.AddValue("Payload", Payload);

        }
    }
}
