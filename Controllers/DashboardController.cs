using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS_System_DotNet.Data;
using POS_System_DotNet.Models.Account;
using POS_System_DotNet.Models.Order;
using POS_System_DotNet.Models.PageModel;
using POS_System_DotNet.Services;

namespace POS_System_DotNet.Controllers
{
    public class DashboardController : Controller
    {
        private MyContext ct;
        public DashboardController(MyContext ct)
        {
            this.ct = ct;
        }

        [HttpGet("user/dashboard")]
        public IActionResult Dashboard(DateTime? startDate, DateTime? endDate)
        {
            var account = HttpContext.Session.GetObject<Account>("account");
       
            var model = new DashboardPageModel() {
                accountId = account.AccountId,
                picture = account.Picture,
                name = account.FullName,
                startDate = startDate == null ? DateTime.Today.Date : startDate,
                endDate = endDate == null? DateTime.Today.Date : endDate,
                service = new Services.DashboardService(ct),
                accountService = new Services.AccountService(ct),
                customerService = new Services.CustomerService(ct),
            };


            return View(model);
        }

        [HttpPost("dashboard/getDataBarChart")]
        public IActionResult GetDataBarChart(DateTime? startDate, DateTime? endDate)
        {
            startDate = startDate == null ? DateTime.Today.Date : startDate;
            endDate = endDate == null ? DateTime.Today.AddDays(1).Date : endDate;
            DashboardService service = new DashboardService(ct);


            var barChartDataSet = service.getBarChart(startDate.Value, endDate.Value);

            return Json(barChartDataSet);
        }
    }
    

}