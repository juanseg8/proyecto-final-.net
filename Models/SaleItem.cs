using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAPi.Models
{
    public class SaleItem
    {
        public int Id { get; set; }

        [Required]
        public int SaleId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public decimal Subtotal { get; set; }

        [JsonIgnore]
        public Sale Sale { get; set; }
    }
}
