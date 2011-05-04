using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Blackbox.Data
{
    public static class EventMapper
    {

        public static Event Map(this Interfaces.ContractsInnerRepresentations.Event @event)
        {

            BinaryFormatter formatter = new BinaryFormatter();

            MemoryStream memoryStream = new MemoryStream();

            formatter.Serialize(memoryStream, @event);

            return new Event() { EventData = memoryStream.ToArray(), EventHash = @event.GetHashCode() };

        }

        public static Interfaces.ContractsInnerRepresentations.Event Map(this Event @event)
        {

            BinaryFormatter formatter = new BinaryFormatter();

            MemoryStream memoryStream = new MemoryStream(@event.EventData.ToArray());

            return (Interfaces.ContractsInnerRepresentations.Event)formatter.Deserialize(memoryStream);

        }

    }
}
