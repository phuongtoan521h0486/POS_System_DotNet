using Microsoft.AspNetCore.Mvc;
using POS_System_DotNet.Data;
using POS_System_DotNet.Models.Account;
using POS_System_DotNet.Models.Product;

namespace POS_System_DotNet.Controllers
{
    public class ProductController : Controller
    {
        private MyContext ct;
        public ProductController(MyContext ct)
        {
            this.ct = ct;
        }
        public IActionResult Index()
        {
            var account = HttpContext.Session.GetObject<Account>("account");
            ViewBag.Role = account.Role;
            return View(ct.Products);
        }
        [HttpGet]
        public IActionResult Add()
        {
            var account = HttpContext.Session.GetObject<Account>("account");
            if (account.Role == "Employee")
            {
                return RedirectToAction("Index", "Product");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Add(string productName, string barcode, string retailPrice, string importPrice, string category, IFormFile picture)
        {
            Product product = new Product();
            product.ProductName = productName;
            product.Barcode = barcode;
            product.RetailPrice = Convert.ToDouble(retailPrice);
            product.ImportPrice = Convert.ToDouble(importPrice);
            product.Category = category;
            product.Picture = ConvertIFormFileToByteArray(picture);
            product.CreationDate = DateTime.Now;

            ct.Products.Add(product);
            ct.SaveChanges();

            return RedirectToAction("Index", "Product");
        }
        private byte[] ConvertIFormFileToByteArray(IFormFile file)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        [HttpGet]
        
        public IActionResult Edit(int? id)
        {
            if (id == null) return RedirectToAction("Index");
            Product p = ct.Products.FirstOrDefault(x => x.ProductId == id);
            if (p == null) return RedirectToAction("Index");
            return View(p);
        }
        [HttpPost]
        public IActionResult Edit(string productId, string productName, string barcode, string retailPrice, string importPrice, string category, IFormFile picture)
        {
            Product product = ct.Products.FirstOrDefault(x => x.ProductId == Convert.ToInt32(productId));
            product.ProductName = productName;
            product.Barcode = barcode;
            product.RetailPrice = Convert.ToDouble(retailPrice);
            product.ImportPrice = Convert.ToDouble(importPrice);
            product.Category = category;
            if (picture != null)
            {
                product.Picture = ConvertIFormFileToByteArray(picture);
            }
            

            ct.Update(product);
            ct.SaveChanges(true);
            return RedirectToAction("Index", "Product");
        }


        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if(ct.OrderDetails.Where(x => x.ProductId == id).Count() == 0)
            {
                Product p = ct.Products.FirstOrDefault(x => x.ProductId == id);
                ct.Products.Remove(p);
                ct.SaveChanges();
            }
            
            return RedirectToAction("Index");
        }
    }
}
