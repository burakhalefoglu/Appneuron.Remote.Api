using Core.Entities;
using System;


namespace Entities.Concrete
{
    public class RemoteOfferModel : DocumentDbEntity
    {
        public ProductModel[] ProductModelDtos { get; set; }
        public float FirstPrice { get; set; }
        public float LastPrice { get; set; }
        public int OfferId { get; set; }
        public bool IsGift { get; set; }
        public bool IsActive { get; set; }
        public byte[] GiftTexture { get; set; }
        public int ValidityPeriod { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
    }
}
