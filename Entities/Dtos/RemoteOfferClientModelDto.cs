namespace Entities.Dtos;

public class RemoteOfferClientModelDto
{
    public RemoteOfferClientModelDto()
    {
        RemoteOfferProductClientModelDtos = new List<RemoteOfferProductClientModelDto>();
    }
    public long ProjectId { get; set; }
    public float FirstPrice { get; set; }
    public float LastPrice { get; set; }
    public int PlayerPercent { get; set; }
    public bool IsGift { get; set; }
    public byte[] GiftTexture { get; set; }
    public int ValidityPeriod { get; set; }
    public long StartTime { get; set; }
    public long FinishTime { get; set; }
    public List<RemoteOfferProductClientModelDto> RemoteOfferProductClientModelDtos { get; set; }
}