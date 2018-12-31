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
            SqlDataReader dr = conn.SqlServerConnect("SELECT usr_idnt, usr_name, usr_email, log_enabled, log_tochange, log_admin_lvl, log_access_lvl, log_password, st_idnt, CASE WHEN st_idnt=12 THEN 'Shell Uhuru Highway' ELSE st_name END, st_database FROM Users INNER JOIN [Login] ON usr_idnt=log_user INNER JOIN Stations ON log_station=st_idnt WHERE log_username='" + username +"'");
            if (dr.Read())
            {
                user = new Users
                {
                    Id = Convert.ToInt64(dr[0]),
                    Name = dr[1].ToString(),
                    Email = dr[2].ToString(),
                    Enabled = Convert.ToBoolean(dr[3]),
                    ToChange = Convert.ToBoolean(dr[4]),

                    AdminLevel = Convert.ToInt64(dr[5]),
                    AccessLevel = dr[6].ToString(),

                    Username = username,
                    Password = dr[7].ToString()
                };

                user.Station.Id = Convert.ToInt64(dr[8]);
                user.Station.Name = dr[9].ToString();
                user.Station.Prefix = dr[10].ToString();
            }

            return user;
        }
    }
}
