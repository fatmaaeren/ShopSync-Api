namespace ShopSync.Context
{
    public class Products
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public string Image { get; set; }
    }
}
