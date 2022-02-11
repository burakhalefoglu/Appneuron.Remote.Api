﻿using System;

namespace Core.Entities.Dtos
{
    public class LogDto : IEntity
    {
        public string Level { get; set; }
        public string ExceptionMessage { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public string User { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public bool Status = true;
        public long Id { get; set; }
    }
}