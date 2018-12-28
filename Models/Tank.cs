using System;
namespace Client.Models
{
    public class Tank
    {
        public Int64 Id { get; set; }
        public String Name { get; set; }
        public Double Capacity { get; set; }
        public Fuel Fuel { get; set; }

        public Tank()
        {
            Id = 0;
            Name = "";
            Capacity = 0.0;

            Fuel = new Fuel();
        }
    }
}
