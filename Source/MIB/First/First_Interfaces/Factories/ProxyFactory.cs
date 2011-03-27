using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace First_Interfaces
{
    public static class ProxyFactory
    {

        public static T GetProxy<T>(String clientName)
        {

            return new ChannelFactory<T>(clientName).CreateChannel();

        }

    }
}
