using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SocialNetwork.Models
{
    [Table("user", Schema = "security")]
    public class User
    {
        [Key]
        [Required]
        [Column("id")]
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [Required]
        [Column("username")]
        [JsonPropertyName("username")]
        public string UserName { get; set; } = null!;
        [Required]
        [Column("email")]
        [JsonPropertyName("email")]
        public string Email { get; set; } = null!;
        [Required]
        [Column("password")]
        [JsonPropertyName("password")]
        public string Password { get; set; } = null!;

        [Required]
        [Column("role")]
        [ForeignKey("Role")]
        [JsonPropertyName("role_id")]
        public int RoleId { get; set; }

        public bool CheckPassword(string password)
            => password == Password;
    }
}
