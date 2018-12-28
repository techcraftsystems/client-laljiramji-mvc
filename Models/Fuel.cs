using System;
namespace Client.Models
{
    public class Fuel
    {
        public Int64 Id { get; set; }
        public String Name { get; set; }

        public Fuel()
        {
            Id = 0;
            Name = "";
        }
    }
}
