namespace WebApplication1.Models
{
    public class Product
    {
        public required Guid Name { get; set; }
        public required string Feature { get; set; }
        public required string PublicationDate { get; set; }
        public required string Image { get; set; }
        public required int Price { get; set; }
        public required string ConditionProd { get; set; }
    }
}
