using System;
namespace Client.Models
{
    public class LedgerTotals
    {
        public Stations Station { get; set; }
        public DateTime Date { get; set; }

        public Double Sale { get; set; }
        public Double Cash { get; set; }
        public Double Visa { get; set; }
        public Double Mpesa { get; set; }
        public Double POS { get; set; }
        public Double Invoice { get; set; }
        public Double Expense { get; set; }
        public Double Account { get; set; }
        public Double Summary { get; set; }
        public Double Discount { get; set; }

        public Double CarWash { get; set; }
        public Double TyreCtr { get; set; }
        public Double Service { get; set; }

        public LedgerTotals()
        {
            Station = new Stations();
            Date = DateTime.Now;
            Sale = 0;
            Cash = 0;
            Visa = 0;
            Mpesa = 0;
            POS = 0;
            Invoice = 0;
            Expense = 0;
            Account = 0;
            Summary = 0;
            Discount = 0;

            CarWash = 0;
            TyreCtr = 0;
            Service = 0;
        }
    }
}
