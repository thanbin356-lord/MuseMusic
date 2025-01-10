using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MuseMusic.Models.ManagerModels;
using MuseMusic.Models.Tables;

namespace MuseMusic.Controllers.AdminManagement.CustomerManagement;
[Authorize(Roles = "Admin")]
[Route("admin")]
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
    [HttpGet("editcustomers/{id}")]
    public IActionResult EditCustomers(int id)
    {
        using (var db = new shopmanagementContext())
        {

            var customers = db.Orders
            .Include(o => o.Customer)
                .ThenInclude(ac => ac.Account)

            .FirstOrDefault(o => o.CustomerId == id);
            if (customers == null)
            {
                return NotFound(); // Return 404 if no customers is found
            }
            // var images = db.ImageUrls
            //             .Where(img => img.CustomerId == customers.Customer.Id)
            //             .ToList()
            var orderCount = db.Orders
                       .Where(o => o.CustomerId == id)
                       .Count();
            var viewModel = new OrderViewModel
            {
                SelectedCustomerOrders = new Models.ManagerModels.Orders
                {
                    CustomerId = customers.Customer.Id,
                    CustomerName = customers.Customer.Name,
                    Phone = customers.Customer.Phone,
                    Email = customers.Customer.Account.Email,
                    Address = customers.Customer.Address,
                    OrderCount = orderCount,
                    OrderId = customers.Id,
                    created_at = (DateTime)customers.CreatedAt,
                    total = (decimal)customers.Total,
                },
                // AllBrands = db.Brands
                //     .Select(b => new MuseMusic.Models.ManagerModels.Brand { Id = b.Id, Name = b.Name })
                //     .ToList(),
                // SelectedBrandId = customers.BrandId ?? 0,
            };

            return View("~/Views/Admin/CustomerManagement/editcustomers.cshtml", viewModel);
        }
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
