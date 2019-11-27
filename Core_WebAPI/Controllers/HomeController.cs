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
using Infrastructure.IDbContext;
using Microsoft.Samples.EFLogging;

namespace yzj.Controllers
{
    //允许的response类型
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDbContextCore _context;
        public HomeController(ILogger<HomeController> logger, IDbContextCore context)
        {
            _logger = logger;
            _context = context;
        }
        public string Index()
        {
            string groupName = string.Empty;

            _logger.LogInformation("Hello, this is the index!");
            return "";
        }
        public void Test()
        {
            //var model = _context.GetDbSet<Role>().FirstOrDefault();
            string sql = "";
            _context.GetDbSet<Group>().ConfigureLogging(p => sql = p, null);
            var groupList = _context.GetDbSet<Group>()
                .Where(p => p.Name.Contains("省"))
                .Where(p => p.Id > 1)
                .ToList();
            //_context.GetDbSet<Group>().AttachRange(groupList);
            //groupList.ForEach(p => p.Name += "3");
            //int i = _context.SaveChanges();
            var datatable = _context.GetDataTable("select * from [user].[group]", null);

        }
    }
}