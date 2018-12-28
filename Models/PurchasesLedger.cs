using System;

namespace Client.Models
{
    public class PurchasesLedger
    {
        public Int64 Id { get; set; }
        public String Type { get; set; }
        public String Date { get; set; }
        public String Invoice { get; set; }
        public Double Quantity { get; set; }
        public Double Purchase { get; set; }
        public Double Delivery { get; set; }
        public Double Price { get; set; }
        public Double Total { get; set; }

        public Stations Station { get; set; }
        public Fuel Fuel { get; set; }
        public Suppliers Supplier { get; set; }


        public PurchasesLedger()
        {
            Id = 0;
            Type = "";
            Date = DateTime.Now.ToString("dd.MM.yyyy");
            Invoice = "";
            Quantity = 0;
            Purchase = 0;
            Delivery = 0;
            Price = 0;
            Total = 0;

            Station = new Stations();
            Fuel = new Fuel();
            Supplier = new Suppliers();
        }
    }
}
