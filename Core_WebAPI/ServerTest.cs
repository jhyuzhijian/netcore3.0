using Core_Entity.Entity;
using Infrastructure.IDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core_WebAPI
{
    public class ServerTest
    {
        private IDbContextCore _context;
        public ServerTest(IDbContextCore context)
        {
            _context = context;
        }
        public dynamic GetGroup()
        {
            return _context.GetDbSet<Group>().ToList();
        }
    }
}
