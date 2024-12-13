namespace WebAPi.DTOs
{
    public class SaleItemDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotal { get; set; }
    }
}
