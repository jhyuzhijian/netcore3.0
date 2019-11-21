using System;
using System.Collections.Generic;
using System.Text;

namespace Core_Entity.Dto
{
   public class Dto_Group
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 父编号
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// 是否参加排行
        /// </summary>
        public bool? RankFlag { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 子部门数量
        /// </summary>
        public int ChildCount { get; set; }

        /// <summary>
        /// 单位人数
        /// </summary>
        public int UserCount { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; set; }
    }
}
