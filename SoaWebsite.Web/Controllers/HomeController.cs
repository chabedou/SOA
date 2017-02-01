using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SoaWebsite.Common;
using Newtonsoft.Json;
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

        public IActionResult FromWebService()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5555/");
            var request = client.GetAsync("/api/values").Result;
            var result = request.Content.ReadAsStringAsync().Result;
            var list = JsonConvert.DeserializeObject<List<User>>(result);
            return View(model: list);
        }
    }
}
