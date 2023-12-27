using POS_System_DotNet.Models.Account;
using POS_System_DotNet.Models.Customer;
using POS_System_DotNet.Models.Product;
using POS_System_DotNet.Services;

namespace POS_System_DotNet.Models.PageModel
{
    public class Step3Model
    {
        public int orderId { get; set; }
        public string staffName { get; set; }
        public string customerName { get; set; }
        public string address { get; set; }
        public DateTime orderDate { get; set; }
        public List<CartItem> cartItems { get; set; }
        public double totalAmount { get; set; }
        public double givenMoney { get; set; }
        public double excessMoney { get; set; }
    }
}
