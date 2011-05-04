using System;

namespace Interfaces.ClientInformationTypes
{

    public abstract class ClientInformation
    {

        public String EmitterType { get; set; }

        public override string ToString()
        {
            return "emitterType: " + EmitterType +"; ";
        }

    }
}
