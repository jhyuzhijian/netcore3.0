using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using yzj.Server;

namespace yzj.Controllers
{
    //允许的response类型
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BasicDbContext _context;
        public HomeController(ILogger<HomeController> logger, BasicDbContext context)
        {
            _logger = logger;
            _context = context;
            _logger.LogDebug(1, "NLog injected into HomeController");
        }
        public string Index()
        {

            string groupName = string.Empty;

            var group = _context.Groups.FirstOrDefault();
            groupName = group.Name;

            _logger.LogInformation("Hello, this is the index!");
            return groupName;
        }
    }
}