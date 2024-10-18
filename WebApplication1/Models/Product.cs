namespace WebApplication1.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Feature { get; set; }
        public required DateTime PublicationDate { get; set; }
        public required string Image { get; set; }
        public required decimal Price { get; set; }
        public required string ConditionProd { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
