namespace CafeShop.Models.DTOs
{
    public class ProductSizeDto : ProductSize
    {
        public int Price { get; set; }
        public int ProductDetailsId { get; set; }
    }
}
