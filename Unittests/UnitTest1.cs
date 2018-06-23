using System;
using FritzBoxSoap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using System.Threading;

namespace Unittests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
         
               
        }

        [TestMethod]
        public void PercentageDl()
        {
            FritzBoxSoap.FritzBoxSoap so = new FritzBoxSoap.FritzBoxSoap("192.168.178.1", "-");
            double d = so.getPercentageUsageDownStream();
            Console.WriteLine(d);
        }
    }
}
