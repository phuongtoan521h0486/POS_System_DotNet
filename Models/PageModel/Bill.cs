using POS_System_DotNet.Services;

namespace POS_System_DotNet.Models.PageModel
{
    public class Bill
    {
        public int customerId { get; set; }
        public int OrderId { get; set; }
        public string staffName { get; set; }
        public string customerName { get; set; }
        public string address { get; set; }

        public OrderService orderService { get; set; }

        public OrderDetailService orderDetailService { get; set; }

        public double totalAmount { get; set; }
        public double givenMoney { get; set; }
        public double excessMoney { get; set; }

        public DateTime orderDate { get; set; }
    }
}
