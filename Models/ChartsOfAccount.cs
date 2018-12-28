using System;
namespace Client.Models
{
    public class ChartsOfAccount
    {
        public Int64 Id { get; set; }
        public String Name { get; set; }
        public Int64 ParentId { get; set; }
        public String ParentName { get; set; }
        public String ViewName { get; set; }
        public String TabsName { get; set; }
        public String Notes { get; set; }
        public Stations Station { get; set; }

        public ChartsOfAccount()
        {
            Id = 0;
            ParentId = 0;

            Name = "";
            ParentName = "";
            ViewName = "";
            TabsName = "";
            Notes = "";

            Station = new Stations();
        }
    }
}
