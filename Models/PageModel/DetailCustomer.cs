using POS_System_DotNet.Services;

namespace POS_System_DotNet.Models.PageModel
{
    public class DetailCustomer
    {
        public int customerId { get; set; }
        public CustomerService customerService { get; set; }
        public OrderService orderService { get; set; }

        public AccountService accountService { get; set; }

    }
}
