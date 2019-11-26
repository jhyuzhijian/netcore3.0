using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Extensions
{
   public static class TypeExtension
    {
        public static bool IsNullable(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}
