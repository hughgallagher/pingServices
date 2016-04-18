using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PingServices
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceUrls = new Dictionary<string, string>
            {
                {"ESignature", "http://assu4-esign.tsrv.lan/services/ESignature.svc"},
                {"SSO", "http://assu4-sso.test.pcl.co.uk/AuthenticationService/SSO.svc"},
                {"Address Service", "http://ssu5-app-sfp.test.pcl.co.uk/External/AddressService.svc"},
                {"Doc Store Services", "http://172.20.20.50:8000/DocBank/LetterService.svc"},
                {"Real Time Service for Bank Validation", "http://10.10.110.51:8000/Service.asmx"},
                {"PDF XML Service", "https://www.s2.pclwebtest.co.uk/xml/xmlEntryPoint.php"},
                {"P&C Various Client Services", "https://ssu4-payments.tsrv.lan/Services/ClientTransactions.svc"},
                {"Message Gateway Email Service", "http://pclthpdevvt01.pcl.co.uk:8090/cc-webgui/app"}
            };


            foreach (var serviceUrl in serviceUrls)
            {
                PingService(serviceUrl);
            }

            Console.WriteLine("Checked all services..");
            Console.Read();
        }

        private static void PingService(KeyValuePair<string, string> serviceUrl)
        {
            try
            {
                var myRequest = (HttpWebRequest)WebRequest.Create(serviceUrl.Value);
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                var response = (HttpWebResponse)myRequest.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //  it's at least in some way responsive
                    //  but may be internally broken
                    //  as you could find out if you called one of the methods for real
                    Console.WriteLine(string.Format("{0} - {1} Available", serviceUrl.Key, serviceUrl.Value));
                }
                else
                {
                    //  well, at least it returned...
                    Console.WriteLine(string.Format("{0} - {1} Returned, but with status: {1}", serviceUrl.Key, serviceUrl.Value, response.StatusDescription));
                }
            }
            catch (Exception ex)
            {
                //  not available at all, for some reason
                Console.WriteLine(string.Format("{0} - {1} unavailable: {2}", serviceUrl.Key, serviceUrl.Value, ex.Message));
            }
        }
    }
}
