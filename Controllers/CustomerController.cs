using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Client.Models;
using Client.Services;
using Microsoft.AspNetCore.Authorization;

namespace Client.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        // GET: /<controller>/
        [Route("customers")]
        public IActionResult Index()
        {
            CustomerServices service = new CustomerServices(HttpContext);
            List<Customers> customers = new List<Customers>(service.GetCustomers());

            return View(customers);
        }

        [Route("customers/{idnt}")]
        public IActionResult Customers(Int64 idnt)
        {
            CustomerServices service = new CustomerServices(HttpContext);
            Customers customer = service.GetCustomer(idnt);

            return View(customer);
        }


        [AllowAnonymous]
        public JsonResult GetLedgerEntries(Int64 custid, string start, string stop, string filter)
        {
            StationsService service = new StationsService(HttpContext);
            if (string.IsNullOrWhiteSpace(filter))
                filter = "";

            List<LedgerEntries> entries = service.GetLedgerEntries(DateTime.Parse(start), DateTime.Parse(stop), filter, custid);
            return Json(entries);
        }
    }
}
