namespace CafeShop.Models.DTOs
{
    public class CartDto
    {
        public int CartId { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; set; }
        public string? PriceFormat { get; set; }
        public int? ProductDetailsId { get; set; }
        public string? ProductName { get; set; }
        public string? SizeName { get; set; }
        public string? ImageUrl { get; set; }
    }
}
