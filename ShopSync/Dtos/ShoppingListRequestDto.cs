using ShopSync.Context;

namespace ShopSync.Dtos
{
    public class ShoppingListRequestDto
    {
        public string Name{ get; set; }
        public long UserId { get; set; }
    }
}
