using ECommerceSample.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECommerceSample.Data.Repository
{
    public class ProductRepository : IRepository<Product>
    {
        readonly IECommerceDataSource dataSource;
        public ProductRepository(IECommerceDataSource dataSource)
        {
            this.dataSource = dataSource;
        }
        public void Add(Product entity)
        {
            dataSource.Products.Add(entity);
        }

        public IEnumerable<Product> Find(Func<Product, bool> predicate)
        {
            return (IEnumerable<Product>)dataSource.Products.Where((Func<Product, bool>)predicate);
        }

        public Product GetById(object id)
        {
            return (Product)dataSource.Products.FirstOrDefault(x => x.Code == (string)id);
        }

        public void Remove(Product entity)
        {
            dataSource.Products.Remove((Product)entity);
        }
    }
}
