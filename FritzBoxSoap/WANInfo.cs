using System.Xml;

namespace FritzBoxSoap
{
    public class WANInfo
    {
        XmlNamespaceManager manager;
        XmlDocument doc;

        public WANInfo(XmlDocument doc)
        {
            this.doc = doc;
            manager = new XmlNamespaceManager(doc.NameTable);
            manager.AddNamespace("dsl", "urn:dslforum-org:service:WANDSLInterfaceConfig:1");
        }

        private string getInfo(string str)
        {
            return doc.SelectSingleNode("//dsl:GetInfoResponse/"+str, manager).InnerText;
        }

        public string getDownStreamRate()
        {
            return getInfo("NewDownstreamCurrRate");
        }

        public string getUpstreamRate()
        {
            return getInfo("NewUpstreamCurrRate");
        }
    }
}