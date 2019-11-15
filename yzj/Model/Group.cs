using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace yzj.Model
{
    public class Group
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [Display(Name = "单位名称")]
        public string Name { get; set; }

        /// <summary>
        /// 父编号
        /// </summary>
        [Display(Name = "上级单位")]
        public int? ParentId { get; set; }

        /// <summary>
        /// 是否参加排行
        /// </summary>
        [Display(Name = "是否参加排行")]
        public bool? RankFlag { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Display(Name = "状态")]
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
