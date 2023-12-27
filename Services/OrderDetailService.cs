using POS_System_DotNet.Data;
using POS_System_DotNet.Models.Product;
using System.Linq;

namespace POS_System_DotNet.Services
{
    public class OrderDetailService
    {
        private MyContext ct;
        public OrderDetailService(MyContext ct)
        {
            this.ct = ct;
        }

        public List<CartItem> getOrderDetailsByOrderId(int orderId)
        {
            var details = ct.OrderDetails.Where(x => x.OrderId == orderId).ToList();
            List<CartItem> list = new List<CartItem>();
            foreach (var item in details)
            {
                CartItem cartItem = new CartItem()
                {
                    product = ct.Products.FirstOrDefault(x => x.ProductId == item.ProductId),
                    Quantity = item.Quantity,
                };
                list.Add(cartItem);
            }
            return list;
        }
    }
}
