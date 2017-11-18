using System;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Specialized;

namespace cdnetworks_dotnet
{
    public class CdnetworksConfig
    {
        public CdnetworksConfig()
        {
            output = "json";

            servicearea = ServiceArea.US;

        }
        public string user { get; set; }
        public string pass { get; set; }
        public string output { get; set; }
        public string servicearea { get; set; }
    }

    

    public class CdnetworksClient
    {
        CdnetworksConfig conf;
        public CdnetworksClient(CdnetworksConfig _conf)
        {
            conf = _conf;
        }

        private string http_send(String url, NameValueCollection nameValueCollection,string method="POST")
        {
            var parameters = new StringBuilder();

                foreach (string key in nameValueCollection.Keys)
                {

                    parameters.AppendFormat("{0}={1}&",
                        WebUtility.UrlEncode(key),
                        WebUtility.UrlEncode(nameValueCollection[key]));
                }

            parameters.Length -= 1;

            var request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.ContentType = "application/json; charset=utf-8";
            request.Method = "POST";

            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(parameters.ToString());
            }

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                return reader.ReadToEnd();
            }
            

           
        }


        /// <summary>
        /// Retrieves Pad List from CDNetworks
        /// </summary>
        /// <returns></returns>
        public string get_PadList()
        {
           
            string url = string.Format("{0}{1}",conf.servicearea,ApiFunction.PADLIST);
            var http_params = new NameValueCollection();
            http_params.Add("user", conf.user);
            http_params.Add("pass", conf.pass);
            http_params.Add("output", conf.output);
            return http_send(url,http_params );
        }

        /// <summary>
        /// Send Cache Purge Request to CDNetworks
        /// </summary>
        /// <param name="pad">Pad/Domain Name ( www.abc.com) </param>
        /// <param name="purge_type">item/wildcard/path <see cref="PurgeType"/>  </param>
        /// <param name="paths_to_be_purged">Each path must start with '/'</param>
        /// <param name="mail_to"></param>
        /// <returns></returns>
        public string send_Purge_Request(string pad, string purge_type, string[] paths_to_be_purged, string[] mail_to = null)
        {
            string url = string.Format("{0}{1}",conf.servicearea,ApiFunction.DOPURGE);
            var http_params = new NameValueCollection();
            http_params.Add("user", conf.user);
            http_params.Add("pass", conf.pass);
            http_params.Add("output", conf.output);
            http_params.Add("pad", pad);
            http_params.Add("type", purge_type);

            if (mail_to != null)
            {
                http_params.Add("mailTo", string.Join(",", mail_to));
            }
            foreach (var _path in paths_to_be_purged)
            {
                http_params.Add("path", _path);
            }

            return http_send(url, http_params);
        }

        public string get_Status_Of_Purge_Request(string purge_id)
        {
            string url = string.Format("{0}{1}", conf.servicearea, ApiFunction.STATUS);
            var http_params = new NameValueCollection();
            
            http_params.Add("user", conf.user);
            http_params.Add("pass", conf.pass);
            http_params.Add("output", conf.output);
            http_params.Add("pid", purge_id);
            return http_send(url, http_params);


        }
    }
}
