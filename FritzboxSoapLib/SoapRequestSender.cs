using System;
using System.Net;
using System.Xml;
using System.Threading;
using System.Net.Security;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FritzBoxSoap
{

    public class WebClient : System.Net.WebClient
    {
        public WebClient()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            this.container = new CookieContainer();
        }
        public WebClient(CookieContainer container)
        {
            this.container = container;
        }

        public CookieContainer CookieContainer
        {
            get { return container; }
            set { container = value; }
        }

        private CookieContainer container = new CookieContainer();

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest r = base.GetWebRequest(address);
            var request = r as HttpWebRequest;
            if (request != null)
            {
                request.CookieContainer = container;
            }
            return r;
        }

        protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
        {
            WebResponse response = base.GetWebResponse(request, result);
            ReadCookies(response);
            return response;
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse response = base.GetWebResponse(request);
            ReadCookies(response);
            return response;
        }

        private void ReadCookies(WebResponse r)
        {
            var response = r as HttpWebResponse;
            if (response != null)
            {
                CookieCollection cookies = response.Cookies;
                container.Add(cookies);
            }
        }
    }

    public class SoapRequestSender
    {

        private Dictionary<String, DateTime> timewatchdict = new Dictionary<string, DateTime>();
        private Dictionary<String, String> cache = new Dictionary<string, String>();

        private string ip;
        private string pw;
        public static string user = "dslf-config";

        private string port = "";

        public SoapRequestSender(string ip, string password)
        {
            this.ip = ip;
            this.pw = password;
        }

        private string SendSoapRequest(String url, WebHeaderCollection headers, String body, NetworkCredential cred)
        {

            ServicePointManager.ServerCertificateValidationCallback =
           new RemoteCertificateValidationCallback(
                delegate
                { return true; }
            );

            if (this.cache.ContainsKey(url) && (DateTime.Now - this.timewatchdict[url]).TotalSeconds < 5)
            {
                Console.WriteLine("Using Cached entry");
                return this.cache[url];

            } else
            {
                Console.WriteLine("Cache expired redownload");
                var sslFailureCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                ServicePointManager.ServerCertificateValidationCallback += sslFailureCallback;

                WebClient client = new WebClient();

                client.Encoding = System.Text.Encoding.UTF8;
                client.Headers = headers;
                if (cred != null)
                    client.Credentials = cred;

                var mem = client.UploadString(url, body);
                cache[url] = mem;
                timewatchdict[url] = DateTime.Now;
                return mem;
            }         
        }

        private  string GetPort()
        {
            if(!port.Equals(""))
            {
                return port;
            } else
            {
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

                var str = SendSoapRequest(url, header, body, null);
                var xml = getSoapLetter(str);

                XmlNamespaceManager manager = new XmlNamespaceManager(xml.NameTable);
                manager.AddNamespace("dsl", "urn:dslforum-org:service:DeviceInfo:1");

                XmlNode list = xml.SelectSingleNode("//dsl:GetSecurityPortResponse", manager);

                return list.InnerText;
            }  
        }

        public WANInfo GetWANInfo()
        {
            var url = "https://" + ip + ":" + GetPort() + "/upnp/control/wandslifconfig1";

            var header = new WebHeaderCollection();
            header.Add("Content-Type", "text/xml; charset='utf-8'");
            header.Add("SOAPACTION", "urn:dslforum-org:service:WANDSLInterfaceConfig:1#GetInfo");

            NetworkCredential cred = new NetworkCredential(user, pw);

            var body = @"<?xml version=""1.0""?>
                            <s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"" s:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                                    <s:Body>
                                        <u:GetInfo xmlns:u=""urn:dslforum-org:service:WANDSLInterfaceConfig:1"">
                                        </u:GetInfo>
                                    </s:Body>
                             </s:Envelope>".Replace(System.Environment.NewLine, "");

            var str = SendSoapRequest(url, header, body, cred);
            var xml = getSoapLetter(str);

            return new WANInfo(xml);
        }

        public OnlineMonitorInfo GetOnlineMonitorInfo()
        {

            var url = "https://" + ip + ":" + GetPort() + "/upnp/control/wancommonifconfig1";

            var header = new WebHeaderCollection();
            header.Add("Content-Type", "text/xml; charset='utf-8'");
            header.Add("SOAPACTION", "urn:dslforum-org:service:WANCommonInterfaceConfig:1#X_AVM-DE_GetOnlineMonitor");

            NetworkCredential cred = new NetworkCredential(user, pw);

            var body = @"<?xml version=""1.0""?>
                            <s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"" s:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                                <s:Body>
                                    <u:X_AVM-DE_GetOnlineMonitor xmlns:u=""urn:dslforum-org:service:WANDSLInterfaceConfig:1"">
                                        <NewSyncGroupIndex>0</NewSyncGroupIndex>
                                    </u:X_AVM-DE_GetOnlineMonitor>
                                </s:Body>
                            </s:Envelope>".Replace(System.Environment.NewLine, "");

            var str = SendSoapRequest(url, header, body, cred);
            var xml = getSoapLetter(str);

            return new OnlineMonitorInfo(xml);
        }

        private XmlDocument getSoapLetter(string str)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(str);
            return doc;
        }
    }
}
