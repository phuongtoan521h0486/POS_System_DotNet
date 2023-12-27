using POS_System_DotNet.Data;
using POS_System_DotNet.Models.Order;

namespace POS_System_DotNet.Services
{
    public class OrderService
    {
        private MyContext ct;
        public OrderService(MyContext ct)
        {
            this.ct = ct;
        }

        public List<Order> getOrdersByCustomerId(int customerId)
        {
            return ct.Orders.Where(x => x.CustomerId == customerId).ToList();
        }

        public Order getLastOrder(int customerId)
        {
            
           return ct.Orders
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.OrderDate)
            .FirstOrDefault();
        }

        public Order getOrderByOrderId(int orderId)
        {

            return ct.Orders.FirstOrDefault(x => x.OrderId == orderId);
            
        }
    }
}
