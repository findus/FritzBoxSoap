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
            var req = new SoapRequestSender("192.168.178.1", "-");
            using (var writer = new System.IO.StreamWriter("log.meem"))
            {
                writer.WriteLine("TEST");
                writer.WriteLine(req.GetOnlineMonitorInfo().getCurrentDownStreamRate());
                writer.WriteLine(req.GetOnlineMonitorInfo().getCurrentDownStreamRate());

                writer.WriteLine(req.GetOnlineMonitorInfo().getCurrentDownStreamRate());

                writer.WriteLine(req.GetOnlineMonitorInfo().getCurrentDownStreamRate());
                writer.WriteLine(req.GetOnlineMonitorInfo().getCurrentDownStreamRate());
                writer.WriteLine(req.GetOnlineMonitorInfo().getCurrentDownStreamRate());
                var str = req.GetOnlineMonitorInfo().getCurrentDownStreamRate();
                writer.WriteLine(str);
            }
               
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
