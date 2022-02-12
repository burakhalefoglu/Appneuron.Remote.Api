using Core.Entities;

namespace Entities.Concrete
{
    public class RemoteOfferProductModel: IEntity
    {
        public string RemoteOfferName { get; set; }
        public string Version { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public float Count { get; set; }
        public string ImageName { get; set; }
        
        public bool Status = true;
        public long Id { get; set; }
    }
}