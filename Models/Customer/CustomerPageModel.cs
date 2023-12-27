using POS_System_DotNet.Services;

namespace POS_System_DotNet.Models.Customer
{
    public class CustomerPageModel
    {
        public CustomerService customerService { get; set; }
        public List<Customer> customers { get; set; }
    }
}
