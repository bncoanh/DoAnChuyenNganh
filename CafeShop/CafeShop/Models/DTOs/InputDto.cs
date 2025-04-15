namespace CafeShop.Models.DTOs
{
    public class InputDto
    {
        public string? request { get; set; } = "";
        public int? pageNumber { get; set; } = 1;
        public DateTime? dateStart{ get; set; } = DateTime.Now;
        public DateTime? dateEnd { get; set; } = DateTime.Now;
        public int? status { get; set; } = -1;
        
    }
}
