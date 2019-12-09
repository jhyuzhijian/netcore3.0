using System;
using System.Collections.Generic;
using System.Text;

namespace Core_Entity.Entity
{
    public class RelationExtend : IEntity
    {
        public int Id { get; set; }
        public int? Sex { get; set; }
        public string Address { get; set; }
        public int RelationId { get; set; }
        public Relation Relation { get; set; }
    }
}
