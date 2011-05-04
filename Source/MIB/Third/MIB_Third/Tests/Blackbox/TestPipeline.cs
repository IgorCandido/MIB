using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Transactions;
using Blackbox.Data;
using Blackbox.Pipeline;
using Blackbox.SubscriptionsTree;
using Interfaces.ClientInformationTypes;
using Interfaces.ContractsFromClients;
using Interfaces.Services;
using NUnit.Framework;
using Rhino.Mocks;
using Event = Interfaces.ContractsInnerRepresentations.Event;
using Subscription = Interfaces.ContractsInnerRepresentations.Subscription;

namespace Tests.Blackbox
{

    [TestFixture]
    class TestPipeline
    {
        private delegate void EmittMockDelegate(ClientInformationContract clientInformation, object data);

        private byte[] data = {1,4,20,40};

        private TcpClientInformation TcpclientInformation;

        private Event @event;

        private volatile bool wait;

        private void EmittMock(ClientInformationContract clientInformation, object data)
        {

            Assert.AreEqual(TcpclientInformation.EmitterType, clientInformation.EmitterType);
            Assert.AreEqual(TcpclientInformation,
                            TcpClientInformationAdapter.ConstructFromData(clientInformation.EmitterType,
                                                                          clientInformation.SpecificClientInformation));

            Event eventReceived = (Event) data;

            Assert.AreEqual(@event, eventReceived);

            //
            // Clean the event transaction, so it can be reused
            //

            eventReceived.propagationToken = null;

        }

       
        private void EmittMockToKeepPersistence(ClientInformationContract clientInformation, object data)
        {

            Assert.AreEqual(TcpclientInformation.EmitterType, clientInformation.EmitterType);
            Assert.AreEqual(TcpclientInformation,
                            TcpClientInformationAdapter.ConstructFromData(clientInformation.EmitterType,
                                                                          clientInformation.SpecificClientInformation));

            Event eventReceived = (Event)data;

            Assert.NotNull(eventReceived.propagationToken);
            try
            {

                Transaction tran = TransactionInterop.GetTransactionFromTransmitterPropagationToken(eventReceived.propagationToken);

                using (TransactionScope ts = new TransactionScope(tran))
                {
                    
                }

            }
            catch (Exception)
            {
                
            }

            //
            // Clean the event transaction, so it can be reused
            //

            eventReceived.propagationToken = null;

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

            Pipeline pipe = new Pipeline(emitters);

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

            Pipeline pipe = new Pipeline(emitters);

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

            Pipeline pipe = new Pipeline(emitters);

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

            Pipeline pipe = new Pipeline(emitters);

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

            Pipeline pipe = new Pipeline(emitters);

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

            Pipeline pipe = new Pipeline(emitters);

            pipe.Process(subscription);

            contentDescription = "<description> <color>black</color> </description>";

            @event = new Event(new EventContract() { ContentDescription = contentDescription, Data = data });

            pipe.Process(@event);

            //
            // Verify if event has been corresponded
            //

            mocks.Verify(emitter);

        }

        [Test]
        public void TestProcessingSubscriptionAndEventsToBePersisted()
        {

            Hashtable emitters = new Hashtable();

            MockRepository mocks = new MockRepository();

            IEmitter emitter = mocks.StrictMock<IEmitter>();

            Expect.Call(() => emitter.Emit(null, null)).IgnoreArguments().Repeat.Once().Do(new EmittMockDelegate(EmittMockToKeepPersistence));

            mocks.ReplayAll();

            emitters["TCP"] = emitter;

            Pipeline pipe = new Pipeline(emitters);

            //
            // Add subscription with persistence required
            //

            TcpclientInformation = new TcpClientInformation() { EmitterType = "TCP", Hostname = "localhost", Port = 77 };

            string contentDescription = "<description> <color>black</color> <shape>rectangle</shape> </description>";



            Subscription subscription = new Subscription(
                                    new SubscriptionContract()
                                    {
                                        ContentDescription = contentDescription,
                                        ClientInformation = TcpClientInformationAdapter.SerializeToClientInformation(TcpclientInformation)
                                    }
                                    ){ Persist = true};

            @event = new Event(new EventContract() { ContentDescription = contentDescription, Data = data });

            MIB_DAL.DeleteSubscription(subscription);

            pipe.Process(subscription);

            wait = true;

            //
            // Send Events
            //

            pipe.Process(@event);

            //
            // Check if event is persisted
            //

            mocks.VerifyAll();

            Assert.NotNull(MIB_DAL.GetEvent(@event.GetHashCode()));

        }

        [Test]
        public void TestProcessingSubscriptionsAlreadySubscribed()
        {

            Hashtable emitters = new Hashtable();

            MockRepository mocks = new MockRepository();

            IEmitter emitter = mocks.StrictMock<IEmitter>();

            Expect.Call(() => emitter.Emit(null, null)).IgnoreArguments().Repeat.Times(2).Do(new EmittMockDelegate(EmittMockToKeepPersistence));

            mocks.ReplayAll();

            emitters["TCP"] = emitter;

            Pipeline pipe = new Pipeline(emitters);

            //
            // Add subscription with persistence required
            //

            TcpclientInformation = new TcpClientInformation() { EmitterType = "TCP", Hostname = "localhost", Port = 77 };

            string contentDescription = "<description> <color>black</color> <shape>rectangle</shape> </description>";



            Subscription subscription = new Subscription(
                                    new SubscriptionContract()
                                    {
                                        ContentDescription = contentDescription,
                                        ClientInformation = TcpClientInformationAdapter.SerializeToClientInformation(TcpclientInformation)
                                    }
                                    ) { Persist = true };

            @event = new Event(new EventContract() { ContentDescription = contentDescription, Data = data });

            MIB_DAL.DeleteSubscription(subscription);

            pipe.Process(subscription);

            wait = true;

            //
            // Send Events
            //

            pipe.Process(@event);

            //
            // Check if event is persisted
            //

            Assert.NotNull(MIB_DAL.GetEvent(@event.GetHashCode()));

            //
            // Add same subscription
            // 

            pipe.Process(subscription);

            mocks.VerifyAll();

        }

        [Test]
        public void TestProcessingUnSubscriptionWithEventsPersisted()
        {

            Hashtable emitters = new Hashtable();

            MockRepository mocks = new MockRepository();

            IEmitter emitter = mocks.StrictMock<IEmitter>();

            Expect.Call(() => emitter.Emit(null, null)).IgnoreArguments().Repeat.Any().Do(new EmittMockDelegate(EmittMockToKeepPersistence));

            mocks.ReplayAll();

            emitters["TCP"] = emitter;

            Pipeline pipe = new Pipeline(emitters);

            //
            // Add subscription with persistence required
            //

            TcpclientInformation = new TcpClientInformation() { EmitterType = "TCP", Hostname = "localhost", Port = 77 };

            string contentDescription = "<description> <color>black</color> <shape>rectangle</shape> </description>";
            string contentDescription1 = "<description> <shape>rectangle</shape> </description>";


            Subscription subscription = new Subscription(
                                    new SubscriptionContract()
                                    {
                                        ContentDescription = contentDescription,
                                        ClientInformation = TcpClientInformationAdapter.SerializeToClientInformation(TcpclientInformation)
                                    }
                                    ) { Persist = true };


            Subscription subscription1 = new Subscription(
                                    new SubscriptionContract()
                                    {
                                        ContentDescription = contentDescription1,
                                        ClientInformation = TcpClientInformationAdapter.SerializeToClientInformation(TcpclientInformation)
                                    }
                                    ) { Persist = true };

            MIB_DAL.DeleteSubscriptions( new List<Subscription>() {subscription, subscription1} );

            pipe.Process(subscription);

            pipe.Process(subscription1);

            wait = true;

            //
            // Send Events
            //

            @event = new Event(new EventContract() { ContentDescription = contentDescription, Data = data });
            Event @event1 = new Event(new EventContract() { ContentDescription = contentDescription1, Data = data });

            pipe.Process(@event);
            pipe.Process(@event1);

            //
            // Unsubscribe
            //

            subscription1.Unsubscribe = true;

            pipe.Process(subscription1);

            //
            // Verify
            //

            Assert.True(MIB_DAL.GetEvent( @event.GetHashCode()) != null);
            Assert.True(MIB_DAL.GetEvent(@event1.GetHashCode()) == null);

        }

    }
}
