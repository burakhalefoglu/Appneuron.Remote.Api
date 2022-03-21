using Core.Entities;

namespace Entities.Concrete
{
    public class InterstitialAdModel : IEntity
    {
        public InterstitialAdModel() 
        {
            CreatedAt = DateTimeOffset.Now;
            Status = true;
        }
        
        public string Name { get; set; }
        public long ProjectId { get; set; }
        public string Version { get; set; }
        public int PlayerPercent { get; set; }
        public long Id { get; set; }
        public bool Status { get; set; }
        public bool Terminated { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

    }
}