using System;
using Core.Entities;

namespace Entities.Concrete
{
    public class InterstitialAdHistoryModel : IEntity
    {
        public string Name { get; set; }
        public long ProjectId { get; set; }
        public string Version { get; set; }
        public int PlayerPercent { get; set; }
        public bool IsAdvSettingsActive { get; set; }
        public DateTime StarTime { get; set; }
        public bool Status = true;
        public long Id { get; set; }
    }
}