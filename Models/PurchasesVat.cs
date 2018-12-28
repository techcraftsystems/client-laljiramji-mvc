using System;
namespace Client.Models
{
    public class PurchasesVat
    {
        public Int64 Id { get; set; }
        public DateTime Date { get; set; }
        public Products Product { get; set; }

        public Double LandingCost { get; set; }
        public Double Pipeline { get; set; }
        public Double Roads { get; set; }
        public Double PipesLoss { get; set; }
        public Double DepotLoss { get; set; }
        public Double Delivery { get; set; }
        public Double StorageDist { get; set; }

        public Double MarginImport { get; set; }
        public Double MarginDealer { get; set; }
        public Double MarginSummary { get; set; }

        public Double ExciseDuty { get; set; }
        public Double MerchantLevy { get; set; }
        public Double DeclarationFee { get; set; }
        public Double RoadsMaintance { get; set; }
        public Double DevtPetroleum { get; set; }
        public Double ReguPetroleum { get; set; }
        public Double DevtRailway { get; set; }
        public Double ValueAddedTax { get; set; }
        public Double TaxesAndLevies { get; set; }

        public Double RetailPrice { get; set; }
        public Double TaxExempts { get; set; }
        public Double TaxableAmts { get; set; }


        public PurchasesVat()
        {
            Id = 0;
            Date = DateTime.Now;
            Product = new Products();


            LandingCost = 0;
            Pipeline = 0;
            Roads = 0;
            PipesLoss = 0;
            DepotLoss = 0;
            Delivery = 0;
            StorageDist = 0;

            MarginImport = 0;
            MarginDealer = 0;
            MarginSummary = 0;

            ExciseDuty = 0;
            MerchantLevy = 0;
            DeclarationFee = 0;
            RoadsMaintance = 0;
            DevtPetroleum = 0;
            ReguPetroleum = 0;
            DevtRailway = 0;
            ValueAddedTax = 0;
            TaxesAndLevies = 0;

            RetailPrice = 0;
            TaxExempts = 0;
            TaxableAmts = 0;
        }
    }
}
