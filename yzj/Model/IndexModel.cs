using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace yzj.Model
{
    public class IndexModel
    {
        private readonly IConfiguration _config;
        public IndexModel(IConfiguration config)
        {
            this._config = config;
        }
    }
}
