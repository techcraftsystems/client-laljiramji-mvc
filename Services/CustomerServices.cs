using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Client.Models;
using Client.Extensions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Client.Services
{
    public class CustomerServices
    {
        public string dbo { get; set; }
        public int dba { get; set; }

        public CustomerServices(HttpContext httpContext)
        {
            dbo = httpContext.User.FindFirst(ClaimTypes.Dsa).Value;
            dba = int.Parse(httpContext.User.FindFirst(ClaimTypes.Dns).Value);
        }

        public Customers GetCustomer(Int64 idnt){
            Customers customer = new Customers();

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT Custid, Names, ISNULL(City,'Nairobi') Contacts, ISNULL(Mobile,'072xxxxxxx') Telephone, KRA_PIN, AccountBalance, CreditLimit, DateJoined, ISNULL(sr_last,'1990-01-01') sr_last_invs FROM " + dbo + "Customers LEFT OUTER JOIN " + dbo + "vLastInvoices ON Custid=sr_cust WHERE Custid=" + idnt);
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
            }

            return customer;
        }

        public List<Customers> GetCustomers(){
            List<Customers> customers = new List<Customers>();

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT Custid, Names, ISNULL(City,'Nairobi') Contacts, ISNULL(Mobile,'072xxxxxxx') Telephone, KRA_PIN, AccountBalance, CreditLimit, DateJoined, ISNULL(sr_last,'1990-01-01') sr_last_invs FROM " + dbo + "Customers LEFT OUTER JOIN " + dbo + "vLastInvoices ON Custid=sr_cust ORDER BY Names");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Customers customer = new Customers
                    {
                        Id = Convert.ToInt64(dr[0]),
                        Name = dr[1].ToString(),
                        Contacts = dr[2].ToString(),
                        Telephone = dr[3].ToString(),
                        KraPin = dr[4].ToString(),

                        Balance = Convert.ToDouble(dr[5]),
                        CreditLimit = Convert.ToDouble(dr[6]),

                        DateJoined = Convert.ToDateTime(dr[7]),
                        LastInvoice = Convert.ToDateTime(dr[8])
                    };

                    customers.Add(customer);
                }
            }

            return customers;
        }
    }
}
