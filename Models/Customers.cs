using System;
namespace Client.Models
{
    public class Customers
    {
        public Int64 Id { get; set; }
        public DateTime DateJoined { get; set; }
        public DateTime LastInvoice { get; set; }
        public String Name { get; set; }
        public String Contacts { get; set; }
        public String Telephone { get; set; }
        public String KraPin { get; set; }
        public Double Balance { get; set; }
        public Double CreditLimit { get; set; }
        public Stations Station { get; set; }

        public Customers()
        {
            Id = 0;
            DateJoined = DateTime.Now;
            LastInvoice = DateTime.Now;
            Name = "";
            Contacts = "";
            Telephone = "";
            KraPin = "";
            Balance = 0;
            CreditLimit = 0;
            Station = new Stations();
        }

        public Customers(Int64 idnt) : this(){
            Id = idnt;
        }
    }
}
