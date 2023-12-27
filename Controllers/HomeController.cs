using Microsoft.AspNetCore.Mvc;
using POS_System_DotNet.Data;
using POS_System_DotNet.Models;
using POS_System_DotNet.Models.Account;
using System.Diagnostics;

namespace POS_System_DotNet.Controllers
{
    
    public class HomeController : Controller
    {
        private MyContext ct;
        public HomeController(MyContext ct)
        {
            this.ct = ct;
        }

        [HttpGet]
        public IActionResult Index()
        {
            Account account = HttpContext.Session.GetObject<Account>("account");
            if (account == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(ct.Products);         
        }


    }
}