using System.Collections.Generic;
using System.Diagnostics;
using Client.ViewModel;
using Client.Models;
using Client.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System;
using System.Globalization;

namespace Client.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index(IndexViewModel model, String date = "")
        {
            StationsService service = new StationsService(HttpContext);
            if (!string.IsNullOrEmpty(date)) 
                model.Timestamp = DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture);

            model.Readings = new List<PumpReadings>(service.GetMetreReadings(model.Timestamp));
            model.Summaries = new List<TankSummary>(service.GetSummaries(model.Timestamp));
            model.Ledgers = new List<LegderSummary>(service.GetLedgerSummary(model.Timestamp));

            model.Totals = service.GetLedgerTotals(model.Timestamp);

            return View(model);
        }

        public IActionResult Placeholder()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public string GetCurrentUser(HttpContext context) {
            return context.User.FindFirst(ClaimTypes.UserData).Value;
        }
    }
}
