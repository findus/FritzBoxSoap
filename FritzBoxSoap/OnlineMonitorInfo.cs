using System.Xml;

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

        public string getCurrentDownStreamRate()
        {
            return getInfo("Newds_current_bps");
        }

        public string getCurrentUpstreamRate()
        {
            return getInfo("Newus_current_bps");
        }
    }
}