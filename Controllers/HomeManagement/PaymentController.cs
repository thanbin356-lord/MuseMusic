using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MuseMusic.Models.ViewModels;
using MuseMusic.Models.Tables;
using MuseMusic.Models;
using MuseMusic.Helpers;
using System.Data.Common;

namespace MuseMusic.Controllers.HomeManagement;

[Route("home")]
[Authorize]
public class PaymentController : Controller
{
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(ILogger<PaymentController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("Payment")]
    public IActionResult Payment()
    {
        return View("~/Views/Home/Payment.cshtml");
    }

    [HttpPost("Payment")]
    public IActionResult PPayment([FromBody] OrderForm request)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        string vnpayRef = new Random().Next(100000, 999999).ToString();
        using (var db = new shopmanagementContext())
        {
            var customer = db.Customers.FirstOrDefault(c => c.AccountId == Convert.ToInt32(userId));
            // Create a new order with the parsed total payment
            var newOrder = new MuseMusic.Models.Tables.Order
            {
                CustomerId = customer.Id,
                AdminsellerId = 1,
                CreatedAt = DateTime.Now,
                Status = "Pending",
                PaymentId = 1,
                Total = request.TotalPayment,
                VnpayRef = vnpayRef,
            };
            db.Add(newOrder);
            db.SaveChanges();
            foreach (var orderDetail in request.Cart)
            {
                var newOrderDetail = new MuseMusic.Models.Tables.OrderDetail
                {
                    ProductId = orderDetail.Id,
                    Quantity = orderDetail.Quantity,
                    OrderId = newOrder.Id,
                };
                db.Add(newOrderDetail);
            }

            db.SaveChanges();
        }

        VnPayLibrary vnpay = new VnPayLibrary();

        vnpay.AddRequestData("vnp_Version", "2.1.0");
        vnpay.AddRequestData("vnp_Command", "pay");
        vnpay.AddRequestData("vnp_TmnCode", "JAUROCZK");
        vnpay.AddRequestData("vnp_Amount", (request.TotalPayment * 100).ToString());
        vnpay.AddRequestData("vnp_BankCode", "VNBANK");
        vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
        vnpay.AddRequestData("vnp_CurrCode", "VND");
        vnpay.AddRequestData("vnp_IpAddr", HttpContext.Connection.RemoteIpAddress?.ToString());
        vnpay.AddRequestData("vnp_Locale", "vn");
        vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + vnpayRef);
        vnpay.AddRequestData("vnp_OrderType", "other");
        vnpay.AddRequestData("vnp_ReturnUrl", "http://localhost:5089/home/ReturnUrl");
        vnpay.AddRequestData("vnp_TxnRef", vnpayRef);

        string paymentUrl = vnpay.CreateRequestUrl("https://sandbox.vnpayment.vn/paymentv2/vpcpay.html", "XIKKDZX1ASFO17G2IK7JB2PW05BV2K1E");
        var response = new APIResponse
        {
            Success = true,
            Message = paymentUrl
        };

        return StatusCode(200, response);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }

    [HttpGet("ReturnUrl")]
    public IActionResult Verify()
    {
        var transactionStatus = HttpContext.Request.Query["vnp_TransactionStatus"];
        if (transactionStatus == "00")
        {
            var vnpayRef = HttpContext.Request.Query["vnp_TxnRef"].ToString();
            Console.WriteLine(vnpayRef);
            using (var db = new shopmanagementContext())
            {
                var order = db.Orders.Where(o => o.VnpayRef == vnpayRef).FirstOrDefault();
                if (order == null)
                {
                    return NotFound(); // Return 404 if accessories not found
                }
                Console.WriteLine(order.Id.ToString());
                order.Status = "Paid";
                db.SaveChanges();
            }
        }
        return RedirectToAction("Payment", "Home", new { success = true });
    }
}
