using ShopSync.Context;

namespace ShopSync.Dtos
{
    public class IGoShoppingRequestDto
    {
        public long UserId { get; set; }
        public long ShoppingListId { get; set; }
    }
}
