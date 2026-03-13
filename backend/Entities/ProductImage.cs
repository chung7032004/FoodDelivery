namespace FoodDelivery.Entities
{
    public class ProductImage
    {
        public Guid Id {get; set;} = Guid.NewGuid();
        public Guid ProductId { get; set;}
        public virtual Product Product { get; set; } = null!;
        public string ImageUrl {get; set;} = string.Empty;
        public bool IsMain {get; set;} = false;
        public int DisplayOrder { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}