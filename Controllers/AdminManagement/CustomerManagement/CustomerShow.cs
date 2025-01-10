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
public class CustomerShow : Controller
{
    private readonly ILogger<CustomerShow> _logger;

    public CustomerShow(ILogger<CustomerShow> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("customers")]
    public IActionResult Customers()
    {
        using (var db = new shopmanagementContext())
        {
            var customerViewModel = new OrderViewModel();

            // Lấy danh sách khách hàng từ db.Customers
            var customers = db.Customers
                .Include(c => c.Account) // Bao gồm thông tin tài khoản
                .ToList();

            if (customers == null || !customers.Any())
            {
                return NotFound(); // Trả về 404 nếu không có khách hàng nào
            }

            customerViewModel.Orders = customers.Select(customer => new MuseMusic.Models.ManagerModels.Orders
            {
                CustomerId = customer.Id,
                CustomerName = customer.Name,
                Email = customer.Account?.Email, // Lấy email từ Account liên quan
                Phone = customer.Phone,
                Address = customer.Address,
                OrderCount = db.Orders.Count(o => o.CustomerId == customer.Id) // Đếm số lượng đơn hàng của khách hàng này
            }).ToList();

            return View("~/Views/Admin/CustomerManagement/Customers.cshtml", customerViewModel);
        }
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
