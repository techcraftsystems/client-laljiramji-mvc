using System;
namespace Client.Models
{
    public class Pump
    {
        public Int64 Id { get; set; }
        public String Name { get; set; }

        public Pump()
        {
            Id = 0;
            Name = "";
        }
    }
}
