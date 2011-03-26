using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace First.BlackBox
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
