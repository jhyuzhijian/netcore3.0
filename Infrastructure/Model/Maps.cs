﻿using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Model
{
    //AutoMapper9.0已经不能用静态api注册,且Automapper.DI已经实现注入到默认DI的代码
    public static class AutoMapperExtension
    {
        //public static IServiceCollection AddAutoMapper(this IServiceCollection service)
        //{
        //    service.TryAddSingleton<MapperConfigurationExpression>();
        //    service.TryAddSingleton(serviceProvider =>
        //    {
        //        var mapperConfigurationExpression = serviceProvider.GetRequiredService<MapperConfigurationExpression>();
        //        var instance = new MapperConfiguration(mapperConfigurationExpression);

        //        instance.AssertConfigurationIsValid();//检测映射错误
        //        return instance;
        //    });
        //    service.TryAddSingleton(serviceProvider =>
        //    {
        //        var mapperConfiguration = serviceProvider.GetRequiredService<MapperConfiguration>();

        //        return mapperConfiguration.CreateMapper();

        //    });
        //    return service;
        //}
        //public static IMapperConfigurationExpression UseAutoMapper(this IApplicationBuilder applicationBuilder)
        //{
        //    var expression = applicationBuilder.ApplicationServices.GetRequiredService<MapperConfigurationExpression>();
        //    return expression;
        //}
    }
}
