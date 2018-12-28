using System;
using Client.Models;

namespace Client.ViewModel
{
    public class LoginModel
    {
        public Users User { get; set; }
        public String Message { get; set; }
        public String ReturnUrl { get; set; }

        public LoginModel()
        {
            User = new Users();
            Message = "";
            ReturnUrl = "";
        }
    }
}
