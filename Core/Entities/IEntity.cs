namespace Core.Entities;

public interface IEntity
{
    public long Id { get; set; }
    public bool Status { get; set; }
}