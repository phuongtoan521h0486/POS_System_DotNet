using POS_System_DotNet.Services;

namespace POS_System_DotNet.Models.PageModel
{
    public class DashboardPageModel
    {
        public int accountId { get; set; }
        public byte[] picture { get; set; }
        public string name { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public DashboardService service { get; set; }

        public AccountService accountService { get; set; }

        public CustomerService customerService { get; set; }
    }
}
