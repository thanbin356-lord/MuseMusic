using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MuseMusic.Controllers.AdminManagement.CustomerManagement
{
    [Route("[controller]")]
    public class Edit_Customer : Controller
    {
        private readonly ILogger<Edit_Customer> _logger;

        public Edit_Customer(ILogger<Edit_Customer> logger)
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