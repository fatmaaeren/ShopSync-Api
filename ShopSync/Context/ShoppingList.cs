namespace ShopSync.Context
{
    public class ShoppingList
    {
        public long Id { get; set; }
        public long ShoppingListId { get; set; }
        public string Name { get; set; }
        public long UserId { get; set; }
        public virtual User? User { get; set; }
        public long? ProductId { get; set; }
        public virtual Products? Products { get; set; }
        public string? Description { get; set; }
        public bool IsBought { get; set; }
        public bool IsGoShopping { get; set; }


    }
}
