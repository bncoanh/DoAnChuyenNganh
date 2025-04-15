namespace CafeShop.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public int? ProductDetailId { get; set; }
        public int? Quantity { get; set; }
    }
}
