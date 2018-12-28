using System;

namespace Client.Models
{
    public class LedgerEntries
    {
        public Int64 Id { get; set; }
        public Int64 Action { get; set; }
        public Int64 Account { get; set; }
        public String Date { get; set; }
        public String Description { get; set; }
        public String Lpo { get; set; }
        public String Invoice { get; set; }
        public String Name { get; set; }
        public Double Quantity { get; set; }
        public Double Price { get; set; }
        public Double Amount { get; set; }
        public Double Discount { get; set; }
        public Stations Station { get; set; }
        public Customers Customer { get; set; }


        public LedgerEntries()
        {
            Id = 0;
            Action = 0;
            Account = 0;

            Date = DateTime.Now.ToString("dd/MM/yyyy");

            Description = "";
            Lpo = "";
            Invoice = "";
            Name = "";

            Quantity = 0;
            Price = 0;
            Amount = 0;
            Discount = 0;

            Station = new Stations();
            Customer = new Customers();
        }
    }
}
