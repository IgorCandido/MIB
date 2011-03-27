using System;

namespace Second.Interfaces.ClientNodeInformationTypes
{
    public class ClientInformation
    {

        public String emitterType { get; set; }

        public override string ToString()
        {
            return "emitterType: " + emitterType +"; ";
        }

    }
}
