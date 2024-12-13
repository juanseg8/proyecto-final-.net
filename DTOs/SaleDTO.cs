namespace WebAPi.DTOs
{
    public class SaleDTO
    {
        public string Buyer { get; set; }
        public DateTime Date { get; set; }
        public string Withdraw { get; set; }
        public string? Address { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Total { get; set; }
        public List<SaleItemDTO> SaleItems { get; set; }
    }
}
