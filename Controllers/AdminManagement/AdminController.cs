using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MuseMusic.Models;
using MuseMusic.Models.Tables;
using Microsoft.AspNetCore.Authorization;
namespace MuseMusic.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public AdminController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    public IActionResult Login()
    {
        return View();
    }

    
    public IActionResult Categories()
    {
        return View();
    }
    public IActionResult Usermanage()
    {
        return View();
    }
    public IActionResult Ordermanage()
    {
        return View();
    }
    public IActionResult Blogmanage()
    {
        return View();
    }
    public IActionResult Phukienmanage()
    {
        return View();
    }
    public IActionResult Daudiamanage()
    {
        return View();
    }
    public IActionResult Profile()
    {
        return View();
    }

    [HttpGet("orderdetail")]
    public IActionResult OrderDetail()
    {
        return View("~/Views/Admin/Order/OrderDetail.cshtml");
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

    public IActionResult Orders()
    {
        using (var db = new shopmanagementContext())
        {
            var orderViewModel = new OrderViewModel();

            // Truy vấn danh sách đơn hàng
            var orders = (from o in db.Orders
                          join c in db.Customers on o.CustomerId equals c.Id
                          join a in db.Adminsellers on o.AdminsellerId equals a.Id
                          select new
                          {
                              Order = o,
                              Customer = c,
                              AdminSeller = a
                          }).ToList();

            orderViewModel.Orders = orders.Select(x => new Order
            {
                OrderId = x.Order.Id,
                CustomerName = x.Customer.Name,
                AdminName = db.Accounts.FirstOrDefault(acc => acc.Id == x.AdminSeller.AccountId)?.Username,
                CreatedAt = x.Order.CreatedAt?.ToString("yyyy-MM-dd") ?? "N/A",
                TotalPrice = x.Order.Total?.ToString("C") ?? "N/A",
                Status = x.Order.Status
            }).ToList();

            return View(orderViewModel);
        }
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    public partial class AccessoriesViewModel
    {
        public List<Product> Products { get; set; }
    }

    public class CustomerViewModel
    {
        public List<Customer> Customers { get; set; }
    }

    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int OrderCount { get; set; }
    }
    public class OrderViewModel
    {
        public List<Order> Orders { get; set; }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string AdminName { get; set; }
        public string CreatedAt { get; set; }
        public string TotalPrice { get; set; }
        public string Status { get; set; }
    }

}
