using System;
using System.Net;
using System.Xml;

namespace FritzBoxSoap
{
    public class Requests
    {
        private static string SendSoapRequest(String url, WebHeaderCollection headers, String body)
        {
            WebClient client = new WebClient();

            client.Encoding = System.Text.Encoding.UTF8;
            client.Headers = headers;
            var mem = client.UploadString(url, body);
            return mem;
        }

        public static string GetPort()
        {

            var ip = "192.168.178.1";

            var url = "http://" + ip + ":49000/upnp/control/deviceinfo";

            var header = new WebHeaderCollection();
            header.Add("Content-Type", "text/xml; charset='utf-8'");
            header.Add("SOAPACTION", "urn:dslforum-org:service:DeviceInfo:1#GetSecurityPort");

            var body = @"<?xml version=""1.0""?>
                            <s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"" s:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                                <s:Body>
                                    <u:GetSecurityPort xmlns:u=""urn:dslforum-org:service:DeviceInfo:1"">
                                    </u:GetSecurityPort>
                                </s:Body>
                        </s:Envelope>".Replace(System.Environment.NewLine, "");

            var str = SendSoapRequest(url, header,body);
            var xml = getSoapLetter(str);

            XmlNamespaceManager manager = new XmlNamespaceManager(xml.NameTable);
            manager.AddNamespace("dsl", "urn:dslforum-org:service:DeviceInfo:1");

            XmlNode list = xml.SelectSingleNode("//dsl:GetSecurityPortResponse",manager);

            return list.InnerText;


        }

        private static XmlDocument getSoapLetter(string str)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(str);
            return doc;
        }
    }
}
