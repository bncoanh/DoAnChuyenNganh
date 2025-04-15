namespace CafeShop.Models.DTOs
{
    public class OrderDetailsDto
    {
        public string? ProductName { get; set; }
        public string? SizeName { get; set; }
        public int TotalMoney { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public string? ImageUrl { get; set; }
    }
}
