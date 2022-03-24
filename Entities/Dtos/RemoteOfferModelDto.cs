﻿using Core.Entities;
using Entities.Concrete;

namespace Entities.Dtos;

public class RemoteOfferModelDto : IDto
{
    public RemoteOfferModelDto()
    {
        RemoteOfferProductModels = new List<RemoteOfferProductModelDto>();
    }
    public long ProjectId { get; set; }
    public string Name { get; set; }
    public float FirstPrice { get; set; }
    public float LastPrice { get; set; }
    public string Version { get; set; }
    public int PlayerPercent { get; set; }
    public bool IsGift { get; set; }
    public string GiftTexture { get; set; }
    public int ValidityPeriod { get; set; }
    public long StartTime { get; set; }
    public long FinishTime { get; set; }
    public bool IsActive { get; set; }
    public long Id { get; set; }
    public List<RemoteOfferProductModelDto> RemoteOfferProductModels { get; set; }
}