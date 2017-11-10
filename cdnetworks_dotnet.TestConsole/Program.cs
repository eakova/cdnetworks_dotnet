using System;

namespace cdnetworks_dotnet.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            CdnetworksConfig config = new CdnetworksConfig()
            {
                user= "_cdnetworks_user_email",
                pass= "_cdnetworks_user_password",
                servicearea=ServiceArea.US, // service area
                output="json"
            };

            CdnetworksClient client = new CdnetworksClient(config);
            string pad_list_response= client.get_PadList();
            Console.WriteLine(pad_list_response);

            var purge_response_json = client.send_Purge_Request("www.abc.com", PurgeType.WILDCARD, new string[] { "/my_wild_card_url1*", "/my_wild_card_url2*" }, null);
            Console.WriteLine(purge_response_json);


            string pid = ""; //"pid": 625976057,
            var purge_status = client.get_Status_Of_Purge_Request(pid);
            Console.WriteLine(purge_status);
            Console.ReadKey();
            
        }
    }
}
