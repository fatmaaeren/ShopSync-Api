using ShopSync.Context;

namespace ShopSync.Dtos
{
    public class ShoppingListProductDescriptionChangeRequestDto
    {
        public string Description { get; set; }
        public long UserId { get; set; }
        public long ShoppingListId { get; set; }
        public long ProductId { get; set; }
    }
}
