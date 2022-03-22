using Core.Entities;

namespace Entities.Concrete;

public class AdvStrategy : IEntity
{
    public AdvStrategy()
    {
        CreatedAt = DateTimeOffset.Now;
        Status = true;
    }

    public string Name { get; set; }
    public float StrategyValue { get; set; }
    public string Version { get; set; }
    public long ProjectId { get; set; }
    public string StrategyName { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public long Id { get; set; }
    public bool Status { get; set; }
}