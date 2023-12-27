using Microsoft.AspNetCore.Mvc;

namespace POS_System_DotNet.Controllers
{
	[Route("ErrorPage/")]
	public class ErrorController : Controller
	{
		public IActionResult Error()
		{
			return View("Error");
		}
	}
}
