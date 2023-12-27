using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Controller;
using Org.BouncyCastle.Crypto.Prng.Drbg;
using POS_System_DotNet.Data;
using POS_System_DotNet.Models.Account;
using POS_System_DotNet.Models.Customer;
using POS_System_DotNet.Models.PageModel;
using POS_System_DotNet.Services;

namespace POS_System_DotNet.Controllers
{
    public class CustomerController : Controller
    {
        private MyContext ct;
        public CustomerController(MyContext ct)
        {
            this.ct = ct;
        }
        [HttpPost]
        public IActionResult Create([FromBody] CustomerModel model)
        {
            Customer customer = new Customer() { PhoneNumber = model.PhoneNumber, Name = model.Name, Address = model.Address };
            ct.Customers.Add(customer);
            ct.SaveChanges();
            return Json(new { success = true, message = "Customer created successfully" });
        }

        [HttpGet("/customer/check")]
        public IActionResult CheckCustomer(string phone)
        {
            var customer = ct.Customers.FirstOrDefault(c => c.PhoneNumber == phone);

            if (customer == null)
            {
                return NotFound();
            }
            
            return Json(new { name = customer.Name, address = customer.Address });
        }

  
        public IActionResult Index()
        {
            var model = new CustomerPageModel()
            {
                customers = ct.Customers.ToList(),
                customerService = new CustomerService(ct)
            };
            var account = HttpContext.Session.GetObject<Account>("account");
            ViewBag.Role = account.Role;
            return View(model);
        }

        [HttpPost("/Customer/Edit/{id}")]
        public IActionResult Edit(string name, string phoneNumber, string address, int id)
        {
            Customer customer = ct.Customers.FirstOrDefault(x => x.CustomerId == id);
            customer.Name = name;
            customer.Address = address;
            customer.PhoneNumber = phoneNumber;

            ct.Customers.Update(customer);
            ct.SaveChanges(true);
            return RedirectToAction("Index");
        }

        [HttpPost("/Customer/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            Customer customer = ct.Customers.FirstOrDefault(x => x.CustomerId == id);
      

            ct.Customers.Remove(customer);
            ct.SaveChanges(true);
            return RedirectToAction("Index");
        }

        [HttpGet("/Customer/{id}")]
        public IActionResult Detail(int id)
        {
            DetailCustomer customer = new DetailCustomer()
            {
                customerId = id,
                accountService = new AccountService(ct),
                customerService = new CustomerService(ct),
                orderService = new OrderService(ct)
            };
            return View(customer);
        }

        [HttpGet("/Customer/{customerId}/invoice/{orderId}")]
        public IActionResult Bill(int customerId, int orderId)
        {
            var accountService = new AccountService(ct);
            var order = ct.Orders.FirstOrDefault(x => x.OrderId == orderId);
            var customer = ct.Customers.FirstOrDefault(x => x.CustomerId == customerId);
            Bill model = new Bill()
            {
                customerId = customerId,
                OrderId = orderId,
                staffName = accountService.findAccountById(order.AccountId).FullName,
                customerName = customer.Name,
                address = customer.Address,
                orderService = new OrderService(ct),
                orderDetailService = new OrderDetailService(ct),
                totalAmount = order.TotalAmount,
                givenMoney = order.GivenMoney,
                excessMoney = order.ExcessMoney,
                orderDate = order.OrderDate,
            };
            return View(model);
        }
    }

    public class CustomerModel
    {
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }

    
}
