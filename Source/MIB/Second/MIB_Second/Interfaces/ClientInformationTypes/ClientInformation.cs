using System;

namespace Interfaces.ClientInformationTypes
{

    public class ClientInformation
    {

        public String EmitterType { get; set; }

        public override string ToString()
        {
            return "emitterType: " + EmitterType +"; ";
        }

    }
}
