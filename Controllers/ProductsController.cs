using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Client.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [Route("core/fuel")]
        public IActionResult FuelList()
        {
            return View();
        }

        [Route("core/fuel/{id}")]
        public IActionResult FuelView(Int64 id) {
            return View();
        }
    }
}
