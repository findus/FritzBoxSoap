using System;
using System.Xml;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace FritzBoxSoap
{
    public class OnlineMonitorInfo
    {
        private XmlNamespaceManager manager;
        private XmlDocument doc;

        public OnlineMonitorInfo(XmlDocument doc)
        {
            this.doc = doc;
            manager = new XmlNamespaceManager(doc.NameTable);
            manager.AddNamespace("dsl", "urn:dslforum-org:service:WANCommonInterfaceConfig:1");
        }

        private string getInfo(string str)
        {
            return doc.SelectSingleNode("//dsl:X_AVM-DE_GetOnlineMonitorResponse/" + str, manager).InnerText;
        }

        public List<long> getCurrentDownStreamRate()
        {
            var lst =  getInfo("Newds_current_bps").Split(',').OfType<string>().ToList();
            var lst2 = lst.Select(x => Convert.ToInt64(x)).ToList();
            return lst2;
        }

        public List<long> getCurrentUpstreamRate()
        {
            return getInfo("Newus_current_bps").Split(',').OfType<string>().ToList().Select(x => Convert.ToInt64(x)).ToList();
        }
    }
}