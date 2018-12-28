using System;
namespace Client.Models
{
    public class Suppliers
    {
        public Int64 Id { get; set; }
        public String Name { get; set; }

        public Suppliers()
        {
            Id = 0;
            Name = "";
        }
    }
}
