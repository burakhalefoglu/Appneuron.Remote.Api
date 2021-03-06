using Core.Entities;

namespace Entities.Concrete;

public class RemoteOfferModel : IEntity
{
    public RemoteOfferModel()
    {
        CreatedAt = DateTimeOffset.Now;
        Status = true;
        IsActive = false;
    }

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
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public long Id { get; set; }
    public bool Status { get; set; }
}