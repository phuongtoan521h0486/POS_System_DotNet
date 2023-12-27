using Microsoft.AspNetCore.Mvc;
using POS_System_DotNet.Data;
using POS_System_DotNet.Models.Account;
using POS_System_DotNet.Models.Customer;
using POS_System_DotNet.Models.Order;
using POS_System_DotNet.Models.PageModel;
using POS_System_DotNet.Models.Product;
using POS_System_DotNet.Services;

namespace POS_System_DotNet.Controllers
{
    public class CartController : Controller
    {
        private MyContext ct;
        public CartController(MyContext ct)
        {
            this.ct = ct;
        }
        [HttpGet("cart/step-1")]
        public ActionResult Step1()
        {
            return View();
        }

        [HttpPost("cart/step-1")]
        public ActionResult Step1(string phone)
        {
            Customer customer = ct.Customers.FirstOrDefault(x => x.PhoneNumber == phone);
            HttpContext.Session.SetObject("customer", customer);
            Step2Model model = new Step2Model() { cartItem = HttpContext.Session.GetObject<List<CartItem>>("Cart"), cartService = new Services.CartService() };
            return View("Step2", model);
        }

        [HttpPost("cart/step-2")]
        public ActionResult Step2(double givenMoney)
        {
            var customer = HttpContext.Session.GetObject<Customer>("customer");
            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart");
            var staff = HttpContext.Session.GetObject<Account>("account");
            CartService cartService = new Services.CartService();
            
            Order order = new Order();
            order.OrderDate = DateTime.Now;
            order.TotalAmount = cartService.calculateTotalAmount(cart);
            order.GivenMoney = givenMoney;
            order.ExcessMoney = order.GivenMoney - order.TotalAmount;
            order.Quantity = cart.Count();
            order.CustomerId = customer.CustomerId;
            order.AccountId = staff.AccountId;

            ct.Orders.Add(order);
            ct.SaveChanges();

            int orderId = order.OrderId;
            foreach(var item in cart)
            {
                OrderDetail detail = new OrderDetail();
                detail.OrderId = orderId;
                detail.ProductId = item.product.ProductId;
                detail.Quantity = item.Quantity;

                ct.OrderDetails.Add(detail);
                ct.SaveChanges();
            }

            Step3Model model = new Step3Model()
            {
                orderId = orderId,
                staffName = staff.FullName,
                customerName = customer.Name,
                address = customer.Address,
                orderDate = order.OrderDate,
                cartItems = cart,
                totalAmount = order.TotalAmount,
                givenMoney = order.GivenMoney,
                excessMoney = order.ExcessMoney
            };

            HttpContext.Session.Clear();
            HttpContext.Session.SetObject("account", staff);

            return View("Step3", model);
        }

        [HttpPost("cart/step-3")]
        public ActionResult Step3()
        {
            
            return RedirectToAction("Index", "Home");
        }

        [HttpPost("cart/add")]
        public IActionResult AddToCart([FromBody] CartItemDto itemDto)
        {
            try
            {
            
                var cartItems = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();

             
                var existingItem = cartItems.FirstOrDefault(item => item.product.ProductId == itemDto.ProductId);
                if (existingItem != null)
                {
                 
                    existingItem.Quantity += itemDto.Quantity;
                }
                else
                {
            
                    var newItem = new CartItem
                    {
                        product = ct.Products.FirstOrDefault(x => x.ProductId == itemDto.ProductId),
                        Quantity = itemDto.Quantity
                    };

                    cartItems.Add(newItem);
                }

                HttpContext.Session.SetObject("Cart", cartItems);

                return Ok(new { Message = "Product added to the cart successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = "Failed to add product to the cart" });
            }
        }

        [HttpGet("cart/getCartQuantity")]
        public IActionResult GetCartQuantity()
        {
            try
            {
                var cartItems = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();


                int cartQuantity = cartItems.Sum(item => item.Quantity);

                return Ok(cartQuantity);
            }
            catch (Exception ex)
            {

                return BadRequest(new { Error = "Failed to retrieve cart quantity" });
            }
        }

        [HttpPost("cart/remove")]
        public IActionResult RemoveItemFromCart([FromBody] int productId)
        {
            try
            {
                var cartItems = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();
                var itemToRemove = cartItems.FirstOrDefault(item => item.product.ProductId == productId);
                if (itemToRemove != null)
                {
                    cartItems.Remove(itemToRemove);
                }

                HttpContext.Session.SetObject("Cart", cartItems);

                return Ok(new { Message = "Item removed from cart successfully" });
            }
            catch (Exception ex)
            {

                return BadRequest(new { Error = "Failed to remove item from cart" });
            }
        }

        [HttpGet("cart/info")]
        public IActionResult GetCartInfo()
        {
            try
            {
                var cartItems = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();
                var totalQuantity = cartItems.Sum(item => item.Quantity);
                var totalAmount = cartItems.Sum(item => item.Quantity * item.product.RetailPrice);

                return Ok(new { TotalQuantity = totalQuantity, TotalAmount = totalAmount });
            }
            catch (Exception ex)
            {

                return BadRequest(new { Error = "Failed to fetch cart information" });
            }
        }

        [HttpPost("cart/update")]
        public IActionResult UpdateCartItem([FromBody] CartUpdateRequest request)
        {
            try
            {
                var cartItems = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();
                var cartItem = cartItems.FirstOrDefault(item => item.product.ProductId == request.ProductId);

                if (cartItem != null)
                {

                    cartItem.Quantity = request.Quantity;
                }


                HttpContext.Session.SetObject("Cart", cartItems);

                return Ok(new { Message = "Cart updated successfully" });
            }
            catch (Exception ex)
            {

                return BadRequest(new { Error = "Failed to update cart" });
            }
        }

        [HttpGet("api/getCart")]
        public IActionResult GetCart()
        {
            try
            {
                var cartItems = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();

                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return BadRequest(new { Error = "Failed to fetch cart information" });
            }
        }


    }


    public class CartItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class CartUpdateRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class CartItemRequest
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
