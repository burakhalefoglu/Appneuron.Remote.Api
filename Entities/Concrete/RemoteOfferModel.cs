using Core.Entities;

namespace Entities.Concrete;

public class RemoteOfferModel : IEntity
    {
        public RemoteOfferModel()
        {
            CreatedAt = DateTimeOffset.Now;
            Status = true;
        }

        public long Id { get; set; }
        public long ProjectId { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public int PlayerPercent { get; set; }
        public float FirstPrice { get; set; }
        public float LastPrice { get; set; }
        public bool IsGift { get; set; }
        public byte[] GiftTexture { get; set; }
        public int ValidityPeriod { get; set; }
        public long StartTime { get; set; }
        public long FinishTime { get; set; }
        public bool Status { get; set; }
        public bool Terminated { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }