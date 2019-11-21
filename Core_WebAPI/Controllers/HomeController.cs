using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Core_Entity.Entity;
using Core_Entity.Dto;
using Infrastructure;
using Infrastructure.AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
        }
        public string Index()
        {
            string groupName = string.Empty;

            var group = _context.Groups.FirstOrDefault();
            Dto_Group dtoGroup = group.MapTo<Dto_Group>();
            groupName = group.Name;

            _logger.LogInformation("Hello, this is the index!");
            return dtoGroup.GroupName;
        }
        public void Test()
        {
            var model = _context.Set<Role>().FirstOrDefault();


        }
    }
}