using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Interfaces.ClientInformationTypes;
using Interfaces.ContractsFromClients;
using Interfaces.ContractsInnerRepresentations;
using Interfaces.Services;
using Interfaces.Tcp;
using Interfaces.Tcp.ClientObjects;
using NUnit.Framework;
using Rhino.Mocks;

namespace Tests.Blackbox
{

    [TestFixture]
    class TestTcpReceiver
    {

        private const int Portnumberusedbytcpreceiver = 60;

        private byte[] data = new byte[] {1, 234};

        private EventContract @event;

        private SubscriptionContract subscription;

        private Dictionary<String, String> expected = new Dictionary<string, string>();

        private TcpReceiver.TcpReceiver receiver;

        private TcpClientInformation info;

        private volatile bool responseReceived;

        private delegate void ProcessMockDelegate(OperationContract op);

        [TestFixtureSetUp]
        public void Setup()
        {

            string description = "<description> <color>black</color> <shape>rectangle</shape> </description>";

            @event = new EventContract()
                         {
                             ContentDescription = description,
                             Data = data
                         };

            info = new TcpClientInformation() {EmitterType = "TCP", Hostname = "localhost", Port = 66};

            subscription = new SubscriptionContract()
                               {
                                   ClientInformation = TcpClientInformationAdapter.SerializeToClientInformation(info),
                                   ContentDescription = description
                               };

            expected["color"] = "black";
            expected["shape"] = "rectangle";

            MockRepository mocks = new MockRepository();

            IBlackbox blackbox = mocks.StrictMock<IBlackbox>();

            Expect.Call(() => blackbox.Process(null)).IgnoreArguments().Repeat.Any().Do(new ProcessMockDelegate(ProcessMock));

            mocks.ReplayAll();

            receiver = new TcpReceiver.TcpReceiver(blackbox, Portnumberusedbytcpreceiver);

        }

        private void ProcessMock(OperationContract op)
        {

            Event eventReceived;

            Subscription subscriptionReceived;

            foreach (var contentDescriptionAttribute in op.ContentDescription)
            {

                Assert.IsNotNull(expected[contentDescriptionAttribute.Name]);
                Assert.AreEqual(expected[contentDescriptionAttribute.Name], contentDescriptionAttribute.Value);

            }

            if ( (subscriptionReceived = op as Subscription) != null)
            {

                TcpClientInformation tcpInfo = TcpClientInformationAdapter.ConstructFromData(subscriptionReceived.ClientInformation.EmitterType,
                                                              subscriptionReceived.ClientInformation.SpecificClientInformation);

                Assert.AreEqual(tcpInfo.EmitterType, info.EmitterType);
                Assert.AreEqual(tcpInfo.Hostname, info.Hostname);
                Assert.AreEqual(tcpInfo.Port, info.Port);

            }

            if( ( eventReceived = op as Event ) != null)
            {

                Assert.AreEqual(eventReceived.Data, data);

            }

            responseReceived = true;

        }
        
        [Test]
        public void TestReceiveEvent()
        {
            
            receiver.Receive(@event);

        }

        [Test]
        public void TestReceiveSubscription()
        {
            
            receiver.Subscribe(subscription);

        }

        [Test]
        public void TestReceiveSubscriptionThroughNetwork()
        {

            responseReceived = false;

            TcpClient client = new System.Net.Sockets.TcpClient("localhost", Portnumberusedbytcpreceiver);

            MemoryStream subscriptionStream = new MemoryStream();

            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(subscriptionStream, subscription);

            TcpCommand command = new TcpCommand() { Command = Commands.Subscription, Payload = subscriptionStream.GetBuffer() };

            formatter.Serialize(client.GetStream(), command);

            client.Close();

            while (!responseReceived)
            {
                
            }

        }

        [Test]
        public void TestReceiveEventThroughNetwork()
        {

            responseReceived = false;

            TcpClient client = new System.Net.Sockets.TcpClient("localhost", Portnumberusedbytcpreceiver);

            MemoryStream eventStream = new MemoryStream();

            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(eventStream, @event);

            TcpCommand command = new TcpCommand() { Command = Commands.Event, Payload = eventStream.GetBuffer() };

            formatter.Serialize(client.GetStream(), command);

            client.Close();

            //
            // This holds the unit test thread until the test completes on tcpReceiver thread
            //

            while (!responseReceived)
            {

            }

        }

    }
}
