using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MuseMusic.Models;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MuseMusic.Models.Tables;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MuseMusic.Controllers;

public class LoginController : Controller
{
    private readonly ILogger<LoginController> _logger;
    private readonly shopmanagementContext _context;

    public LoginController(ILogger<LoginController> logger, shopmanagementContext context)
    {
        _logger = logger;
        _context = context;
    }

    [AllowAnonymous]  // Allow anonymous access
    public IActionResult Login()
    {
        // Check if the user is already authenticated
        if (User.Identity.IsAuthenticated)
        {
            // If the user is authenticated, redirect them to the homepage with a message
            TempData["Message"] = "You are already logged in.";
            return RedirectToAction("Index", "Home");  // Redirect to homepage
        }

        // Otherwise, show the login page
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string login, string password)
    {
        // Find the account by email or username
        var account = await _context.Accounts
            .FirstOrDefaultAsync(a => a.Email == login || a.Username == login); // Check both email and username

        if (account == null)
        {
            // If account not found
            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }

        // Check password validity
        bool isValidPassword = VerifyPassword(password, account.Password);

        if (isValidPassword)
        {
            // If password is valid, proceed with login
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, account.Username),  // Always store the username, even if logged in with email
            new Claim(ClaimTypes.NameIdentifier, account.Id.ToString())
        };

            // Get user roles
            var roles = await _context.AccountRoles
                .Where(ur => ur.AccountId == account.Id)
                .Select(ur => ur.Role.Name)
                .ToListAsync();

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Sign in the user
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // Redirect based on role
            if (roles.Contains("Admin"))
            {
                return RedirectToAction("Categories", "Admin");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        else
        {
            // If password is incorrect, rehash if needed
            if (NeedsRehashing(account.Password))
            {
                var newHash = BCrypt.Net.BCrypt.HashPassword(password);
                account.Password = newHash;

                _context.Update(account);
                await _context.SaveChangesAsync();
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }
    }


    public static bool VerifyPassword(string password, string hashedPassword)
    {
        try
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
        catch (BCrypt.Net.SaltParseException ex)
        {
            // Log the exception (optional)
            Console.WriteLine("Invalid bcrypt salt version: " + ex.Message);
            return false;
        }
    }

    public static bool NeedsRehashing(string hashedPassword)
    {
        // Check if the password hash needs rehashing (e.g., length check or if it's a weak version)
        return hashedPassword.Length < 60 || !BCrypt.Net.BCrypt.Verify("test", hashedPassword); // Example: testing the strength
    }

    // Log out logic
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");  // Đảm bảo bạn chuyển hướng về trang chủ sau khi đăng xuất
    }


    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View(); // Return an Access Denied view with a message
    }

    public async Task LoginGG()
    {
        await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
        {
            RedirectUri = Url.Action("GoogleResponse")
        });
    }

    public async Task<IActionResult> GoogleResponse()
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
        {
            claim.Issuer,
            claim.OriginalIssuer,
            claim.Type,
            claim.Value
        });
        return Json(claims);
    }

    public IActionResult Register()
    {
        return View();
    }

    public IActionResult Lostpass()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
