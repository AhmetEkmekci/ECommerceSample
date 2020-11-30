using ECommerceSample.Service;
using System;

namespace ECommerceSample
{
    class Program
    {
        //Takes Scenario file path.
        static void Main(string[] args)
        {
            Console.WriteLine("E-Commerce Sample.");

            if (args == null || args.Length == 0)
                throw new ArgumentException("A scenario file should be passed as an argument.");

            var dataSource = new Data.InMemoryECommerceDataSource();
            var productRepository = new Data.Repository.ProductRepository(dataSource);
            var orderRepository = new Data.Repository.OrderRepository(dataSource);
            var campaignRepository = new Data.Repository.CampaignRepository(dataSource);
            var systemDate = DateTime.Now.Date;

            var commandArray = string.Join(' ', args).GetFileContent();

            var eCommerceBase = new ECommerceSample.Service.ECommerceBase(
                    LogParser, systemDate, productRepository, orderRepository, campaignRepository
                );

            eCommerceBase.RunBatch(commandArray);

;
        }
        private static void LogParser(string Message)
        {
            System.Diagnostics.Trace.WriteLine(Message);

            Console.WriteLine(Message);
        }
    }
}
