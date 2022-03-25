using Entities.Concrete;

namespace Entities.Dtos;

public class InterstitialAdClientModelDto
{
    public InterstitialAdClientModelDto()
    {
        AdvStrategyClientDto = new List<AdvStrategyClientDto>();
    }
    public long ProjectId { get; set; }
    public int PlayerPercent { get; set; }
    public List<AdvStrategyClientDto> AdvStrategyClientDto { get; set; }
}