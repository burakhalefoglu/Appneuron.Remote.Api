using Microsoft.AspNetCore.Http;

namespace Entities.Dtos;

public class RemoteOfferProductModelDto
{
    public string Name { get; set; }
    public string  Image { get; set; }
    public float Count { get; set; }
    public string ImageName { get; set; }
}