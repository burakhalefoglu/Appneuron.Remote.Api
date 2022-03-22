using Core.Entities;

namespace Entities.Concrete;

public class InterstitialAdModel : IEntity
{
    public InterstitialAdModel()
    {
        CreatedAt = DateTimeOffset.Now;
        Status = true;
        IsActive = false;
    }

    public string Name { get; set; }
    public long ProjectId { get; set; }
    public string Version { get; set; }
    public int PlayerPercent { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public long Id { get; set; }
    public bool Status { get; set; }
}