using System;

namespace Interfaces.ContractsFromClients
{

    [Serializable]
    public class ClientInformationContract
    {

        public String EmitterType { get; set; }

        public byte[] SpecificClientInformation { get; set; }

        public override bool Equals(object obj)
        {

            ClientInformationContract arg;

            if( ( arg = obj as ClientInformationContract ) != null)
            {

                return arg.EmitterType.Equals(EmitterType) && ( ( SpecificClientInformation == null && arg.SpecificClientInformation == null ) ||
                       SpecificClientInformation.Equals(arg.SpecificClientInformation) );

            }

            return base.Equals(obj);
        }

    }
}
