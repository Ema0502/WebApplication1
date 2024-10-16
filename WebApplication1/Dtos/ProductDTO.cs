namespace WebApplication1.Dtos
{
    public class ProductDTO
    {

        public required string Name { get; set; }
        public required string Feature { get; set; }
        public required DateTime PublicationDate { get; set; }
        public required string Image { get; set; }
        public required decimal Price { get; set; }
        public required string ConditionProd { get; set; }
    }
}
