using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Client.ViewModel;
using Client.Services;

namespace Client.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        // GET: /<controller>/
        [Route("reports")]
        public IActionResult Index(ReportsIndexViewModel model)
        {
            return View(model);
        }
    }
}
