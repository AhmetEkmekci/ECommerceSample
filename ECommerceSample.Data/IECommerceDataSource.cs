using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceSample.Data
{
    public interface IECommerceDataSource
    {
        public ICollection<Domain.Product> Products { get; }
        public ICollection<Domain.Order> Orders { get;  }
        public ICollection<Domain.Campaign> Campaigns { get; }
    }
}
