using System.ComponentModel.DataAnnotations;

namespace Lab.Application.Models.DTOs.Secure
{
    public class TokenDto
    {
        [Required]
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; } = DateTime.Now.AddDays(1);
    }
}
