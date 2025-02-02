using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;


namespace Lab.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; } 

     /*   public string FullName
        {
            get => $"{FirstName ?? string.Empty} {LastName ?? string.Empty}".Trim(); 
        }*/
        public short CurrrentTimeZone { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; } = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

        [Column(TypeName = "timestamp")]
        public DateTime UpdatedAt { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? ArchivedAt { get; set; }
        public string? Issuer { get; set; }= "https://securetoken.google.com/dotnet-lab-eb6af";
        public UserStatus Status { get; set; }
    }

    public enum UserStatus
    {
        Active,
        Pending,
        Rejective
    }
    public enum UserActionEnum
    {
        Read = 0,
        Write = 1,
        Login = 2,
        Register = 3
    }
}