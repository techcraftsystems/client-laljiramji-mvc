using System;
namespace Client.Models
{
    public class Products
    {
        public Int64 Id { get; set; }
        public String Name { get; set; }
        public String Category { get; set; }

        public Products()
        {
            Id = 0;
            Name = "";
            Category = "";
        }
    }
}
