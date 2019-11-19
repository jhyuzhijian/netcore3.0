using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;

namespace Infrastructure
{
    public static class GlobalData
    {
        static readonly List<string> _loadAssemblies =
            new List<string>
            {
                "yzj"
            };
        static GlobalData()
        {
            var assemblys = _loadAssemblies.Select(x => Assembly.Load(x)).ToList();
            List<Type> allTypes = new List<Type>();
            assemblys.ForEach(assembly =>
            {
                allTypes.AddRange(assembly.GetTypes());
            });
            LoadedTypes = allTypes;
        }
        public static readonly List<Type> LoadedTypes;
    }
}
