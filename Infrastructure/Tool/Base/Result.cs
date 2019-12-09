using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Tool.Base
{
    /// <summary>
    /// Request包装返回Result
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 返回冗余的装箱对象
        /// </summary>
        [JsonProperty]
        public dynamic @object { get; set; }

        /// <summary>
        /// 1：成功 0：失败
        /// </summary>
        [JsonProperty]
        public Int32 code { get; set; }

        /// <summary>
        /// HTTP错误状态码
        /// </summary>
        [JsonProperty]
        public string errorCode { get; set; }

        /// <summary>
        /// 扩展参数
        /// </summary>
        [JsonProperty]
        public dynamic extendObject { get; set; }
        /// <summary>
        /// 分页信息
        /// </summary>
        [JsonProperty]
        public Page page { get; set; }
    }
}
