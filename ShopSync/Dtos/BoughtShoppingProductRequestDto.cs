using ShopSync.Context;

namespace ShopSync.Dtos
{
    public class BoughtShoppingProductRequestDto
    {
        public long ShoppingListId{ get; set; }
        public long ProductId { get; set; }
        public long UserId { get; set; }
    }
}
