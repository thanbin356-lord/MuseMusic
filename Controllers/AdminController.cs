using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MuseMusic.Models;
using MuseMusic.Models.Tables;
namespace MuseMusic.Controllers;

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
    public IActionResult Vinylmanage(){
        return View();
    }
    public IActionResult Usermanage(){
        return View();
    }
     public IActionResult Ordermanage(){
        return View();
    }
     public IActionResult Blogmanage(){
        return View();
    }
     public IActionResult Phukienmanage(){
        return View();
    }
     public IActionResult Daudiamanage(){
        return View();
    }
       public IActionResult Profile(){
        return View();
    }
     public IActionResult OrderDetail(){
        return View();
    }
        public IActionResult Addvinyl(){
        return View();
    }
      [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

    public IActionResult Vinyl()
    {
        {
            using (var db = new shopmanagementContext())
            {
                // phải xử lý trong cái using 
                // bước 1 tạo đối tượng 
                var vinylViewModel = new VinylViewModel();
                // bước 2 load dữ liệu từ db 
                var product = (from p in db.Products
                               join v in db.Vinyls on p.Id equals v.ProductId
                               select new
                               {
                                   Product = p,
                                   Vinyl = v
                               }).ToList();

                // bước 3 gắn lại dữ liệu vào đối tượng 
                vinylViewModel.Products = product.Select(x => new Product
                {
                    ProductId = x.Product.Id,
                    ProductName = x.Product.Name,
                    ProductDescription = x.Product.Description,
                    Price = x.Product.Price.ToString("C"),
                    DiskId = x.Vinyl.DiskId,
                    Years = (int)x.Vinyl.Years,
                    Tracklist = x.Vinyl.Tracklist
                }).ToList();

                // bước 4 return l 
                return View(vinylViewModel);
            }
        }
    }
    public IActionResult RecordPlayer()
    {
        {
            using (var db = new shopmanagementContext())
            {
                // phải xử lý trong cái using 
                // bước 1 tạo đối tượng 
                var recordPlayerViewModel = new RecordPlayerViewModel();
                // bước 2 load dữ liệu từ db 
                var product = (from p in db.Products
                               join r in db.Recordplayers on p.Id equals r.ProductId
                               join br in db.Brands on r.BrandId equals br.Id
                               select new
                               {
                                   Product = p,
                                   RecordPlayer = r,
                                   Brand = br
                               }).ToList();

                // bước 3 gắn lại dữ liệu vào đối tượng 
                recordPlayerViewModel.Products = product.Select(x => new Product
                {
                    ProductId = x.Product.Id,
                    ProductName = x.Product.Name,
                    ProductDescription = x.Product.Description,
                    Price = x.Product.Price.ToString("C"),
                    BrandName = x.Brand.Name,
                }).ToList();

                // bước 4 return l 
                return View(recordPlayerViewModel);
            }
        }
    }
    public IActionResult Accessories()
    {
        {
            using (var db = new shopmanagementContext())
            {
                // phải xử lý trong cái using 
                // bước 1 tạo đối tượng 
                var acessoriesViewModel = new AccessoriesViewModel();
                // bước 2 load dữ liệu từ db 
                var product = (from p in db.Products
                               join asory in db.Accessories on p.Id equals asory.ProductId
                               join br in db.Brands on asory.BrandId equals br.Id
                               select new
                               {
                                   Product = p,
                                   Accessory = asory,
                                   Brand = br
                               }).ToList();

                // bước 3 gắn lại dữ liệu vào đối tượng 
                acessoriesViewModel.Products = product.Select(x => new Product
                {
                    ProductId = x.Product.Id,
                    ProductName = x.Product.Name,
                    ProductDescription = x.Product.Description,
                    Price = x.Product.Price.ToString("C"),
                    BrandName = x.Brand.Name,
                }).ToList();

                // bước 4 return l 
                return View(acessoriesViewModel);
            }
        }
    }

    public IActionResult Customers()
    {
        {
            using (var db = new shopmanagementContext())
            {
                var customerViewModel = new CustomerViewModel();

                // Tách dữ liệu ra để tránh lỗi dịch ngữ cảnh
                var customers = db.Customers
                    .Select(c => new
                    {
                        Customer = c,
                        OrderCount = db.Orders.Count(o => o.CustomerId == c.Id)
                    })
                    .ToList();

                customerViewModel.Customers = customers.Select(x => new Customer
                {
                    CustomerId = x.Customer.Id,
                    CustomerName = x.Customer.Name,
                    Email = db.Accounts.FirstOrDefault(a => a.Id == x.Customer.AccountId)?.Email,
                    Phone = x.Customer.Phone,
                    Address = x.Customer.Address,
                    OrderCount = x.OrderCount
                }).ToList();

                return View(customerViewModel);
            }
        }
    }


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
    public partial class VinylViewModel
    {
        public List<Product> Products { get; set; }

    }
    public partial class RecordPlayerViewModel
    {
        public List<Product> Products { get; set; }
    }
    public partial class AccessoriesViewModel
    {
        public List<Product> Products { get; set; }
    }
    public class Product
    {
        public int ProductId { get; set; }
        public string DiskId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string Price { get; set; }
        public int Years { get; set; }
        public string Tracklist { get; set; }
        public string BrandName { get; set; }
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
