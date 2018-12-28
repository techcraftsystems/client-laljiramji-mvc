using System;
namespace Client.Models
{
    public class LegderSummary
    {
        public Int64 Type { get; set; }
        public Int64 Action { get; set; }
        public Customers Customer { get; set; }
        public Double FuelSales { get; set; }
        public Double LubeSales { get; set; }
        public Double Discounts { get; set; }

        public LegderSummary()
        {
            Customer = new Customers();

            Type = 0;
            Action = 0;
            FuelSales = 0;
            LubeSales = 0;
            Discounts = 0;
        }
    }
}
