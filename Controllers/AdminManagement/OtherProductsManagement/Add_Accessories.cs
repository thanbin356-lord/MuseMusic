using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MuseMusic.Controllers.AdminManagement.OtherProductsManagement
{
    [Route("[controller]")]
    public class Add_Accessories : Controller
    {
        private readonly ILogger<Add_Accessories> _logger;

        public Add_Accessories(ILogger<Add_Accessories> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}