using ShopSync.Context;

namespace ShopSync.Dtos
{
    public class ProductRequestDto
    {
        public long Id{ get; set; }
        public string Name { get; set; }
        public long CategoryId { get; set; }
        public string Image { get; set; }

    }
}
