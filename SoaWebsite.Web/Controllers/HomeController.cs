using Microsoft.AspNetCore.Mvc;
using SoaWebsite.Web.ViewModels;


namespace SoaWebsite.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Skills matcher helps you find developers with given skills";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "You can send us an email or a mail";

            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel vm)
        {
            if(ModelState.IsValid)
            {
                // here we use an stmp server to send email
                ViewBag.Message = "Thank you for Contacting us ";
                return Redirect("Index");        
            }
            return View(vm);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
