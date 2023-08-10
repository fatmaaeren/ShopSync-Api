namespace ShopSync.Dtos
{
    public class ShoppingListResponseDto
    {
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public long ShoppingListId { get; set; }
        public long ProductId { get; set; }
        public string Description { get; set; }
    }
}
