using System.Collections.Generic;
using Core.Entities;

namespace Entities.Concrete
{
    public class InterstitialAdEventModel : IEntity
    {
        public long ProjectId { get; set; }
        public string[] ClientIdList { get; set; }
        public bool IsAdvSettingsActive { get; set; }
        public Dictionary<string, int> AdvFrequencyStrategies { get; set; }
        public bool Status = true;
        public long Id { get; set; }
    }
}