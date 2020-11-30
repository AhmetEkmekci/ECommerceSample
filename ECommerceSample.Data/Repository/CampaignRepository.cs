using ECommerceSample.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECommerceSample.Data.Repository
{
    public class CampaignRepository : IRepository<Campaign> 
    {
        readonly IECommerceDataSource dataSource;
        public CampaignRepository(IECommerceDataSource dataSource)
        {
            this.dataSource = dataSource;
        }
        public void Add(Campaign entity)
        {
            dataSource.Campaigns.Add(entity);
        }

        public IEnumerable<Campaign> Find(Func<Campaign, bool> predicate)
        {
            return (IEnumerable<Campaign>)dataSource.Campaigns.Where(predicate);
        }

        public Campaign GetById(object id)
        {
            return dataSource.Campaigns.FirstOrDefault(x => x.Name == (string)id);
        }

        public void Remove(Campaign entity)
        {
            dataSource.Campaigns.Remove(entity);
        }
    }
}
