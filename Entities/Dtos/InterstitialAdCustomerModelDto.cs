using Core.Entities;
using Entities.Concrete;

namespace Entities.Dtos;

public class InterstitialAdCustomerModelDto : IDto
{
    public InterstitialAdCustomerModelDto()
    {
        AdvStrategies = new List<AdvStrategy>();
    }
    public string Name { get; set; }
    public long ProjectId { get; set; }
    public string Version { get; set; }
    public int PlayerPercent { get; set; }
    public List<AdvStrategy> AdvStrategies { get; set; }
    public long Id { get; set; }
    public bool IsActive { get; set; }
}