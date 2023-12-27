using Microsoft.AspNetCore.Mvc;
using POS_System_DotNet.Data;
using POS_System_DotNet.Models.Account;
using BCrypt.Net;
using POS_System_DotNet.Services;
using Org.BouncyCastle.Crypto.Generators;

namespace POS_System_DotNet.Controllers
{
    public class AccountController : Controller
    {
        private MyContext ct;
        public AccountController(MyContext ct)
        {
            this.ct = ct;
        }
        [HttpGet("Account/Login")]
        public IActionResult Login()
        {
            if (ct.Accounts.Where(x => x.Username == "admin").Count() == 0)
            {
                Account account = new Account();
                account.Username = "admin";
                account.Password = BCrypt.Net.BCrypt.HashPassword("admin");
                account.Role = "Admin";
                account.Email = "admin@gmail.com";
                account.IsActivate = true;
                account.Status = true;
                account.Picture = System.IO.File.ReadAllBytes("wwwroot/images/avatar-default.png");
                account.Phone = "";
                ct.Accounts.Add(account);
                ct.SaveChanges();
            }
            return View();
        }
        [HttpGet("/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Login");
        }
        
        [HttpPost("Account/Login")]
        public IActionResult Login(string username, string password, string token)
        {
            if (token != null)
            {
                string newUser = TokenService.GetUsernameFromToken(token);
                if (username == newUser && newUser != null)
                {
                    return View("Welcome", ct.Accounts.SingleOrDefault(x => x.Username == newUser));
                }
            }
            var user = ct.Accounts.SingleOrDefault(x => x.Username == username);
            if (user != null)
            {
                if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    if (!user.IsActivate)
                    {
                        return View();
                    }
                    HttpContext.Session.SetObject("account", user);
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Invalid email or password");

            return View();
        }
       

        [HttpPost]
        public IActionResult Welcome(int accountId, string password)
        {
            Account account = ct.Accounts.SingleOrDefault(x => x.AccountId == accountId);
            account.Password = BCrypt.Net.BCrypt.HashPassword(password);
            account.IsActivate = true;
            ct.Accounts.Update(account);
            ct.SaveChanges();
            return RedirectToAction("Login", "Account");
        }
    }
}
