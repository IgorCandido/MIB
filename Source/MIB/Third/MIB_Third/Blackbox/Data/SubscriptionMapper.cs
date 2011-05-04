using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Blackbox.Data
{
    public static class SubscriptionMapper
    {

        public static Subscription Map(this Interfaces.ContractsInnerRepresentations.Subscription subscription)
        {
            
            BinaryFormatter formatter = new BinaryFormatter();

            MemoryStream memoryStream = new MemoryStream();

            formatter.Serialize(memoryStream,subscription);

            return new Subscription(){SubscriptionData = memoryStream.ToArray(), SubscriptionHash = subscription.GetHashCode()};

        }

        public static Interfaces.ContractsInnerRepresentations.Subscription Map(this Subscription subscription)
        {

            BinaryFormatter formatter = new BinaryFormatter();

            MemoryStream memoryStream = new MemoryStream(subscription.SubscriptionData.ToArray());

            return (Interfaces.ContractsInnerRepresentations.Subscription)formatter.Deserialize( memoryStream );

        }
    }
}
