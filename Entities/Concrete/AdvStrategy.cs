using System;
using Core.Entities;

namespace Entities.Concrete
{
    public class AdvStrategy : DocumentDbEntity
    {
        public string Name { get; set; }
        public float Count { get; set; }
    }
}
