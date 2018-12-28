using System;
namespace Client.Models
{
    public class TankSummary
    {
        public Tank Tank { get; set; }
        public Fuel Fuel { get; set; }
        public Double Opening { get; set; }
        public Double Sales { get; set; }
        public Double Delivery { get; set; }
        public Double Returns { get; set; }
        public Double Closing { get; set; }
        public Double Dips { get; set; }
        public Double Variance { get; set; }


        public TankSummary()
        {
            Tank = new Tank();
            Fuel = new Fuel();

            Opening = 0;
            Sales = 0;
            Delivery = 0;
            Returns = 0;
            Closing = 0;
            Dips = 0;
            Variance = 0;
        }
    }
}
