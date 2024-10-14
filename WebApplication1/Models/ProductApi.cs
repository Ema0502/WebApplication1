namespace WebApplication1.Models
{
    public class ProductApi
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required decimal Price { get; set; }
        public required string Description { get; set; }
        public required string Image { get; set; }
    }
}
