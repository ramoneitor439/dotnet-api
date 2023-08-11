using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SocialNetwork.Models
{
    [Table("post", Schema = "content")]
    public class Post
    {
        [Key]
        [Required]
        [Column("id")]
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("title")]
        [JsonPropertyName("title")]
        public string Title { get; set; } = null!;

        [Required]
        [Column("content")]
        [JsonPropertyName("content")]
        public string Content { get; set; } = null!;

        [Required]
        [ForeignKey("User")]
        [JsonPropertyName("user_id")]
        public Guid UserId { get; set; }
    }
}
