using System;
namespace Client.Models
{
    public class PumpReadings
    {
        public Int64 Id { get; set; }
        public DateTime Date { get; set; }
        public Pump Pump { get; set; }
        public Double Price { get; set; }
        public Double Opening { get; set; }
        public Double Adjustment { get; set; }
        public Double Tests { get; set; }
        public Double Closing { get; set; }

        public PumpReadings()
        {
            Id = 0;
            Date = DateTime.Now;
            Price = 0;
            Opening = 0;
            Adjustment = 0;
            Tests = 0;
            Closing = 0;

            Pump = new Pump();
        }
    }
}
