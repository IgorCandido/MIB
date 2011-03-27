using System;
using System.ServiceModel;

namespace Second.Interfaces.Factories
{
    public static class ProxyFactory
    {

        public static T GetProxy<T>(String clientName)
        {

            return new ChannelFactory<T>(clientName).CreateChannel();

        }

    }
}
