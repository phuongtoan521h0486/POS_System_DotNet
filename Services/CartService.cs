using POS_System_DotNet.Models.Product;

namespace POS_System_DotNet.Services
{
    public class CartService
    {
        public CartService() { }
        public double calculateTotalAmount(List<CartItem> cartItems)
        {
            double totalAmount = 0;
            foreach (var item in cartItems)
            {
                totalAmount += item.product.RetailPrice * item.Quantity;
            }
            return totalAmount;
        }

        public int calculateTotalQuantity(List<CartItem> cartItems)
        {
            int totalQuantity = 0;
            foreach (var item in cartItems)
            {
                totalQuantity += item.Quantity;
            }
            return totalQuantity;
        }
    }
}
