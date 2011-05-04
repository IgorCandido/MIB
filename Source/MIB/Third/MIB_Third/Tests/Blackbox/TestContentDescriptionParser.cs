using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces.ContractsInnerRepresentations;
using NUnit.Framework;

namespace Tests.Blackbox
{

    [TestFixture]
    class TestContentDescriptionParser
    {

        [Test]
        public void TestParse()
        {

            Dictionary<string, string> testDictionary = new Dictionary<string, string>();

            testDictionary["test"] = "valuetest";
            testDictionary["test1"] = "valuetest2";
            
            string description = "<description> <test>valuetest</test> <test1>valuetest2</test1> </description>";

            List<ContentDescriptionAttribute> contentDescription = ContentDescriptionParser.Parse(description);

            Assert.AreEqual(2, contentDescription.Count);

            Assert.AreEqual(testDictionary[contentDescription[0].Name],contentDescription[0].Value);
            Assert.AreEqual(testDictionary[contentDescription[1].Name], contentDescription[1].Value);

        }
    }
}
