using System;
using System.Collections.Generic;
using System.Text;

namespace Core_Entity.Entity
{
    public class Relation : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public RelationExtend RelationExtend { get; set; }
    }
}
