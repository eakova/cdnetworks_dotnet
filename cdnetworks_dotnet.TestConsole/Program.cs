using System;

namespace cdnetworks_dotnet.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            CdnetworksConfig config = new CdnetworksConfig()
            {
                user= "", // enter your cdnetworks email
                pass= "", // enter your cdnetworks password
                servicearea=ServiceArea.US, // service area
                output="json"
            };

            CdnetworksClient client = new CdnetworksClient(config);
            string pad_list_response= client.get_PadList();
            Console.WriteLine(pad_list_response);

            Console.ReadLine();
            var purge_response_json = client.send_Purge_Request("_enter_your_pad_domain_here", PurgeType.WILDCARD, new string[] { "/my_wild_card_url1*", "/my_wild_card_url2*" }, null);
            Console.WriteLine(purge_response_json);

            Console.ReadLine();
            string pid = ""; //"pid": 625976057,
            var purge_status = client.get_Status_Of_Purge_Request(pid);
            Console.WriteLine(purge_status);
            Console.ReadKey();
            
        }
    }
}
