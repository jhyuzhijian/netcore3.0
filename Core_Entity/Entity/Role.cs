using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core_Entity.Entity
{
    [Table(name: "Role", Schema = "Right")]
    public class Role : IEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Display(Name = "编号")]
        public int Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [Display(Name = "角色名称")]
        public string Name { get; set; }


        /// <summary>
        /// 角色类型
        /// </summary>
        [Display(Name = "类型")]
        public int TypeId { get; set; }


        /// <summary>
        /// 角色描述
        /// </summary>
        [Display(Name = "描述")]
        public string Description { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        [Display(Name = "显示顺序")]
        public int Sort { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Display(Name = "状态")]
        public string Status { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Display(Name = "编码")]
        public string Code { get; set; }

        /// <summary>
        /// 权级
        /// </summary>
        [Display(Name = "权级")]

        public int Level { get; set; }
    }
}
