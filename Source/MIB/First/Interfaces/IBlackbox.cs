using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using First.BlackBox;

namespace First.Interfaces
{
    interface IBlackbox
    {

        void Receive(String topic, object data);

        void Subscribe(String topic, IClientNode clientInformation);

    }
}
