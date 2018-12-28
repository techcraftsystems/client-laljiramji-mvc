using System;


namespace Client.Models
{
    public class StationsReconcile
    {
        public Stations Station { get; set; }
        public String Date { get; set; }
        public Double Amount { get; set; }
        public Double Payment { get; set; }
        public Double Uprna { get; set; }
        public Double Debt { get; set; }
        public Double Discount { get; set; }
        public Double Transport { get; set; }
        public Double Balance { get; set; }

        public StationsReconcile()
        {
            Station = new Stations();
            Date = DateTime.Now.ToString("dd/MM/yyyy");

            Amount = 0;
            Payment = 0;
            Uprna = 0;
            Debt = 0;
            Discount = 0;
            Transport = 0;
            Balance = 0;
        }
    }
}
