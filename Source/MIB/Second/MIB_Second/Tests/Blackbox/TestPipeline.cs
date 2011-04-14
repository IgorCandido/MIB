using System.Collections;
using Blackbox.Handlers;
using Blackbox.Pipeline;
using Interfaces.ClientInformationTypes;
using Interfaces.ContractsFromClients;
using Interfaces.ContractsInnerRepresentations;
using Interfaces.Services;
using NUnit.Framework;
using Rhino.Mocks;

namespace Tests.Blackbox
{

    [TestFixture]
    class TestPipeline
    {
        private delegate void EmittMockDelegate(ClientInformationContract clientInformation, object data);

        private byte[] data = {1,4,20,40};

        private TcpClientInformation TcpclientInformation;

        private Event @event;

        private void EmittMock(ClientInformationContract clientInformation, object data)
        {

            Assert.AreEqual(TcpclientInformation.EmitterType, clientInformation.EmitterType);
            Assert.AreEqual(TcpclientInformation,
                            TcpClientInformationAdapter.ConstructFromData(clientInformation.EmitterType,
                                                                          clientInformation.SpecificClientInformation));

            Assert.AreEqual(@event, data);

        }

        [Test]
        public void TestProcessingAnSubscription()
        {

            TcpclientInformation = new TcpClientInformation()
                                                         {EmitterType = "TCP", Hostname = "localhost", Port = 77};

            string contentDescription = "<description> <color>black</color> </description>";

            

            Subscription subscription = new Subscription(
                                    new SubscriptionContract() { 
                                        ContentDescription = contentDescription, 
                                        ClientInformation = TcpClientInformationAdapter.SerializeToClientInformation(TcpclientInformation) }
                                    );

            Hashtable emitters = new Hashtable();

            MockRepository mocks = new MockRepository();

            IEmitter emitter = mocks.StrictMock<IEmitter>();

            Expect.Call(() => emitter.Emit(null,null)).IgnoreArguments().Repeat.Any().Do(new EmittMockDelegate(EmittMock));

            mocks.ReplayAll();


            emitters["TCP"] = emitter;

            Pipeline pipe = PipelineFactory.CreatePipeline(new MatchingSubscribe(emitters));

            pipe.Process(subscription);

            @event = new Event(new EventContract() { ContentDescription = contentDescription , Data = data});

            pipe.Process(@event);

        }

        [Test]
        public void TestProcessingAnSubscriptionAndRemovingIt()
        {

            TcpclientInformation = new TcpClientInformation() { EmitterType = "TCP", Hostname = "localhost", Port = 77 };

            string contentDescription = "<description> <color>black</color> </description>";



            Subscription subscription = new Subscription(
                                    new SubscriptionContract()
                                    {
                                        ContentDescription = contentDescription,
                                        ClientInformation = TcpClientInformationAdapter.SerializeToClientInformation(TcpclientInformation)
                                    }
                                    );

            Hashtable emitters = new Hashtable();

            MockRepository mocks = new MockRepository();

            IEmitter emitter = mocks.StrictMock<IEmitter>();

            Expect.Call(() => emitter.Emit(null, null)).IgnoreArguments().Repeat.Once().Do(new EmittMockDelegate(EmittMock));

            mocks.ReplayAll();

            emitters["TCP"] = emitter;

            Pipeline pipe = PipelineFactory.CreatePipeline(new MatchingSubscribe(emitters));

            pipe.Process(subscription);

            @event = new Event(new EventContract() { ContentDescription = contentDescription, Data = data });

            pipe.Process(@event);

            subscription.Unsubscribe = true;

            pipe.Process(subscription);

            pipe.Process(@event);

            //
            // Verify if the subscription has been removed sucessfuly
            //

            mocks.Verify(emitter);

        }

        [Test]
        public void TestProcessingAnEventWithPartialValidSubscription()
        {

            TcpclientInformation = new TcpClientInformation() { EmitterType = "TCP", Hostname = "localhost", Port = 77 };

            string contentDescription = "<description> <color>black</color> </description>";



            Subscription subscription = new Subscription(
                                    new SubscriptionContract()
                                    {
                                        ContentDescription = contentDescription,
                                        ClientInformation = TcpClientInformationAdapter.SerializeToClientInformation(TcpclientInformation)
                                    }
                                    );

            Hashtable emitters = new Hashtable();

            MockRepository mocks = new MockRepository();

            IEmitter emitter = mocks.StrictMock<IEmitter>();

            Expect.Call(() => emitter.Emit(null, null)).IgnoreArguments().Repeat.Once().Do(new EmittMockDelegate(EmittMock));

            mocks.ReplayAll();

            emitters["TCP"] = emitter;

            Pipeline pipe = PipelineFactory.CreatePipeline(new MatchingSubscribe(emitters));

            pipe.Process(subscription);

            contentDescription = "<description> <color>black</color> <shape>rectangle</shape> </description>";

            @event = new Event(new EventContract() { ContentDescription = contentDescription, Data = data });

            pipe.Process(@event);

            //
            // Verify if event has been corresponded
            //

            mocks.Verify(emitter);

        }

        [Test]
        public void TestProcessingAnEventWithoutValidSubscription()
        {

            TcpclientInformation = new TcpClientInformation() { EmitterType = "TCP", Hostname = "localhost", Port = 77 };

            string contentDescription = "<description> <color>black</color> </description>";



            Subscription subscription = new Subscription(
                                    new SubscriptionContract()
                                    {
                                        ContentDescription = contentDescription,
                                        ClientInformation = TcpClientInformationAdapter.SerializeToClientInformation(TcpclientInformation)
                                    }
                                    );

            Hashtable emitters = new Hashtable();

            MockRepository mocks = new MockRepository();

            IEmitter emitter = mocks.StrictMock<IEmitter>();

            Expect.Call(() => emitter.Emit(null, null)).IgnoreArguments().Repeat.Never();

            mocks.ReplayAll();

            emitters["TCP"] = emitter;

            Pipeline pipe = PipelineFactory.CreatePipeline(new MatchingSubscribe(emitters));

            pipe.Process(subscription);

            contentDescription = "<description> <shape>rectangle</shape> </description>";

            @event = new Event(new EventContract() { ContentDescription = contentDescription, Data = data });

            pipe.Process(@event);

            //
            // Verify if event has been corresponded
            //

            mocks.Verify(emitter);

        }

        [Test]
        public void TestProcessingAnEventWithSubscriptionWithIncorrectValueToField()
        {

            TcpclientInformation = new TcpClientInformation() { EmitterType = "TCP", Hostname = "localhost", Port = 77 };

            string contentDescription = "<description> <color>black</color> </description>";



            Subscription subscription = new Subscription(
                                    new SubscriptionContract()
                                    {
                                        ContentDescription = contentDescription,
                                        ClientInformation = TcpClientInformationAdapter.SerializeToClientInformation(TcpclientInformation)
                                    }
                                    );

            Hashtable emitters = new Hashtable();

            MockRepository mocks = new MockRepository();

            IEmitter emitter = mocks.StrictMock<IEmitter>();

            Expect.Call(() => emitter.Emit(null, null)).IgnoreArguments().Repeat.Never();

            mocks.ReplayAll();

            emitters["TCP"] = emitter;

            Pipeline pipe = PipelineFactory.CreatePipeline(new MatchingSubscribe(emitters));

            pipe.Process(subscription);

            contentDescription = "<description> <color>red</color> </description>";

            @event = new Event(new EventContract() { ContentDescription = contentDescription, Data = data });

            pipe.Process(@event);

            //
            // Verify if event has been corresponded
            //

            mocks.Verify(emitter);

        }

        [Test]
        public void TestProcessingAnEventWithSubscriptionWithPartialMatching()
        {

            TcpclientInformation = new TcpClientInformation() { EmitterType = "TCP", Hostname = "localhost", Port = 77 };

            string contentDescription = "<description> <color>black</color> <shape>rectangle</shape> </description>";



            Subscription subscription = new Subscription(
                                    new SubscriptionContract()
                                    {
                                        ContentDescription = contentDescription,
                                        ClientInformation = TcpClientInformationAdapter.SerializeToClientInformation(TcpclientInformation)
                                    }
                                    );

            Hashtable emitters = new Hashtable();

            MockRepository mocks = new MockRepository();

            IEmitter emitter = mocks.StrictMock<IEmitter>();

            Expect.Call(() => emitter.Emit(null, null)).IgnoreArguments().Repeat.Never();

            mocks.ReplayAll();

            emitters["TCP"] = emitter;

            Pipeline pipe = PipelineFactory.CreatePipeline(new MatchingSubscribe(emitters));

            pipe.Process(subscription);

            contentDescription = "<description> <color>black</color> </description>";

            @event = new Event(new EventContract() { ContentDescription = contentDescription, Data = data });

            pipe.Process(@event);

            //
            // Verify if event has been corresponded
            //

            mocks.Verify(emitter);

        }

    }
}
