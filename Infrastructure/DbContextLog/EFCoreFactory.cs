using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DbContextLog
{
    //public class DbLog
    //{
    //    public static readonly Microsoft.Extensions.Logging.ILoggerFactory MyLoggerFactory
    //    = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
    //    {
    //        builder
    //            .AddFilter((category, level) =>
    //                category == DbLoggerCategory.Database.Command.Name
    //                && level == LogLevel.Information)
    //            .AddConsole()
    //            .AddDebug()
    //            ;
    //    });
    //}
    public class EFCoreFactory : ILoggerFactory
    {
        public EFCoreFactory()
        {

        }
        public void AddProvider(ILoggerProvider provider)
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new EFCoreLogger(categoryName);
        }
        public void Dispose() { }
        //#region IDisposable Support
        //private bool disposedValue = false; // 要检测冗余调用

        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!disposedValue)
        //    {
        //        if (disposing)
        //        {
        //            // TODO: 释放托管状态(托管对象)。
        //        }

        //        // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
        //        // TODO: 将大型字段设置为 null。

        //        disposedValue = true;
        //    }
        //}

        //// 添加此代码以正确实现可处置模式。
        //public void Dispose()
        //{
        //    // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //    Dispose(true);
        //    // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
        //    // GC.SuppressFinalize(this);
        //}
        //#endregion
    }

    public class EFCoreLogger : ILogger
    {
        private readonly string _categoryName;
        public EFCoreLogger(string categoryName) => this._categoryName = categoryName;

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            //EFCore查询日志级别为info
            if (_categoryName == DbLoggerCategory.Database.Command.Name && logLevel == LogLevel.Information)
            {
                var logContent = formatter(state, exception);
                Tool.Util.NLogUtil.WriteDBLog(NLog.LogLevel.Info, Tool.Util.LogType.DataBase, logContent, exception);
            }
        }
    }
}
