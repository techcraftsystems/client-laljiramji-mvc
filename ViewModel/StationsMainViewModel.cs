using System;
using System.Collections.Generic;
using Client.Models;

namespace Client.ViewModel
{
    public class StationsMainViewModel
    {
        public List<Stations> Stations { get; set; }


        public StationsMainViewModel()
        {
            Stations = new List<Stations>();
        }
    }
}
