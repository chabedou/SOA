using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SoaWebsite.Common;
using Newtonsoft.Json;

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
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
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
