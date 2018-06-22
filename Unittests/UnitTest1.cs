using System;
using FritzBoxSoap;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unittests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var port = Requests.GetPort();
            Console.WriteLine(port);
        }
    }
}
