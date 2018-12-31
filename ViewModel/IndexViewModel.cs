using System;
using System.Collections.Generic;
using Client.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.ViewModel
{
    public class IndexViewModel
    {
        public DateTime Timestamp { get; set; }
        public LedgerTotals Totals { get; set; }

        public List<PumpReadings> Readings { get; set; }
        public List<TankSummary> Summaries { get; set; }
        public List<LegderSummary> Ledgers { get; set; }

        public IndexViewModel()
        {
            Timestamp = DateTime.Now.AddDays(-1);
            Totals = new LedgerTotals();

            Readings = new List<PumpReadings>();
            Summaries = new List<TankSummary>();
            Ledgers = new List<LegderSummary>();
        }
    }
}
