using ShopSync.Context;

namespace ShopSync.Dtos
{
    public class ProductFilterDto
    {
        public string? GeneralSearch { get; set; }
        public long? CategoryId{ get; set; }
        public int Cursor { get; set; }
    }
}
