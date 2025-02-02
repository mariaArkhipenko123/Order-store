using System.ComponentModel.DataAnnotations;

namespace Lab.Application.Models.DTOs.Secure
{
    public class UserModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName
        {
            get => $"{FirstName} {LastName}";
        }
        [Required]
        public string Email { get; set; }
        public short CurrrentTimeZone { get; set; }
    }
}
