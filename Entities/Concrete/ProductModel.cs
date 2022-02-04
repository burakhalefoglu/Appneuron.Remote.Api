namespace Entities.Concrete
{
    public class ProductModel
    {
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public float Count { get; set; }
        public string ImageName { get; set; }
        public bool Status = true;
    }
}