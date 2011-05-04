using System;

namespace Interfaces.ClientInformationTypes
{

    public class TcpClientInformation : ClientInformation
    {

        public String Hostname { get; set; }

        public int Port { get; set; }

        public override bool Equals(object obj)
        {
            TcpClientInformation clientInformation;

            if ((clientInformation = obj as TcpClientInformation) != null)
            {

                return clientInformation.Port == Port && clientInformation.Hostname == Hostname && clientInformation.EmitterType == EmitterType;

            }

            return base.Equals(obj);
        }

        public override string ToString()
        {
            return base.ToString() + "hostname: " + Hostname + "; port: " + Port +"; ";
        }

    }
}
