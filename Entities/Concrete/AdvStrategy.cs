using Core.Entities;

namespace Entities.Concrete;

public class AdvStrategy : IEntity
{
    public AdvStrategy()
    {
        CreatedAt = DateTimeOffset.Now;
        Status = true;
    }

    public long StrategyId { get; set; }
    public string Name { get; set; }
    public float StrategyValue { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public long Id { get; set; }
    public bool Status { get; set; }
}