using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Client.Models;
using Client.Extensions;

namespace Client.Services
{
    public class PurchasesService
    {
        public List<PurchasesVat> GetLatestPurchasesVat()
        {
            List<PurchasesVat> Entries = new List<PurchasesVat>();

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("DECLARE @date DATE = (SELECT MAX(pv_date) dt FROM PurchaseVATCalc); SELECT pv_idnt, pv_date, pv_product, pv_landing, pv_pipeline, pv_road, pv_pipeloss, pv_depotloss, pv_delivery, (pv_pipeline+ pv_road+pv_pipeloss+pv_depotloss+pv_delivery) B_STORAGE_DIST, pv_margin_import, pv_margin_dealer, (pv_margin_import+pv_margin_dealer) C_MARGINS, pv_excise_duty, pv_merchant_levy, pv_declaration_fee, pv_road_maintainance, pv_devt_petroleum, pv_regu_petroleum, pv_devt_railway, pv_vat, (pv_excise_duty+pv_merchant_levy+pv_declaration_fee+pv_road_maintainance+pv_devt_petroleum+pv_regu_petroleum+pv_devt_railway+pv_vat) D_TAX_LEVIES, pv_landing+(pv_pipeline+ pv_road+pv_pipeloss+pv_depotloss+pv_delivery)+(pv_margin_import+pv_margin_dealer)+(pv_excise_duty+pv_merchant_levy+pv_declaration_fee+pv_road_maintainance+pv_devt_petroleum+pv_regu_petroleum+pv_devt_railway+pv_vat) E_RETAIL, (pv_pipeloss+pv_depotloss+pv_merchant_levy+pv_declaration_fee+pv_road_maintainance+pv_devt_petroleum+pv_regu_petroleum+pv_devt_railway) F_EXEMPT FROM PurchaseVATCalc WHERE pv_date=@date ORDER BY pv_product");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    PurchasesVat Entry = new PurchasesVat();
                    Entry.Id = Convert.ToInt64(dr[0]);
                    Entry.Date = Convert.ToDateTime(dr[1]);
                    Entry.Product.Id = Convert.ToInt64(dr[2]);

                    Entry.LandingCost = Convert.ToDouble(dr[3]);
                    Entry.Pipeline = Convert.ToDouble(dr[4]);
                    Entry.Roads = Convert.ToDouble(dr[5]);
                    Entry.PipesLoss = Convert.ToDouble(dr[6]);
                    Entry.DepotLoss = Convert.ToDouble(dr[7]);
                    Entry.Delivery = Convert.ToDouble(dr[8]);
                    Entry.StorageDist = Convert.ToDouble(dr[9]);

                    Entry.MarginImport = Convert.ToDouble(dr[10]);
                    Entry.MarginDealer = Convert.ToDouble(dr[11]);
                    Entry.MarginSummary = Convert.ToDouble(dr[12]);

                    Entry.ExciseDuty = Convert.ToDouble(dr[13]);
                    Entry.MerchantLevy = Convert.ToDouble(dr[14]);
                    Entry.DeclarationFee = Convert.ToDouble(dr[15]);
                    Entry.RoadsMaintance = Convert.ToDouble(dr[16]);
                    Entry.DevtPetroleum = Convert.ToDouble(dr[17]);
                    Entry.ReguPetroleum = Convert.ToDouble(dr[18]);
                    Entry.DevtRailway = Convert.ToDouble(dr[19]);
                    Entry.ValueAddedTax = Convert.ToDouble(dr[20]);
                    Entry.TaxesAndLevies = Convert.ToDouble(dr[21]);
                    Entry.RetailPrice = Convert.ToDouble(dr[22]);
                    Entry.TaxExempts = Convert.ToDouble(dr[23]);
                    Entry.TaxableAmts = Entry.RetailPrice - Entry.TaxExempts;

                    Entries.Add(Entry);
                }
            }

            return Entries;
        }

        public Double GetFuelPurchasesLedgerOpenning(Int64 stid, DateTime date){
            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT ISNULL(SUM(total-pdo1-pdo2),0) FROM vPurchasesLedger WHERE st=" + stid + " AND dt < '" + date.Date + "'");
            if (dr.Read())
            {
                return Convert.ToDouble(dr[0]);
            }

            return 0;
        }
    }
}
