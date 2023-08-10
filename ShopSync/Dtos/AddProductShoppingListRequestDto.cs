using ShopSync.Context;

namespace ShopSync.Dtos
{
    public class AddProductShoppingListRequestDto
    {
        public long ShoppingListId { get; set; }
        public long UserId { get; set; }
        public long ProductId { get; set; }
        public string Description { get; set; }
    }
}
