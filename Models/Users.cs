using System;
namespace Client.Models
{
    public class Users
    {
        public Int64 Id { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public Boolean Enabled { get; set; }
        public Boolean ToChange { get; set; }
        public Int64 AdminLevel { get; set; }
        public String AccessLevel { get; set; }
        public string Message { get; set; }
        public Stations Station { get; set; }

        public Users()
        {
            Id = 0;
            Name = "";
            Email = "";
            Username = "";
            Password = "";
            Enabled = true;
            ToChange = false;
            AdminLevel = 0;
            AccessLevel = "";
            Message = "";

            Station = new Stations();
        }
    }
}
