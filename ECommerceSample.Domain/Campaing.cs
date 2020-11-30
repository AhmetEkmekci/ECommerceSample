using System;

namespace ECommerceSample.Domain
{
    public class Campaign
    {
        public string Name { get; set; }
        public string ProductCode { get; set; }
        public DateTime StartDate { get; set; }
        public int Duration { get; set; }
        public int PriceManipulationLimit { get; set; }
        public int TargetSalesCount { get; set; }
    }
}
