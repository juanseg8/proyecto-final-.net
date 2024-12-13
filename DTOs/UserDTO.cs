using System.ComponentModel.DataAnnotations.Schema;
using WebAPi.Models;

namespace WebAPi.DTOs
{
    public class UserDTO
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
