using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace yzj.Configuration
{
    public class Configure
    {
        public void GetConfigure(IConfiguration config)
        {
            Poto poto = new Poto();
            //config.GetSection("配置文件节点名").Bind(poto);
            var _poto = config.GetSection("配置文件节点名").Get<Poto>();
        }
    }
    public class Poto
    {
        public string Name { get; set; }
        public int age { get; set; }
    }
}
