using System;
using Core.Entities;

namespace Entities.Concrete
{
    public class InterstielAdHistoryModel : DocumentDbEntity
    {
        public string Name { get; set; }
        public string ProjectId { get; set; }
        public string Version { get; set; }
        public int playerPercent { get; set; }
        public bool IsAdvSettingsActive { get; set; }
        public DateTime StarTime { get; set; }
        public bool Status = true;
    }
}