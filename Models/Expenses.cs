using System;
namespace Client.Models
{
    public class Expenses
    {
        public Int64 Id { get; set; }
        public String Date { get; set; }
        public String Location { get; set; }
        public String Description { get; set; }
        public Double Amount { get; set; }
        public ChartsOfAccount Account { get; set; }
        public Stations Station { get; set; }

        public Expenses()
        {
            Id = 0;
            Date = DateTime.Now.ToString("dd/MM/yyyy");
            Location = "";
            Description = "";
            Amount = 0;

            Station = new Stations();
        }
    }
}
