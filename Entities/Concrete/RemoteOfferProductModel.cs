using Core.Entities;

namespace Entities.Concrete;

public class RemoteOfferProductModel : IEntity
{
    public RemoteOfferProductModel()
    {
        CreatedAt = DateTimeOffset.Now;
        Status = true;
    }

    public long StrategyId { get; set; }
    public string Name { get; set; }
    public byte[] Image { get; set; }
    public float Count { get; set; }
    public string ImageName { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public long Id { get; set; }
    public bool Status { get; set; }
}