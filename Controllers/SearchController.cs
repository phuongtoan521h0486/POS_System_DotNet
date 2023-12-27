using Microsoft.AspNetCore.Mvc;
using POS_System_DotNet.Data;
using POS_System_DotNet.Models.Product;

namespace POS_System_DotNet.Controllers
{
    [ApiController]
    [Route("api")]
    public class SearchController : ControllerBase
    {
        private MyContext ct;
        public SearchController(MyContext ct)
        {
            this.ct = ct;
        }

        [HttpGet("search")]
        public IActionResult SearchProducts(string keyword)
        {
            if (keyword == null || keyword == "" || keyword == "all") 
            {
                return Ok(ct.Products.ToList());
            }
            try
            {
                var searchResults = ct.Products.Where(x => x.ProductName.Contains(keyword)).ToList();

                return Ok(searchResults);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = "Failed to perform the product search" });
            }
        }

        [HttpGet("checkBarcode")]
        public IActionResult CheckBarcode(string barcode)
        {
            try
            {
                
                var product = ct.Products.FirstOrDefault(p => p.Barcode == barcode);

                if (product != null)
                {
                    var cartItems = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();
                    var existingCartItem = cartItems.FirstOrDefault(item => item.product.ProductId == product.ProductId);

                    if (existingCartItem != null)
                    {
                        // Increment the quantity if the product is already in the cart
                        existingCartItem.Quantity += 1;
                    }
                    else
                    {
                        // Add the new product to the cart
                        cartItems.Add(new CartItem
                        {
                            product = ct.Products.FirstOrDefault(x => x.ProductId == product.ProductId),
                            Quantity = 1
                        });
                    }
                    HttpContext.Session.SetObject("Cart", cartItems);
                    return Ok(new { ProductId = product.ProductId });
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return BadRequest(new { Error = "Error checking barcode" });
            }
        }
    }
}