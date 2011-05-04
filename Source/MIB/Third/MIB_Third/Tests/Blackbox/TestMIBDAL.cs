using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using Interfaces.ContractsFromClients;
using Interfaces.ContractsInnerRepresentations;
using NUnit.Framework;
using Blackbox.Data;
using Subscription = Interfaces.ContractsInnerRepresentations.Subscription;

namespace Tests.Blackbox
{
    [TestFixture]
    class TestMIBDAL
    {

        private ClientInformationContract _contact;

        private Subscription _sub1;

        private Subscription _sub2;

        [SetUp]
        public void Init()
        {

            _contact = new ClientInformationContract() { EmitterType = "Test" };

            _sub1 = new Subscription(new SubscriptionContract() { ContentDescription = "<description> <color>black</color> </description>", ClientInformation = _contact });

            _sub2 = new Subscription(new SubscriptionContract() { ContentDescription = "<description> <shape>rectangle</shape></description>", ClientInformation = _contact });

            List<Subscription> inDb = MIB_DAL.GetAllSubscriptions();

            foreach (var subscription in inDb)
            {
                MIB_DAL.DeleteSubscription(subscription);
            }

        }

        [Test]
        public void TestInsertSubscriptions()
        {

           
            MIB_DAL.SaveSubscriptions(new List<Subscription>(){_sub1, _sub2});

            List<Subscription> actual = MIB_DAL.GetAllSubscriptions();

            Assert.AreEqual(2, actual.Count);
            Assert.True(actual.Contains(_sub1) &&  actual.Contains(_sub2));

            MIB_DAL.DeleteSubscription(_sub1);
            MIB_DAL.DeleteSubscription(_sub2);

        }


        [Test]
        public void TestDeleteSubscriptionFromDb()
        {

            MIB_DAL.SaveSubscriptions(new List<Subscription>(){_sub1});

            MIB_DAL.DeleteSubscription(_sub1);

            List<Subscription> actual = MIB_DAL.GetAllSubscriptions();

            Assert.False(actual.Contains(_sub1));

        }

        

    }
}
