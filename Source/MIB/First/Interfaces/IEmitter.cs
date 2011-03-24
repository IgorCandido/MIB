using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using First.BlackBox;

namespace First.Interfaces
{
    interface IEmitter
    {

        void Emit(IClientNode clientInformation, object data);

    }
}
