using ECommerceSample.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECommerceSample.Data.Repository
{
    public class OrderRepository : IRepository<Order> 
    {
        readonly IECommerceDataSource dataSource;
        public OrderRepository(IECommerceDataSource dataSource)
        {
            this.dataSource = dataSource;
        }
        public void Add(Order entity)
        {
            dataSource.Orders.Add(entity);
        }

        public IEnumerable<Order> Find(Func<Order, bool> predicate)
        {
            return dataSource.Orders.Where(predicate);
        }

        public Order GetById(object id)
        {
            return dataSource.Orders.FirstOrDefault(x => x.ProductCode == (string)id);
        }

        public void Remove(Order entity)
        {
            dataSource.Orders.Remove(entity);
        }
    }
}
