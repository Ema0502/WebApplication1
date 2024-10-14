using WebApplication1.Models;

namespace WebApplication1.Entities
{
    public class ProductEntity
    {
        public required Guid Name { get; set; }
        public required string Feature { get; set; }
        public required string PublicationDate { get; set; }
        public required string Image { get; set; }
        public required decimal Price { get; set; }
        public required string ConditionProd { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
