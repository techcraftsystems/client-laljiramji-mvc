using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
    public class StationsBrand
    {
        [Display(Name = "id")]
        [Required]
        public Int64 Id { get; set; }

        [Required]
        [Display(Name = "name")]
        public String Name { get; set; }

        public StationsBrand()
        {
            Id = 0;
            Name = "";
        }
    }
}
