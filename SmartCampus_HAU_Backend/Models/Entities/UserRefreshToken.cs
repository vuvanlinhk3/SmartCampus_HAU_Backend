using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartCampus_HAU_Backend.Models.Entities
{
    [Table("user_refresh_tokens")]
    public class UserRefreshToken
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        [Required]
        public string UserId { get; set; } = null!;

        [Column("token")]
        [Required]
        public string Token { get; set; } = null!;

        [Column("expiry_date")]
        public DateTime ExpiryDate { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}
