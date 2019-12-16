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
using Infrastructure.Tool.Base;
using Infrastructure.CaChe;
using System.Net.Http;
using Newtonsoft.Json;

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
        private readonly IHttpClientFactory _clientFactory;
        public HomeController(ILogger<HomeController> logger, IDbContextCore context, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _context = context;
            _clientFactory = clientFactory;
        }
        [HttpPost()]
        public string Index()
        {
            string groupName = string.Empty;

            _logger.LogInformation("Hello, this is the index!");
            return "";
        }
        static List<string> sql = new List<string>();
        [HttpGet]
        public void Test()
        {
            var hj = new Core_WebAPI.ServerTest(_context).GetGroup();
            //_context.GetDbSet<OtherCredit>().ConfigureLogging(p => sql.Add(p), null);

            //var list = _context.GetDbSet<OtherCredit>().ToList();
            //var list2 = _context.GetDbSet<TrainOtherCredit>().ToList();
            var relationDbSet = _context.GetDbSet<Relation>();
            var hg = (from c in relationDbSet
                      where c.RelationExtend.Address == "l"
                      select c)
                      .Include(p => p.RelationExtend)
                   .ToList();
            var list = _context.GetDbSet<Relation>().Include(p => p.RelationExtend).ToList();
            var list2 = _context.GetDbSet<RelationExtend>().Include(p => p.Relation).ToList();
            //_context.GetDbSet<Group>().AttachRange(groupList);
            //groupList.ForEach(p => p.Name += "3");
            //int i = _context.SaveChanges();
            var datatable = _context.GetDataTable("select * from [user].[group]", null);

        }
        [HttpGet]
        public Result TestLoggerProviderLifeTime()
        {
            var result = new Result();
            var role = _context.GetDbSet<Role>().FirstOrDefault();
            Console.WriteLine(sql.ToString());
            result.@object = role;
            result.code = 1;
            return result;
        }
        [HttpPost]
        public IActionResult AddRole(Dto_Test test)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok();
        }
        [HttpGet]
        [Route("Code/Name")]
        public async Task OnGet()
        {
            string requestUrl = "https://api.github.com/repos/aspnet/AspNetCore.Docs/branches";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("Accept", "application/vnd.github.v3+json");
            request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = response.Content.ReadAsStreamAsync();
                var k = await System.Text.Json.JsonSerializer.DeserializeAsync
                       <IEnumerable<dynamic>>(responseStream.Result);
                var responseStream2 = response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<IEnumerable<dynamic>>(responseStream2.Result);
            }
            else
            {

            }
        }
    }
}