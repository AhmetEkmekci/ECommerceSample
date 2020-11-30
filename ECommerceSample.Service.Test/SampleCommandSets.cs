using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceSample.Service.Test
{
    public static class SampleData
    {
        public static readonly Dictionary<string, List<string>> CommandDictionary = new Dictionary<string, List<string>>()
        {
            {
                "CommandSet1",
                new List<string>()
{
"create_product P11 100 1000",
"create_campaign C11 P11 10 20 100",
"create_order P11 10",
"get_product_info P11",
"increase_time 1",
"create_order P11 15",
"get_product_info P11",
"increase_time 1",
"create_order P11 20",
"get_product_info P11",
"increase_time 1",
"create_order P11 25",
"get_product_info P11",
"increase_time 1",
"create_order P11 30",
"get_product_info P11",
"increase_time 1",
"create_order P11 30",
"get_product_info P11",
"increase_time 1",
"create_order P11 25",
"get_product_info P11",
"increase_time 1",
"create_order P11 20",
"get_product_info P11",
"increase_time 1",
"create_order P11 15",
"get_product_info P11",
"increase_time 1",
"create_order P11 10",
"get_product_info P11",
"increase_time 1",
"get_product_info P11",
"increase_time 1",
"get_campaign_info C11",
}
            },
            {
                "CommandSet2",
                new List<string>()
{
"create_product P4 100 1000",
"create_campaign C1 P4 5 20 100",
"get_product_info P4",
"increase_time 1",
"create_order P4 3",
"get_product_info P4",
"increase_time 1",
"get_product_info P4",
"increase_time 1",
"get_product_info P4",
"increase_time 1",
"get_product_info P4",
"increase_time 2",
"get_campaign_info C1",
}
            },
            {
                "CommandSet3",
                new List<string>()
{
"get_product_info",
}
            },
            {
                "CommandSet4",
                new List<string>()
{
"create_product P12 100 1000",
"create_order P12 10",
"increase_time 1",
"create_campaign C12 P12 10 20 100",
"create_order P12 10",
"increase_time 1",
"get_product_info P12",
"get_campaign_info C12",
}
            },
            {
                "CommandSet5",
                new List<string>()
{
"create_product P13 100 1000",
"create_order P13 10",
"increase_time 1",
"create_campaign C13 P13 10 20 100",
"create_order P13 10",
"increase_time 1",
"get_product_info P13",
"get_campaign_info C13",
"increase_time 1",
"create_order P13 10",
"get_product_info P13",
"get_campaign_info C13",
"increase_time 1",
"create_order P13 10",
"get_product_info P13",
"get_campaign_info C13",
"increase_time 1",
"create_order P13 10",
"get_product_info P13",
"get_campaign_info C13",
"increase_time 1",
"create_order P13 10",
"get_product_info P13",
"get_campaign_info C13",
}
            }
        };

        public static void LogParser(string Message)
        {
            System.Diagnostics.Trace.WriteLine(Message);

            Console.WriteLine(Message);
        }
    }
}
