using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SoaWebsite.Web.Models;
using SoaWebsite.Services.Services;
using Newtonsoft.Json;

namespace SoaWebsite.Services.Controllers
{
    [Route("api/[controller]")]
    public class DeveloperController : Controller
    {
        private readonly DeveloperService service;
        private readonly DeveloperContext _context;
        public DeveloperController(DeveloperContext context)
        {
            service = new DeveloperService(context);
            _context = context;
        }
        [HttpGet]
        public IEnumerable<Developer> GetAllDevelopers()
        {
            return service.GetAllDevelopers();
        }
        
        [HttpGet("{id}")]
        public Developer DeveloperById(int id)
        {
            var developer = _context.Developers
                                    .SingleOrDefault(m => m.ID == id);
            return developer;
        }
    }
}