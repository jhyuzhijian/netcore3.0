using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core_Entity.Entity
{
    [Table("Train_OtherCredit", Schema = "Summary")]
    public class TrainOtherCredit : IEntity
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// OtherCredit表主键ID
        /// </summary>
        public int OtherCreditId { get; set; }
        //[ForeignKey("OtherCreditId")]
        public OtherCredit OtherCredit { get; set; }
        /// <summary>
        /// 职称
        /// </summary>
        public string PersonTitle { get; set; }
        /// <summary>
        /// 工作单位
        /// </summary>
        public string WorkUnit { get; set; }
        /// <summary>
        /// 办班开始时间
        /// </summary>
        public DateTime? BeginTime { get; set; }
        /// <summary>
        /// 办班结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 获得时间
        /// </summary>
        public DateTime? AcquiredTime { get; set; }
        /// <summary>
        /// 培训班名称
        /// </summary>
        public string TrainName { get; set; }
        /// <summary>
        /// 办班单位
        /// </summary>
        public string ClassUnit { get; set; }
    }
}
