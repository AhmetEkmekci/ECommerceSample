using ECommerceSample.Core;
using ECommerceSample.Data;
using ECommerceSample.Data.Repository;
using ECommerceSample.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECommerceSample.Service
{
    public delegate void LogParser(string Message);
    public class ECommerceBase
    {
        readonly LogParser logParser;
        readonly ProductRepository productRepository;
        readonly OrderRepository orderRepository;
        readonly CampaignRepository campaignRepository;

        private DateTime SystemDate { get; set; }

        public ECommerceBase(LogParser logParser, DateTime systemDate, ProductRepository productRepository, OrderRepository orderRepository, CampaignRepository campaignRepository)
        {
            this.logParser = logParser;
            this.SystemDate = systemDate;
            this.productRepository = productRepository;
            this.orderRepository = orderRepository;
            this.campaignRepository = campaignRepository;
        }

        public bool RunBatch(params string[] scenarioContent)
        {
            var CommandEntries = scenarioContent.SplitCommandEntries();
            var result = true;
            foreach (var commandEntry in CommandEntries)
            {
                var command = (CommandSet)Enum.Parse(typeof(CommandSet), commandEntry[0]);
                var commandParameters = commandEntry.Skip(1).ToList();
                var executeResult = ExecuteCommand(command, commandParameters);
                result = result && executeResult;
#if (DEBUG)
                logParser($"Command Log => [{string.Join(' ', commandEntry)}]\t {(executeResult ? "Success" : "Fail")}");
#endif
            }
            return result;
        }


        #region Commands
        public bool ExecuteCommand(CommandSet command, List<string> commandParameters)
        {
            return command switch
            {
                CommandSet.create_product => CreateProduct(commandParameters),
                CommandSet.get_product_info => GetProductInfo(commandParameters),
                CommandSet.create_order => CreateOrder(commandParameters),
                CommandSet.create_campaign => CreateCampaign(commandParameters),
                CommandSet.get_campaign_info => GetCampaignInfo(commandParameters),
                CommandSet.increase_time => IncreaseTime(commandParameters),
                _ => false,
            };
        }

        public bool CreateProduct(List<string> commandParameters)
        {
            if (commandParameters?.Count() < 3)
                return false;
            var product = new Product()
            {
                Code = commandParameters[0],
                Price = decimal.TryParse(commandParameters[1], out decimal price) ? price : default,
                Stock = int.TryParse(commandParameters[2], out int stock) ? stock : default,
            };

            if (productRepository.GetById(product.Code) != null)
                return false;

            productRepository.Add(product);

            logParser($"Product created; code {product.Code}, price {price}, stock {stock}");

            return true;
        }
        public bool GetProductInfo(List<string> commandParameters)
        {
            if (commandParameters?.Count() < 1)
                return false;

            var product = productRepository.GetById(commandParameters[0]);
            var price = product.Price - product.Price * ProductCurrentDiscountPercentage(product.Code, SystemDate) / 100;
            var stock = ProductCurrentStock(product);

            logParser($"Product {product.Code} info; price {price}; stock {stock}");

            return product != null;
        }
        public bool CreateOrder(List<string> commandParameters)
        {
            if (commandParameters?.Count() < 2)
                return false;

            var order = new Order()
            {
                ProductCode = commandParameters[0],
                Quantity = int.TryParse(commandParameters[1], out int stock) ? stock : default,
                OrderDate = SystemDate,
            };

            var product = productRepository.GetById(order.ProductCode);
            var productStock = ProductCurrentStock(product);
            if (productStock < order.Quantity)
                return false;

            orderRepository.Add(order);

            logParser($"Order created; product {order.ProductCode}, quantity {order.Quantity}");

            return true;
        }
        public bool CreateCampaign(List<string> commandParameters)
        {
            if (commandParameters?.Count() < 5)
                return false;

            var campaign = new Campaign()
            {
                Name = commandParameters[0],
                ProductCode = commandParameters[1],
                Duration = int.TryParse(commandParameters[2], out int duration) ? duration : default,
                PriceManipulationLimit = int.TryParse(commandParameters[3], out int priceManipulationLimit) ? priceManipulationLimit : default,
                TargetSalesCount = int.TryParse(commandParameters[4], out int targetSalesCount) ? targetSalesCount : default,
                StartDate = SystemDate,
            };

            if (campaignRepository.GetById(campaign.Name) != null)
                return false;

            var product = productRepository.GetById(campaign.ProductCode);
            var productStock = ProductCurrentStock(product);
            if (product == null || productStock < campaign.TargetSalesCount)
                return false;

            campaignRepository.Add(campaign);

            logParser($"Campaign created; name {campaign.Name}, product {campaign.ProductCode}, duration {campaign.Duration}, limit {campaign.PriceManipulationLimit}, target sales count {campaign.TargetSalesCount}");

            return true;
        }
        public bool GetCampaignInfo(List<string> commandParameters)
        {
            if (commandParameters?.Count() < 1)
                return false;

            var campaign = campaignRepository.GetById(commandParameters[0]);
            var status = CampaignStatus(campaign);
            var totalSales = CampaignTotalSales(campaign);
            var turnover = CampaignTurnover(campaign);
            var average = (int) turnover / totalSales;
            logParser($"Campaign {campaign.Name} info; Status {(status ? "Active" : "Ended")}, Target Sales {campaign.TargetSalesCount}, Total Sales {totalSales}, Turnover {turnover}, Average Item Price {average}");

            return true;
        }
        public bool IncreaseTime(List<string> commandParameters)
        {
            if (commandParameters?.Count() < 1)
                return false;

            SystemDate = SystemDate.AddHours(int.TryParse(commandParameters[0], out int duration) ? duration : default);
            logParser($"Time is {SystemDate:HH:mm}");

            return true;
        }
        #endregion


        #region Calculations
        int ProductCurrentDiscountPercentage(string productCode, DateTime priceDate)
        {
            if (productCode == null)
                return default;

            var campaign = campaignRepository
                .Find(x =>
                        x.ProductCode == productCode &&
                        x.StartDate.AddHours(x.Duration) > priceDate &&
                        x.TargetSalesCount > orderRepository.Find(y => y.ProductCode == x.ProductCode && y.OrderDate >= x.StartDate && y.OrderDate <= priceDate).Sum(x => x.Quantity)
                    )
                .OrderByDescending(x => x.PriceManipulationLimit)
                .FirstOrDefault();
            if (campaign == null)
                return default;
            else
                return (int)(campaign.PriceManipulationLimit * ((decimal)(priceDate.Subtract(campaign.StartDate).Hours + 1) / campaign.Duration));
        }
        int ProductCurrentStock(Product product)
        {
            if (product == null)
                return default;

            return product.Stock - orderRepository.Find(x => x.ProductCode == product.Code).Sum(x => x.Quantity);
        }
        bool CampaignStatus(Campaign campaign)
        {
            return 
                campaign.StartDate.AddHours(campaign.Duration) > SystemDate || 
                campaign.TargetSalesCount > orderRepository.Find(x=>x.OrderDate > campaign.StartDate).Sum(x=>x.Quantity);
        }
        int CampaignTotalSales(Campaign campaign)
        {
            var campaignTarget = campaign.TargetSalesCount;

            return orderRepository
                .Find(x => x.ProductCode == campaign.ProductCode && x.OrderDate >= campaign.StartDate && x.OrderDate <= campaign.StartDate.AddHours(campaign.Duration))
                .OrderBy(x=>x.OrderDate)
                .Where(x=> campaignTarget > 0 && (campaignTarget -= x.Quantity) >= 0)
                .Sum(x => x.Quantity);
        }
        decimal CampaignTurnover(Campaign campaign)
        {
            var productPrice = productRepository.GetById(campaign.ProductCode).Price;
            var campaignTarget = campaign.TargetSalesCount;
            
            return orderRepository
                .Find(x => x.ProductCode == campaign.ProductCode && x.OrderDate >= campaign.StartDate && x.OrderDate <= campaign.StartDate.AddHours(campaign.Duration))
                .OrderBy(x => x.OrderDate)
                .Where(x=> campaignTarget > 0 && (campaignTarget -= x.Quantity) >= 0)
                .Sum(x => (productPrice - productPrice * (decimal)ProductCurrentDiscountPercentage(x.ProductCode, x.OrderDate) / 100) * x.Quantity);
        }
        

        #endregion


    }
}
