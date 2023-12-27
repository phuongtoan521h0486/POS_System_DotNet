using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using POS_System_DotNet.Data;
using POS_System_DotNet.Models.Account;
using POS_System_DotNet.Services;
using System.Data;

namespace POS_System_DotNet.Controllers
{
    public class UserController : Controller
    {
        private readonly IEmailService _emailService;   
        private MyContext ct;
        public UserController(MyContext ct, IEmailService emailService)
        {
            this.ct = ct;
            this._emailService = emailService;
        }
        public IActionResult Index()
        {
            var account = HttpContext.Session.GetObject<Account>("account");
            if (account.Role == "Employee")
            {
                return RedirectToAction("Dashboard", "User");
            }
            return View(ct.Accounts);
        }

        public IActionResult Add()
        {
            var account = HttpContext.Session.GetObject<Account>("account");
            if (account.Role == "Employee")
            {
                return RedirectToAction("Dashboard", "User");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Add(string fullName, string email)
        {
            Account account = new Account();
            account.FullName = fullName;
            account.Email = email;
            account.Username = email.Split('@')[0];
            account.Password = BCrypt.Net.BCrypt.HashPassword(account.Username);
            account.Status = true;
            account.IsActivate = false;
            account.Role = "Employee";
            account.Picture = System.IO.File.ReadAllBytes("wwwroot/images/avatar-default.png");
            account.Phone = "";
            ct.Accounts.Add(account);
            ct.SaveChanges();

            var token = TokenService.GenerateToken(account.Username);
            string body = $"http://localhost:5053/Account/Login?token={token}";
            _emailService.SendEmail(account.Email, "Invitation", body);
            return RedirectToAction("Index");
        }

        public IActionResult Profile() 
        {
            Account account = HttpContext.Session.GetObject<Account>("account");
            return View(account);
        }

        [HttpPost]
        public IActionResult Profile(string name, string email, string phone, IFormFile pictureFile)
        {
            Account account = HttpContext.Session.GetObject<Account>("account");
            account.FullName = name;
            account.Email = email;
            account.Phone = phone;
            if (pictureFile != null)
            {
                account.Picture = ConvertIFormFileToByteArray(pictureFile);
            }
            
            ct.Accounts.Update(account);
            ct.SaveChanges();
            return RedirectToAction("Index");
        }
        private byte[] ConvertIFormFileToByteArray(IFormFile file)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public IActionResult Block(int id)
        {
            Account account = ct.Accounts.FirstOrDefault(x => x.AccountId == id);
            account.Status = !account.Status;
            ct.Accounts.Update(account);
            ct.SaveChanges(true);
            return RedirectToAction("Index");
        }

        public IActionResult Enable(int id)
        {
            Account account = ct.Accounts.FirstOrDefault(x => x.AccountId == id);
            account.Status = !account.Status;
            ct.Accounts.Update(account);
            ct.SaveChanges(true);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Account account = ct.Accounts.FirstOrDefault(x => x.AccountId == id);
            ct.Accounts.Remove(account);
            ct.SaveChanges(true);
            return RedirectToAction("Index");
        }

        public IActionResult Resend(int id)
        {
            Account account = ct.Accounts.FirstOrDefault(x => x.AccountId == id);
            var token = TokenService.GenerateToken(account.Username);
            string body = $"http://localhost:5053/Account/Login?token={token}";
            _emailService.SendEmail(account.Email, "Resend Email", body);
            return RedirectToAction("Index");
        }

		[HttpPost]
		public IActionResult ChangePassword(string password, string oldPassword)
		{
			Account account = HttpContext.Session.GetObject<Account>("account");

            if (!BCrypt.Net.BCrypt.Verify(oldPassword, account.Password))
            {
                return RedirectToAction("Profile", "User");
            }

			account.Password = BCrypt.Net.BCrypt.HashPassword(password);


			ct.Accounts.Update(account);
			ct.SaveChanges();
			return RedirectToAction("Logout", "Account");
		}
	}
}
