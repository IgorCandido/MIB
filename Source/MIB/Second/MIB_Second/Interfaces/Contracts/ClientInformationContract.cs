using System;

namespace Interfaces.Contracts
{

    [Serializable]
    public class ClientInformationContract
    {

        public String EmitterType { get; set; }

        public byte[] SpecificClientInformation { get; set; }

    }
}
