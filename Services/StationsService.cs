using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Client.Models;
using Client.Extensions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Client.Services
{
    public class StationsService
    {
        public string dbo { get; set; }
        public int dba { get; set; }

        public StationsService(HttpContext httpContext)
        {
            dbo = httpContext.User.FindFirst(ClaimTypes.Dsa).Value;
            dba = int.Parse(httpContext.User.FindFirst(ClaimTypes.Dns).Value);
        }

        public List<PumpReadings> GetMetreReadings(DateTime date) {
            List<PumpReadings> readings = new List<PumpReadings>();

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("DECLARE @date DATE='" + date + "'; SELECT ISNULL(pcol_idnt,0)x, pmp_idnt, pmp_name, ISNULL(pcol_price, pp_price) price, ISNULL(pcol_electronic_op,pp_read)op, ISNULL(pcol_electronic_adjust,0)adj, ISNULL(pcol_electronic_test,0)tst, ISNULL(pcol_electronic_cl,pp_read) cl FROM " + dbo + "Pumps LEFT OUTER JOIN " + dbo + "PumpsCollections ON pcol_pump=pmp_idnt AND pcol_date=@date LEFT OUTER JOIN (SELECT pmp, pcol_price pp_price, pcol_electronic_cl pp_read FROM (SELECT MAX(pcol_date) dts, pcol_pump pmp FROM " + dbo + "PumpsCollections WHERE pcol_date<@date GROUP BY pcol_pump) As Foo INNER JOIN " + dbo + "PumpsCollections ON pcol_date=dts AND pcol_pump=pmp) As Op ON pmp=pmp_idnt ORDER BY pmp_idnt");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    PumpReadings reading = new PumpReadings
                    {
                        Id = Convert.ToInt64(dr[0])
                    };
                    reading.Pump.Id = Convert.ToInt64(dr[1]);
                    reading.Pump.Name = dr[2].ToString();

                    reading.Price = Convert.ToDouble(dr[3]);
                    reading.Opening = Convert.ToDouble(dr[4]);
                    reading.Adjustment = Convert.ToDouble(dr[5]);
                    reading.Tests = Convert.ToDouble(dr[6]);
                    reading.Closing = Convert.ToDouble(dr[7]);

                    readings.Add(reading);
                }
            }

            return readings;
        }

        public List<TankSummary> GetSummaries(DateTime date){
            List<TankSummary> summaries = new List<TankSummary>();

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("DECLARE @date1 DATE='" + new DateTime(date.Year, date.Month, 1) + "', @date2 DATE='" + date + "'; SELECT dp_tank, tnk_name, tnk_capacity, ISNULL(tdd.td_fuel, tnk_fuel) dp_fuel, ISNULL(rtn,0) RTNS, ISNULL(sal,0) SALES, ISNULL(fr_quantity,0) DELVS, ISNULL(tdd.td_reading,0) As DIPS, MIN(tda.td_reading)-ISNULL(tnkDF,0) As [OPEN] FROM (SELECT MAX(td_date) dp_date, td_tank dp_tank FROM " + dbo + "TanksDips WHERE CAST(td_date AS DATE)<@date1 GROUP BY td_tank ) As Foo INNER JOIN " + dbo + "Tanks ON dp_tank=tnk_idnt INNER JOIN " + dbo + "TanksDips tda ON dp_date=tda.td_date AND dp_tank=tda.td_tank LEFT OUTER JOIN " + dbo + "TanksDips tdd ON dp_tank=tdd.td_tank AND CAST(tdd.td_date AS DATE)=@date2 LEFT OUTER JOIN " + dbo + "FuelReceipts ON fr_tank=dp_tank AND fr_date=@date2 LEFT OUTER JOIN (SELECT tnk1D, SUM(tnkDF) tnkDF FROM (SELECT pcol_tank tnk1D, pcol_electronic_cl-pcol_electronic_test-pcol_electronic_adjust-pcol_electronic_op tnkDF FROM " + dbo + "PumpsCollections WHERE pcol_date>=@date1 AND pcol_date<@date2 UNION ALL SELECT fr_tank, 0-fr_quantity FROM " + dbo + "FuelReceipts WHERE fr_date>=@date1 AND fr_date<@date2) As Diffs GROUP BY tnk1D) As kD1 ON tnk1D=dp_tank LEFT OUTER JOIN (SELECT pcol_tank tnk, SUM(pcol_electronic_cl-pcol_electronic_op) sal, SUM(pcol_electronic_test) rtn FROM " + dbo + "PumpsCollections WHERE pcol_date=@date2 GROUP BY pcol_tank) As Sl ON tnk=dp_tank GROUP BY dp_tank, tdd.td_fuel, tnk_fuel, tdd.td_idnt, tnk_name, tdd.td_reading, fr_quantity, tnkDF, tnk_capacity, rtn, sal ORDER BY dp_tank");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    TankSummary summary = new TankSummary();
                    summary.Tank.Id = Convert.ToInt64(dr[0]);
                    summary.Tank.Name = dr[1].ToString();
                    summary.Tank.Capacity = Convert.ToDouble(dr[2]);
                    summary.Fuel.Id = Convert.ToInt64(dr[3]);

                    summary.Returns = Convert.ToDouble(dr[4]);
                    summary.Sales = Convert.ToDouble(dr[5]);
                    summary.Delivery = Convert.ToDouble(dr[6]);
                    summary.Dips = Convert.ToDouble(dr[7]);
                    summary.Opening = Convert.ToDouble(dr[8]);
                    summary.Closing = summary.Opening + summary.Delivery + summary.Returns - summary.Sales;
                    summary.Variance = summary.Closing - summary.Dips;

                    summaries.Add(summary);
                }
            }

            return summaries;
        }

        public List<LegderSummary> GetLedgerSummary(DateTime date){
            List<LegderSummary> summaries = new List<LegderSummary>();

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("DECLARE @date DATE='" + date + "' SELECT mm_account, mm_acname, mm_type, mm_action, SUM(mm_amount) mm_amount, SUM(mm_lube) mm_lube, SUM(mm_disc) mm_disc FROM (SELECT mm_date, 0 mm_type, mm_action, mm_account, CASE WHEN mm_action<>4 THEN BankName+(CASE WHEN BankName IN ('EQUITY','CFC') THEN ' VISA' ELSE '' END) ELSE 'LIPA NA MPESA' END mm_acname, mm_amount, 0 mm_lube, 0 mm_disc FROM " + dbo + "MoneyTransactions LEFT OUTER JOIN " + dbo + "AccountsBank ON BankID=mm_account AND mm_action<>4 WHERE mm_source=15 AND mm_action<>0 UNION ALL SELECT sr_date, 1,0,sr_cust, Names, sr_amts, sr_lube, sr_discount FROM " + dbo + "Receipts INNER JOIN " + dbo + "Customers ON Custid=sr_cust) AS Foo WHERE mm_date=@date GROUP BY mm_acname, mm_type, mm_action, mm_account");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    LegderSummary summary = new LegderSummary();
                    summary.Customer.Id = Convert.ToInt64(dr[0]);
                    summary.Customer.Name = dr[1].ToString();
                    summary.Type = Convert.ToInt64(dr[2]);
                    summary.Action = Convert.ToInt64(dr[3]);
                    summary.FuelSales = Convert.ToDouble(dr[4]);
                    summary.LubeSales = Convert.ToDouble(dr[5]);
                    summary.Discounts = Convert.ToDouble(dr[6]);

                    summaries.Add(summary);
                }
            }

            return summaries;
        }

        public LedgerTotals GetLedgerTotals(DateTime date){
            LedgerTotals total = new LedgerTotals();

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("DECLARE @date DATE='" + date.Date + "', @st INT=" + dba +"; SELECT ISNULL(ar_cash,0)cs, ISNULL(mm_visa,0)vc, ISNULL(mm_mpesa,0)mp, ISNULL(mm_pos,0)ps, ISNULL(ex_amts,0)xp, ISNULL(ar_cwash,0)cw, ISNULL(ar_tyre,0)tc, ISNULL(ar_service,0)sv FROM Stations LEFT OUTER JOIN vLedgerAccounts ON st_idnt=mm_st AND mm_date=@date LEFT OUTER JOIN (SELECT ar_st, ar_cash, ar_cwash, ar_tyre, ar_service FROM vLedgerCash WHERE ar_date=@date) As Cash ON ar_st=st_idnt LEFT OUTER JOIN (SELECT ex_st, ex_amts FROM vLedgerExpenses WHERE ex_date=@date) As exps ON ex_st=st_idnt WHERE st_idnt=@st");
            if (dr.Read())
            {
                total.Station.Id = Convert.ToInt64(dba);
                total.Date = date;
                total.Cash = Convert.ToDouble(dr[0]);
                total.Visa = Convert.ToDouble(dr[1]);
                total.Mpesa = Convert.ToDouble(dr[2]);
                total.POS = Convert.ToDouble(dr[3]);
                total.Expense = Convert.ToDouble(dr[4]);

                total.Account = total.Visa + total.Mpesa + total.POS;
                total.Summary = total.Account + total.Cash + total.Expense;

                total.CarWash = Convert.ToDouble(dr[5]);
                total.TyreCtr = Convert.ToDouble(dr[6]);
                total.Service = Convert.ToDouble(dr[7]);
            }

            return total;
        }

        public List<LedgerTotals> GetLedgerTotals(String stations, DateTime date1, DateTime date2){
            List<LedgerTotals> totals = new List<LedgerTotals>();
            String additionalquery = "";

            if (!string.IsNullOrEmpty(stations.Trim()))
            {
                additionalquery = "WHERE st_name IN (" + stations + ")";
            }

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("DECLARE @start DATE='" + date1.Date + "', @stop DATE='" + date2.Date + "'; SELECT st_idnt, st_code, st_name, ISNULL(SUM(SALE),0)SALE, ISNULL(SUM(CASH),0)CASH, ISNULL(SUM(INVS),0)INVS, ISNULL(SUM(VISA),0)VISA, ISNULL(SUM(MPESA),0)MPESA, ISNULL(SUM(POS),0)POS, ISNULL(SUM(EXPS),0)EXPS, ISNULL(SUM(DISC),0)DISC FROM Stations LEFT OUTER JOIN (SELECT pcol_station ST, pcol_amts SALE, 0 CASH, 0 INVS, 0 VISA, 0 MPESA, 0 POS, 0 EXPS, 0 DISC FROM vDailySales WHERE pcol_date BETWEEN @start AND @stop UNION ALL SELECT ar_st, 0, ar_cash, 0,0,0,0,0,0 FROM vLedgerCash WHERE ar_date BETWEEN @start AND @stop UNION ALL SELECT sr_st, 0,0, sr_amts, 0,0,0,0, ABS(sr_disc) FROM vLedgerInvoices WHERE sr_date BETWEEN @start AND @stop UNION ALL SELECT mm_st, 0,0,0, mm_visa, mm_mpesa, mm_pos,0,0 FROM vLedgerAccounts WHERE mm_date BETWEEN @start AND @stop UNION ALL SELECT ex_st, 0,0,0, 0,0,0,ex_amts,0 FROM vLedgerExpenses WHERE ex_date BETWEEN @start AND @stop) As Ledger ON ST=st_idnt " + additionalquery + " GROUP BY st_idnt, st_code, st_order, st_name ORDER BY st_order");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    LedgerTotals total = new LedgerTotals();
                    total.Station.Id = Convert.ToInt64(dr[0]);
                    total.Station.Code = dr[1].ToString().ToLower();
                    total.Station.Name = dr[2].ToString().ToUpper();

                    total.Sale = Convert.ToDouble(dr[3]);
                    total.Cash = Convert.ToDouble(dr[4]);
                    total.Invoice = Convert.ToDouble(dr[5]);
                    total.Visa = Convert.ToDouble(dr[6]);
                    total.Mpesa = Convert.ToDouble(dr[7]);
                    total.POS = Convert.ToDouble(dr[8]);
                    total.Expense = Convert.ToDouble(dr[9]);
                    total.Discount = Convert.ToDouble(dr[10]);

                    total.Account = total.Visa + total.Mpesa + total.POS;
                    total.Summary = total.Cash + total.Invoice + total.Discount + total.Expense + total.Account;

                    totals.Add(total);
                }
            }

            return totals;
        }

        public List<StationsReconcile> GetStationsReconciles(Int64 stid, Int64 year, Int64 mnth){
            List<StationsReconcile> reconciles = new List<StationsReconcile>();

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("DECLARE @st INT=" + stid + ", @year INT=" + year + ", @mnth INT=" + mnth + "; SELECT Dates, SUM(Amts)Amts, SUM(Pd)Pd, SUM(Uprna)Uprna, SUM(Debt)Debt, SUM(Disc)Disc, SUM(Transp)Transp FROM ( SELECT pcol_date Dates, pcol_amts Amts, 0 Pd, 0 Uprna, 0 Debt, 0 Disc, 0 Transp FROM vDailySales WHERE pcol_station=@st AND YEAR(pcol_date)=@year AND MONTH(pcol_date)=@mnth UNION ALL SELECT ar_date, 0, ar_cash,0,0,0,0 FROM vLedgerCash WHERE ar_st=@st AND YEAR(ar_date)=@year AND MONTH(ar_date)=@mnth UNION ALL SELECT sr_date, CASE sr_overpump when 1 THEN sr_amts-(sr_discount) ELSE 0 END, 0,0,sr_amts, sr_discount,0 FROM vInvoicesLedger WHERE sr_st=@st AND YEAR(sr_date)=@year AND MONTH(sr_date)=@mnth UNION ALL SELECT am_date, 0,0,0,am_amts,0,0 FROM vAccounts WHERE am_st=@st AND YEAR(am_date)=@year AND MONTH(am_date)=@mnth UNION ALL SELECT ex_date, 0,ex_amts,0,0,0,0 FROM vLedgerExpenses WHERE ex_st=@st AND YEAR(ex_date)=@year AND MONTH(ex_date)=@mnth UNION ALL SELECT pt_date,0,0,0,0,0, pt_amount FROM vTransport WHERE pt_st=@st AND YEAR(pt_date)=@year AND MONTH(pt_date)=@mnth) As Foo GROUP BY Dates ORDER BY Dates");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    StationsReconcile recon = new StationsReconcile();
                    recon.Date = Convert.ToDateTime(dr[0]).ToString("dd/MM/yyyy");
                    recon.Amount = Convert.ToDouble(dr[1]);
                    recon.Payment = Convert.ToDouble(dr[2]);
                    recon.Uprna = Convert.ToDouble(dr[3]);
                    recon.Debt = Convert.ToDouble(dr[4]);
                    recon.Discount = Convert.ToDouble(dr[5]);
                    recon.Discount = Convert.ToDouble(dr[5]);
                    recon.Transport = Convert.ToDouble(dr[6]);
                    recon.Balance = recon.Amount - recon.Payment - recon.Uprna - recon.Debt - recon.Transport + recon.Discount;

                    reconciles.Add(recon);
                }
            }

            return reconciles;
        }

        public List<LedgerEntries> GetLedgerEntries(DateTime start, DateTime stop, String filter, Int64 custid = 0){
            List<LedgerEntries> entries = new List<LedgerEntries>();
            String AdditionalString = "";

            if (custid != 0){
                AdditionalString = " AND mm_account=" + custid;
            }

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT mm_idnt, mm_action, mm_account, mm_date, mm_desc, am_lpos, am_invs, mm_name, mm_price, mm_amount FROM vLedgerzEntry " + conn.GetQueryString(filter, "mm_desc+'-'+am_lpos+'-'+am_invs+'-'+mm_name+'-'+CAST(mm_amount AS NVARCHAR)", "mm_st=" + dba + " AND mm_date BETWEEN '" + start.Date + "' AND '" + stop.Date + "'") + AdditionalString + " ORDER BY mm_date, am_invs, am_lpos, mm_desc");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    LedgerEntries entry = new LedgerEntries();
                    entry.Station.Id = dba;
                    entry.Id = Convert.ToInt64(dr[0]);
                    entry.Action = Convert.ToInt64(dr[1]);
                    entry.Account = Convert.ToInt64(dr[2]);
                    entry.Date = Convert.ToDateTime(dr[3]).ToString("dd/MM/yyyy");

                    entry.Description = dr[4].ToString();
                    entry.Lpo = dr[5].ToString();
                    entry.Invoice = dr[6].ToString();
                    entry.Name = dr[7].ToString();

                    entry.Price = Convert.ToDouble(dr[8]);
                    entry.Amount = Convert.ToDouble(dr[9]);
                    entry.Quantity = (entry.Amount / entry.Price);

                    entries.Add(entry);
                }
            }

            return entries;
        }

        public List<LedgerEntries> GetLedgerDuplicates(DateTime start, DateTime stop){
            List<LedgerEntries> entries = new List<LedgerEntries>();

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("DECLARE @date1 DATE='" + start.Date + "', @date2 DATE='" + stop.Date + "'; SELECT TOP(200) sr_idnt, sr_date, CASE sr_fuel WHEN 1 THEN 'LTS DIESEL' WHEN 2 THEN 'LTS SUPER' WHEN 3 THEN 'LTS VPOWER' WHEN 4 THEN 'LTS KEROSENE' ELSE 'OTHERS' END xdesc, sr_lpo, sr_invoice, sr_price, sr_amts, sr_cust, Names, sr_st, st_code, st_name FROM ( SELECT sr_idnt, sr_st, sr_cust, sr_date, sr_fuel, sr_overpump, sr_lpo, sr_invoice, sr_price, sr_amts, sr_discount FROM vInvoicesLedger WHERE sr_invoice IN (SELECT DISTINCT invs FROM vInvoicesDuplicates INNER JOIN vInvoicesLedger ON invs=sr_invoice WHERE sr_date BETWEEN @date1 AND @date2) UNION ALL SELECT sr_idnt, sr_st, sr_cust, sr_date, sr_fuel, sr_overpump, sr_lpo, sr_invoice, sr_price, sr_amts, sr_discount FROM vInvoicesLedger WHERE sr_date BETWEEN @date1 AND @date2 AND sr_invoice=0 ) As Foo INNER JOIN Stations ON st_idnt=sr_st INNER JOIN vCustomers ON sr_cust=Custid AND Sts=st_idnt ORDER BY sr_invoice, sr_date, sr_fuel");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    LedgerEntries entry = new LedgerEntries();
                    entry.Id = Convert.ToInt64(dr[0]);
                    entry.Date = Convert.ToDateTime(dr[1]).ToString("dd/MM/yyyy");

                    entry.Description = dr[2].ToString();
                    entry.Lpo = dr[3].ToString();
                    entry.Invoice = dr[4].ToString();

                    entry.Price = Convert.ToDouble(dr[5]);
                    entry.Amount = Convert.ToDouble(dr[6]);
                    entry.Quantity = (entry.Amount / entry.Price);

                    entry.Customer.Id = Convert.ToInt64(dr[7]);
                    entry.Customer.Name = dr[8].ToString();

                    entry.Name = entry.Customer.Name;

                    entry.Station.Id = Convert.ToInt64(dr[9]);
                    entry.Station.Code = dr[10].ToString();
                    entry.Station.Name = dr[11].ToString();

                    entries.Add(entry);
                }
            }

            return entries;
        }

        public List<LedgerEntries> GetLedgerEntriesDuplicates(DateTime start, DateTime stop){
            List<LedgerEntries> entries = new List<LedgerEntries>();

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("DECLARE @date1 DATE='" + start.Date + "', @date2 DATE='" + stop.Date + "'; SELECT sr_idnt, sr_date, CASE sr_fuel WHEN 1 THEN 'LTS DIESEL' WHEN 2 THEN 'LTS SUPER' WHEN 3 THEN 'LTS VPOWER' WHEN 4 THEN 'LTS KEROSENE' ELSE 'OTHERS' END xdesc, sr_lpo, sr_invoice, sr_price, sr_amts, sr_cust, Names, sr_st, st_code, st_name FROM ( SELECT sr_idnt, sr_st, sr_cust, sr_date, sr_fuel, sr_overpump, sr_lpo, sr_invoice, sr_price, sr_amts, sr_discount FROM vInvoicesLedger WHERE sr_invoice IN ( SELECT DISTINCT invs FROM vInvoicesDuplicates INNER JOIN vInvoicesLedger ON invs=sr_invoice WHERE sr_date BETWEEN @date1 AND @date2) UNION ALL SELECT sr_idnt, sr_st, sr_cust, sr_date, sr_fuel, sr_overpump, sr_lpo, sr_invoice, sr_price, sr_amts, sr_discount FROM vInvoicesLedger WHERE sr_date BETWEEN @date1 AND @date2 AND sr_invoice=0 ) As Foo INNER JOIN Stations ON st_idnt=sr_st INNER JOIN vCustomers ON sr_cust=Custid ORDER BY sr_invoice, sr_date, sr_fuel");
            if (dr.HasRows) {
                while (dr.Read()){
                    LedgerEntries entry = new LedgerEntries
                    {
                        Id = Convert.ToInt64(dr[0]),
                        Date = Convert.ToDateTime(dr[1]).ToString("dd/MM/yyyy"),
                        Description = dr[2].ToString(),
                        Lpo = dr[3].ToString(),
                        Invoice = dr[4].ToString(),
                        Price = Convert.ToDouble(dr[5]),
                        Amount = Convert.ToDouble(dr[6])
                    };

                    entry.Quantity = (entry.Amount / entry.Price);

                    entry.Customer.Id = Convert.ToInt64(dr[7]);
                    entry.Customer.Name = dr[8].ToString();

                    entry.Station.Id = Convert.ToInt64(dr[9]);
                    entry.Station.Code = dr[10].ToString();
                    entry.Station.Name = dr[11].ToString();

                    entries.Add(entry);
                }
            }

            return entries;
        }

        public List<Expenses> GetExpenditure(Int64 stid, DateTime start, DateTime stop, String filter){
            List<Expenses> expenses = new List<Expenses>();

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT ex_ids, ex_date, ex_loc, ex_desc, ex_amount, ex_tsid, SETT_NAME FROM vExpenses INNER JOIN vChartOfAccounts ON ex_st=ST_IDNT AND ex_tsid=LINE_IDNT " + conn.GetQueryString(filter, "ex_loc+'-'+ex_desc+'-'+FILT_NAME+'-'+CAST(ex_amount AS NVARCHAR)", "ex_st=" + stid + " AND ex_date BETWEEN '" + start.Date + "' AND '" + stop.Date + "'") + " ORDER BY ex_date, ex_ids");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Expenses expense = new Expenses();
                    expense.Station.Id = stid;

                    expense.Id = Convert.ToInt64(dr[0]);
                    expense.Date = Convert.ToDateTime(dr[1]).ToString("dd/MM/yyyy");
                    expense.Location = dr[2].ToString();
                    expense.Description = dr[3].ToString();
                    expense.Amount = Convert.ToDouble(dr[4]);

                    expense.Account.Id = Convert.ToInt64(dr[5]);
                    expense.Account.Name = dr[6].ToString();

                    expenses.Add(expense);
                }
            }

            return expenses;
        }

        public List<PurchasesLedger> GetPurchasesLedger(Int64 stid, DateTime start, DateTime stop, String filter)
        { 
            List<PurchasesLedger> ledgers = new List<PurchasesLedger>();

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("DECLARE @st INT=" + stid + ", @dt1 DATE='" + start.Date + "', @dt2 DATE='" + stop.Date + "'; SELECT id, fl_idnt, fl_name, Supp, ISNULL(Names,'N/A') Supp, lx, date_, SuppInv, qty, price FROM (SELECT 'PURC' lx, id, Supp, item_id, date_, SuppInv, qty, price FROM vFuelPurchasesLedger WHERE pr_st=@st AND date_ BETWEEN @dt1 AND @dt2 UNION ALL SELECT 'DELV' lx, fr_idnt, 0, fr_fuel, fr_date, 'N/A', fr_quantity, 0 FROM vFuelDeliveryLedger WHERE fr_st=@st AND fr_date BETWEEN @dt1 AND @dt2) As Foo INNER JOIN Fuel On fl_idnt=item_id LEFT OUTER JOIN vSuppliers ON Supp=Suppid AND Stn=@st " + conn.GetQueryString(filter, "fl_name+'-'+ISNULL(Names,'N/A')+'-'+lx+'-'+SuppInv+'-'+CAST(qty AS NVARCHAR)+'-'+CAST(qty*price AS NVARCHAR)") + " ORDER BY date_, lx DESC, item_id");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    PurchasesLedger item = new PurchasesLedger();
                    item.Station.Id = stid;

                    item.Id = Convert.ToInt64(dr[0]);
                    item.Fuel.Id = Convert.ToInt64(dr[1]);
                    item.Fuel.Name = dr[2].ToString();
                    item.Supplier.Id = Convert.ToInt64(dr[3]);
                    item.Supplier.Name = dr[4].ToString();
                    item.Type = dr[5].ToString();
                    item.Date = Convert.ToDateTime(dr[6]).ToString("dd/MM/yyyy");
                    item.Invoice = dr[7].ToString();
                    item.Quantity = Convert.ToDouble(dr[8]);
                    item.Price = Convert.ToDouble(dr[9]);
                    item.Total = item.Quantity * item.Price;

                    if (item.Type.Equals("DELV")){
                        item.Delivery = item.Quantity;
                    }
                    else {
                        item.Purchase = item.Quantity;
                    }

                    ledgers.Add(item);
                }
            }

            return ledgers;
        }
    }
}
