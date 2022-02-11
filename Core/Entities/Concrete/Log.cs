﻿using System;

namespace Core.Entities.Concrete
{
    public class Log : IEntity
    {
        public string MessageTemplate { get; set; }
        public string Level { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public string Exception { get; set; }
        public bool Status = true;
        public long Id { get; set; }
    }
}