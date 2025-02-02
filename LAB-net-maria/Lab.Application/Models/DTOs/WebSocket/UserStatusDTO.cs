using Lab.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Lab.Application.Models.DTOs.WebSocket
{
    public class UserStatusDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public UserStatus Status { get; set; }
    }
}
