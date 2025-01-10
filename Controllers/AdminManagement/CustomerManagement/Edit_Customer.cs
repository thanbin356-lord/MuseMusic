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
            // Try to fetch customer orders with related customer and account
            var orderWithCustomer = db.Orders
                .Include(o => o.Customer)
                    .ThenInclude(c => c.Account)
                .FirstOrDefault(o => o.CustomerId == id);

            if (orderWithCustomer == null)
            {
                // If no order exists, attempt to fetch customer directly
                var customerOnly = db.Customers
                    .Include(c => c.Account)
                    .FirstOrDefault(c => c.Id == id);

                if (customerOnly == null)
                {
                    return NotFound(); // Return 404 if no customer is found
                }

                // Create view model with only customer details
                var customerViewModel = new OrderViewModel
                {
                    SelectedCustomerOrders = new Models.ManagerModels.Orders
                    {
                        CustomerId = customerOnly.Id,
                        CustomerName = customerOnly.Name,
                        Phone = customerOnly.Phone,
                        Email = customerOnly.Account?.Email,
                        Address = customerOnly.Address,
                        OrderCount = 0, // No orders found
                        OrderId = 0,
                        created_at = DateTime.MinValue,
                        total = 0
                    },
                    AllOrders = new List<Models.ManagerModels.Orders>() // Empty orders list
                };

                return View("~/Views/Admin/CustomerManagement/editcustomers.cshtml", customerViewModel);
            }

            // Fetch all orders for the customer
            var allOrders = db.Orders
                .Where(o => o.CustomerId == id)
                .Select(o => new Models.ManagerModels.Orders
                {
                    OrderId = o.Id,
                    created_at = o.CreatedAt ?? DateTime.MinValue,
                    total = o.Total ?? 0
                })
                .ToList();

            // Count orders for the customer
            var orderCount = allOrders.Count;

            // Populate the view model
            var viewModel = new OrderViewModel
            {
                SelectedCustomerOrders = new Models.ManagerModels.Orders
                {
                    CustomerId = orderWithCustomer.Customer.Id,
                    CustomerName = orderWithCustomer.Customer.Name,
                    Phone = orderWithCustomer.Customer.Phone,
                    Email = orderWithCustomer.Customer.Account?.Email,
                    Address = orderWithCustomer.Customer.Address,
                    OrderCount = orderCount,
                    OrderId = orderWithCustomer.Id,
                    created_at = orderWithCustomer.CreatedAt ?? DateTime.MinValue,
                    total = orderWithCustomer.Total ?? 0
                },
                AllOrders = allOrders
            };

            return View("~/Views/Admin/CustomerManagement/editcustomers.cshtml", viewModel);
        }
    }
}