namespace CafeShop.Models.DTOs
{
    public class OrderDto : Order
    {
       public List<OrderDetail>? Details { get; set; }
       public string? StatusText { get; set; }
       public decimal TotalMoney { get; set; }
        public string? DateFormat { get; set; }
    }
}
