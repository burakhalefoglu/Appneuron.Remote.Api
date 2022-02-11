using System;
using Core.Entities;

namespace Entities.Concrete
{
    public class AdvStrategy : IEntity
    {
        public string Name { get; set; }
        public float Count { get; set; }
        public bool Status = true;
        public long Id { get; set; }
    }
}
