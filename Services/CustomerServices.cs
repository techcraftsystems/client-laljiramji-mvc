using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Client.Models;
using Client.Extensions;

namespace Client.Services
{
    public class CustomerServices
    {
        public Customers GetCustomer(String station, Int64 id){
            Customers customer = new Customers();

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT Custid, Names, Contacts, Telephone, KRA_PIN, AccountBalance, CreditLimit, DateJoined, ISNULL(sr_last,'1990-01-01')x, st_idnt, st_code, CASE st_idnt WHEN 12 THEN 'Shell Uhuru Highway' ELSE st_name END st_names FROM vCustomers INNER JOIN Stations ON Sts=st_idnt LEFT OUTER JOIN vLastInvoices ON sr_st=Sts AND sr_cust=Custid WHERE st_code='" + station + "' AND Custid=" + id);
            if (dr.Read()) {
                customer.Id = Convert.ToInt64(dr[0]);
                customer.Name = dr[1].ToString();
                customer.Contacts = dr[2].ToString();
                customer.Telephone = dr[3].ToString();
                customer.KraPin = dr[4].ToString();

                customer.Balance = Convert.ToDouble(dr[5]);
                customer.CreditLimit = Convert.ToDouble(dr[6]);

                customer.DateJoined = Convert.ToDateTime(dr[7]);
                customer.LastInvoice = Convert.ToDateTime(dr[8]);

                customer.Station.Id = Convert.ToInt64(dr[9]);
                customer.Station.Code = dr[10].ToString();
                customer.Station.Name = dr[11].ToString();
            }

            return customer;
        }

        public List<Customers> GetCustomers(String stations){
            List<Customers> customers = new List<Customers>();
            String additionalquery = "";

            if (!string.IsNullOrEmpty(stations.Trim()))
            {
                additionalquery = "WHERE st_name IN ('" + stations + "')";
            }

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT Custid, Names, Contacts, Telephone, KRA_PIN, AccountBalance, CreditLimit, DateJoined, ISNULL(sr_last,'1990-01-01') sr_last_invs, st_idnt, st_code, CASE st_idnt WHEN 12 THEN 'Shell Uhuru Highway' ELSE st_name END FROM vCustomers INNER JOIN Stations ON Sts=st_idnt LEFT OUTER JOIN vLastInvoices ON Sts=sr_st AND Custid=sr_cust " + additionalquery + " ORDER BY st_name, Names");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Customers customer = new Customers();
                    customer.Id = Convert.ToInt64(dr[0]);
                    customer.Name = dr[1].ToString();
                    customer.Contacts = dr[2].ToString();
                    customer.Telephone = dr[3].ToString();
                    customer.KraPin = dr[4].ToString();

                    customer.Balance = Convert.ToDouble(dr[5]);
                    customer.CreditLimit = Convert.ToDouble(dr[6]);

                    customer.DateJoined = Convert.ToDateTime(dr[7]);
                    customer.LastInvoice = Convert.ToDateTime(dr[8]);

                    customer.Station.Id = Convert.ToInt64(dr[9]);
                    customer.Station.Code = dr[10].ToString();
                    customer.Station.Name = dr[11].ToString();

                    customers.Add(customer);
                }
            }

            return customers;
        }
    }
}
