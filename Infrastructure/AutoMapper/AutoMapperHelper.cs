using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using System.Collections;

namespace Infrastructure.AutoMapper
{
    public static class AutoMapperHelper
    {
        private static IServiceProvider ServiceProvider;
        public static void UseStateAutoMapper(this IApplicationBuilder applicationBuilder)
        {
            ServiceProvider = applicationBuilder.ApplicationServices;
        }
        public static TDestination Map<TDestination>(object Source)
        {
            var mapper = ServiceProvider.GetRequiredService<IMapper>();
            return mapper.Map<TDestination>(Source);
        }
        public static TDestination Map<TSource, TDestination>(TSource Source)
        {
            var mapper = ServiceProvider.GetRequiredService<IMapper>();
            return mapper.Map<TSource, TDestination>(Source);

        }
        public static TDestination MapTo<TDestination>(this object Source)
        {
            var mapper = ServiceProvider.GetRequiredService<IMapper>();
            return mapper.Map<TDestination>(Source);
        }
        public static TDestination MapTo<TSource, TDestination>(this TSource Source)
        {
            var mapper = ServiceProvider.GetRequiredService<IMapper>();
            return mapper.Map<TSource, TDestination>(Source);

        }
        public static List<TDestination> MapToList<TDestination>(this IEnumerable Source)
        {
            var mapper = ServiceProvider.GetRequiredService<IMapper>();
            return mapper.Map<List<TDestination>>(Source);
        }
        public static List<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> Source)
        {
            var mapper = ServiceProvider.GetRequiredService<IMapper>();
            return mapper.Map<List<TDestination>>(Source);
        }
    }
}
