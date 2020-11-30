using System;

namespace ECommerceSample.Domain
{
    public class Order
    {
        public DateTime OrderDate { get; set; }
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
    }
}
