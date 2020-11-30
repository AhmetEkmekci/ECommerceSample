using ECommerceSample.Domain;
using System.Collections.Generic;

namespace ECommerceSample.Data
{
    public class InMemoryECommerceDataSource : IECommerceDataSource
    {
        public InMemoryECommerceDataSource()
        {
            ProductList = new List<Product>();
            OrderList = new List<Order>();
            CampaignList = new List<Campaign>();
        }

        private List<Domain.Product> ProductList { get; set ; }
        private List<Domain.Order> OrderList { get; set; }
        private List<Domain.Campaign> CampaignList { get; set; }

        public ICollection<Product> Products { get => ProductList; }
        public ICollection<Order> Orders { get => OrderList; }
        public ICollection<Campaign> Campaigns { get => CampaignList; }
    }

}
