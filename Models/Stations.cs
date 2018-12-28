using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
    public class Stations
    {
        public Int64 Id { get; set; }
        public String Name { get; set; }
        public String Code { get; set; }
        public String Prefix { get; set; }
        public DateTime Push { get; set; }
        public Boolean Updated { get; set; }

        public StationsBrand Brand { get; set; }

        public Double FuelLtrs { get; set; }
        public Double FuelSales { get; set; }
        public Double LubeSales { get; set; }



        public Stations()
        {
            Id = 0;
            Code = "";
            Name = "";
            Prefix = "";
            Updated = false;
            Push = new DateTime();
            FuelLtrs = 0;
            FuelSales = 0;
            LubeSales = 0;

            Brand = new StationsBrand();
        }
    }
}
