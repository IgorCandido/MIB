using System.Collections.Generic;
using System.Linq;
using Blackbox.SubscriptionsTree;
using Interfaces.ContractsFromClients;
using Interfaces.ContractsInnerRepresentations;
using NUnit.Framework;

namespace Tests.Blackbox
{

    [TestFixture]
    public class TestSubscriptionTree
    {
        private ClientInformationContract contact;

        private Subscription sub1;

        private Subscription sub2;

        private Subscription sub3;

        private Subscription sub4;

        private void AddMultipleSubscriptions(SubscriptionTree subTree)
        {

            subTree.Add(sub1);
            subTree.Add(sub2);
            subTree.Add(sub3);
            subTree.Add(sub4);

        }

        [SetUp]
        public void Init()
        {

            contact = new ClientInformationContract() {EmitterType = "Test"};

            sub1 = new Subscription(new SubscriptionContract() { ContentDescription = "<description> <color>black</color> </description>", ClientInformation = contact });

            sub2 = new Subscription(new SubscriptionContract() { ContentDescription = "<description> <shape>rectangle</shape></description>", ClientInformation = contact});

            sub3 = new Subscription(new SubscriptionContract() { ContentDescription = "<description> <color>red</color> <shape>rectangle</shape> </description>", ClientInformation = contact });

            sub4 = new Subscription(new SubscriptionContract() { ContentDescription = "<description> <color>black</color> <shape>rectangle</shape> </description>", ClientInformation = contact });



        }

        [Test]
        public void TestAddingOneSubscription()
        {
            
            SubscriptionTree subscriptionTree = new SubscriptionTree();

            SubscriptionContract subC = new SubscriptionContract()
                                            {
                                                ContentDescription = @"<description><color>black</color></description>",
                                                ClientInformation = contact
                                            };

            Subscription sub = new Subscription(subC);

            subscriptionTree.Add(sub);

            Assert.AreEqual(1,subscriptionTree.Get(new Event(new EventContract(){ContentDescription = subC.ContentDescription})).Count);

        }

        [Test]
        public void TestAddingMultipleSubscription()
        {

            SubscriptionTree subscriptionTree = new SubscriptionTree();

            AddMultipleSubscriptions(subscriptionTree);

            Event e1 = new Event(new EventContract() { ContentDescription = "<description> <color>black</color> </description>" });

            Event e2 = new Event(new EventContract() { ContentDescription = "<description> <shape>rectangle</shape></description>" });

            Event e3 = new Event(new EventContract() { ContentDescription = "<description> <color>red</color> <shape>rectangle</shape> </description>" });

            Event e4 = new Event(new EventContract() { ContentDescription = "<description> <color>black</color> <shape>rectangle</shape> </description>" });

            Assert.AreEqual(1, subscriptionTree.Get(e1).Count);
            Assert.AreEqual(1, subscriptionTree.Get(e2).Count);
            Assert.AreEqual(1, subscriptionTree.Get(e3).Count);
            Assert.AreEqual(2, subscriptionTree.Get(e4).Count);

        }

        [Test]
        public void TestGettingSubscriptions()
        {
            
            SubscriptionTree subscriptionTree = new SubscriptionTree();

            AddMultipleSubscriptions(subscriptionTree);

            Event expected =
                new Event(new EventContract() { ContentDescription = "<description> <color>black</color> </description>" });

            List<Subscription> list = subscriptionTree.Get(expected);

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(expected, list.First());

        }

        [Test]
        public void TestDeletingSubscriptions()
        {

            SubscriptionTree subscriptionTree = new SubscriptionTree();

            AddMultipleSubscriptions(subscriptionTree);

            Subscription toRemove =
                new Subscription(new SubscriptionContract() { ContentDescription = "<description> <color>red</color></description>", ClientInformation = contact });

            Event expected =
                new Event(new EventContract() { ContentDescription = "<description> <color>red</color></description>" });


            subscriptionTree.Remove(toRemove);

            List<Subscription> list = subscriptionTree.Get(expected);

            Assert.AreEqual(0, list.Count);

        }

        [Test]
        public void TestDeletingDontCare()
        {

            SubscriptionTree subscriptionTree = new SubscriptionTree();

            AddMultipleSubscriptions(subscriptionTree);

            Subscription toRemove =
                new Subscription(new SubscriptionContract() { ContentDescription = "<description> <shape>rectangle</shape> </description>", ClientInformation = contact });

            Event expected =
                new Event(new EventContract() { ContentDescription = "<description> <shape>rectangle</shape> </description>" });


            subscriptionTree.Remove(toRemove);

            List<Subscription> list = subscriptionTree.Get(expected);

            Assert.AreEqual(0, list.Count);
            
        }

    }
}
