using Core.Entities;
using Entities.Concrete;

namespace Entities.Dtos
{
    public class InterstitialAdModelDto : IDto
    {
        public string Name { get; set; }
        public long ProjectId { get; set; }
        public string Version { get; set; }
        public int PlayerPercent { get; set; }
        public AdvStrategy[] AdvStrategies { get; set; }
        public long Id { get; set; }
        public bool Status { get; set; }
    }
}