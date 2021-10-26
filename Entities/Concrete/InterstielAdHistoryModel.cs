using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;

namespace Entities.Concrete
{
    public class InterstielAdHistoryModel: DocumentDbEntity
    {
        public string Name { get; set; }
        public string ProjectId { get; set; }
        public float Version { get; set; }
        public int playerPercent { get; set; }
        public bool IsAdvSettingsActive { get; set; }
        public AdvStrategy[] AdvStrategies { get; set; }
        public DateTime StarTime { get; set; }      
    }
}
