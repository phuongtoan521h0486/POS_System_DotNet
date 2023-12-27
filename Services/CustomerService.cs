
using POS_System_DotNet.Data;
using POS_System_DotNet.Models.Customer;
using POS_System_DotNet.Models.Order;

namespace POS_System_DotNet.Services
{
    public class CustomerService
    {
        private MyContext ct;
        public CustomerService(MyContext ct)
        {
            this.ct = ct;
        }
        public int calculateTotalOrder(int customerId)
        {
            return ct.Orders.Count(x => x.CustomerId == customerId);
        }

        public double calculateTotalSpend(int customerId)
        {
            List<Order> orders = ct.Orders.Where(x =>  x.CustomerId == customerId).ToList();
            double total = 0;
            foreach (Order order in orders)
            {
                total += order.TotalAmount;
            }
            return total;
        }

        public Customer getCustomerbyId(int customerId)
        {
            return ct.Customers.FirstOrDefault(x => x.CustomerId == customerId);
        }


        public int countAllProductPurchased(int customerId)
        {
            List<Order> orders = ct.Orders.Where(x => x.CustomerId == customerId).ToList();
            int total = 0;
            foreach (Order order in orders)
            {
                var list = ct.OrderDetails.Where(x => x.OrderId == order.OrderId);
                foreach(var item in list)
                {
                    total += item.Quantity;
                }
            }
            return total;
        }
    }
}
