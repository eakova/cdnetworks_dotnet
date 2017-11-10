 
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

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
        /// <summary>
        /// Retrieves Pad List from CDNetworks
        /// </summary>
        /// <returns></returns>
        public string get_PadList()
        {
            string url = $"{conf.servicearea}{ApiFunction.PADLIST}";
            using (HttpClient _client = new HttpClient())
            {
                var request_content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("user", conf.user),
                    new KeyValuePair<string, string>("pass", conf.pass),
                    new KeyValuePair<string, string>("output", conf.output),
                }) ;
                var t1 = _client.PostAsync(url, request_content);
                t1.Wait();
                using (HttpResponseMessage res = t1.Result)
                {
                    using (HttpContent content = res.Content)
                    {
                        var t2= content.ReadAsStringAsync();
                        t2.Wait();
                        return t2.Result;
                    }
                }
            }

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
            string url = $"{conf.servicearea}{ApiFunction.DOPURGE}";

            using (HttpClient _client = new HttpClient())
            {
                var req_params = new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("user", conf.user),
                    new KeyValuePair<string, string>("pass", conf.pass),
                    new KeyValuePair<string, string>("output", conf.output),
                    new KeyValuePair<string, string>("pad", pad),
                    new KeyValuePair<string, string>("type", purge_type),
            };
                if (mail_to != null)
                {
                    req_params.Add(new KeyValuePair<string, string>("mailTo", string.Join(",", mail_to)));
                }
                foreach (var _path in paths_to_be_purged)
                {
                    req_params.Add(new KeyValuePair<string, string>("path",_path));
                }
                var request_content = new FormUrlEncodedContent(req_params);

                var t1 = _client.PostAsync(url, request_content);
                t1.Wait();
                using (HttpResponseMessage res = t1.Result)
                {
                    using (HttpContent content = res.Content)
                    {
                        var t2 = content.ReadAsStringAsync();
                        t2.Wait();
                        return t2.Result;
                    }
                }
            }
        }

        public string get_Status_Of_Purge_Request(string purge_id)
        {
            string url = $"{conf.servicearea}{ApiFunction.STATUS}";
            using (HttpClient _client = new HttpClient())
            {
                var request_content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("user", conf.user),
                    new KeyValuePair<string, string>("pass", conf.pass),
                    new KeyValuePair<string, string>("output", conf.output),
                    new KeyValuePair<string, string>("pid", purge_id),
                });
                var t1 = _client.PostAsync(url, request_content);
                t1.Wait();
                using (HttpResponseMessage res = t1.Result)
                {
                    using (HttpContent content = res.Content)
                    {
                        var t2 = content.ReadAsStringAsync();
                        t2.Wait();
                        return t2.Result;
                    }
                }
            }
        }
    }
}
