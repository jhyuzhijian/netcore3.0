using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core_Entity.Entity
{
    [Table("OtherCredit", Schema = "Summary")]
    public class OtherCredit : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Credit { get; set; }
        public DateTime CreditTime { get; set; }
        public int TypeId { get; set; }
        public int BatchId { get; set; }
        /// <summary>
        /// 导入类别ID
        /// </summary>
        public int ImportType { get; set; }
        public OtherCredit()
        {
            CreditTime = DateTime.Now;
            CreateTime = DateTime.Now;
        }
        public DateTime CreateTime { get; set; }

        public TrainOtherCredit TrainOtherCredit { get; set; }

    }
}
