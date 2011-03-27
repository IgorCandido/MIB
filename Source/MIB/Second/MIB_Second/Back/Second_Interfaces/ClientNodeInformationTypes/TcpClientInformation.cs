using System;

namespace Second.Interfaces.ClientNodeInformationTypes
{
    public class TcpClientInformation : ClientInformation
    {

        public String hostname { get; set; }

        public int port { get; set; }

        public override bool Equals(object obj)
        {
            TcpClientInformation clientInformation;

            if ((clientInformation = obj as TcpClientInformation) != null)
            {

                return clientInformation.port == port && clientInformation.hostname == hostname && clientInformation.emitterType == emitterType;

            }

            return base.Equals(obj);
        }

        public override string ToString()
        {
            return base.ToString() + "hostname: " + hostname + "; port: " + port +"; ";
        }

    }
}
