using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebAPi.Models
{
    public class Users
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [NotMapped]
        [Required]
        public string Password { get; set; }

        public string PasswordHash { get; set; }
        [Required]
        public int RoleId { get; set; }

        [JsonIgnore]
        public Role Role { get; set; }
    }
}
