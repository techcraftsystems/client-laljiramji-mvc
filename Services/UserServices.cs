using System;
using System.Data.SqlClient;
using Client.Models;
using Client.Extensions;
          
namespace Client.Services
{
    public class UserServices
    {
        public Users GetUser(String username)
        {
            Users user = null;

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT usr_idnt, usr_name, usr_email, log_enabled, log_tochange, log_admin_lvl, log_access_lvl, log_password FROM Users INNER JOIN [Login] ON usr_idnt=log_user WHERE log_username='" + username +"'");
            if (dr.Read())
            {
                user = new Users();

                user.Id = Convert.ToInt64(dr[0]);
                user.Name = dr[1].ToString();
                user.Email = dr[2].ToString();
                user.Enabled = Convert.ToBoolean(dr[3]);
                user.ToChange = Convert.ToBoolean(dr[4]);

                user.AdminLevel = Convert.ToInt64(dr[5]);
                user.AccessLevel = dr[6].ToString();

                user.Username = username;
                user.Password = dr[7].ToString();
            }

            return user;
        }
    }
}
