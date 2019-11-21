using NLog;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Infrastructure.Tool.Util
{
    public enum LogType
    {
        [Description("网站")]
        Web,
        [Description("数据库")]
        DataBase,
        [Description("Api接口")]
        ApiRequest,
        [Description("中间件")]
        Middleware
    }
    public static class NLogUtil
    {
        public static Logger dbLogger = LogManager.GetLogger("logdb");
        public static Logger fileLogger = LogManager.GetLogger("logallfile");
        /// <summary>
        /// 写日志到数据库
        /// </summary>
        /// <param name="logLevel">日志等级</param>
        /// <param name="logTyzpe">日志类型</param>
        /// <param name="message">信息</param>
        /// <param name="exception">异常</param>
        public static void WriteDBLog(LogLevel logLevel, LogType logType, string message, Exception exception = null)
        {
            LogEventInfo theEvent = new LogEventInfo(logLevel, dbLogger.Name, message);
            theEvent.Properties["LogType"] = dbLogger.ToString();
            theEvent.Exception = exception;
            dbLogger.Log(theEvent);
        }
        /// <summary>
        /// 写日志到文件
        /// </summary>
        /// <param name="logLevel">日志等级</param>
        /// <param name="logType">日志类型</param>
        /// <param name="message">信息</param>
        /// <param name="exception">异常</param>
        public static void WriteFileLog(LogLevel logLevel, LogType logType, string message, Exception exception = null)
        {
            LogEventInfo theEvent = new LogEventInfo(logLevel, fileLogger.Name, message);
            theEvent.Properties["LogType"] = fileLogger.ToString();
            theEvent.Exception = exception;
            fileLogger.Log(theEvent);
        }
        public static void EnsureNlogConfig(string nlogPath, string sqlConnectionStr)
        {
            XDocument xmlDoc = XDocument.Load(nlogPath);
            if (xmlDoc.Root.Elements().FirstOrDefault(x => x.Name.LocalName == "targets")
                is XElement targetsNode && targetsNode != null &&
                targetsNode.Elements().FirstOrDefault(x => x.Name.LocalName == "target" && x.Attribute("name").Value == "database")
                is XElement targetNode && targetNode != null
                )
            {
                //判断NLog配置文件中的数据库连接和appsetting中是否一致，不一致则取appsetting中的连接字符串
                if (!targetNode.Attribute("connectionString").Value.Equals(sqlConnectionStr))
                {
                    targetNode.Attribute("connectionString").Value = sqlConnectionStr;
                    xmlDoc.Save(nlogPath);
                    //重新加载配置
                    LogManager.Configuration = new XmlLoggingConfiguration(nlogPath);
                }
            }
        }
    }
}
