using Core.Entities;

namespace Entities.Concrete
{
    public class ProductModel: IEntity
    {
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public float Count { get; set; }
        public string ImageName { get; set; }
        public bool Status = true;
        public long Id { get; set; }
    }
}