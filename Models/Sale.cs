using System.ComponentModel.DataAnnotations;

namespace WebAPi.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public string Buyer { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Withdraw { get; set; }
        public string? Address { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        [Required]
        public decimal Total { get; set; }
        public ICollection<SaleItem> SaleItems { get; set; }

    }
}
