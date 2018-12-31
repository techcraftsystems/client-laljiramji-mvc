using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
    public class StationsBrand
    {
        public Int64 Id { get; set; }
        public String Name { get; set; }

        public StationsBrand()
        {
            Id = 0;
            Name = "";
        }
    }
}
