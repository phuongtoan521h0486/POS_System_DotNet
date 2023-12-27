using POS_System_DotNet.Models.Product;
using POS_System_DotNet.Services;

namespace POS_System_DotNet.Models.PageModel
{
    public class Step2Model
    {
        public List<CartItem> cartItem { get; set; }
        public CartService cartService { get; set; }
    }
}
