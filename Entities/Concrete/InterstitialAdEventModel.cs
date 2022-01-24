using System.Collections.Generic;
using Core.Entities;

namespace Entities.Concrete
{
    public class InterstitialAdEventModel : DocumentDbEntity
    {
        public string ProjectId { get; set; }
        public string[] ClientIdList { get; set; }
        public bool IsAdvSettingsActive { get; set; }
        public Dictionary<string, int> AdvFrequencyStrategies { get; set; }
    }
}