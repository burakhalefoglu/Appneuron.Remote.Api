using Core.Entities;

namespace Entities.Concrete
{
    public class InterstitialAdModel : DocumentDbEntity
    {
        public string Name { get; set; }
        public string ProjectId { get; set; }
        public string Version { get; set; }
        public int PlayerPercent { get; set; }
        public bool IsAdvSettingsActive { get; set; }
        public AdvStrategy[] AdvStrategies { get; set; }
        public bool Status = true;
    }
}