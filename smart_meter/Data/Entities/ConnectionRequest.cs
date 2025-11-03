using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smart_meter.Data.Entities
{
    [Table("connection_request")]
    public class ConnectionRequest
    {
        [Key]
        [Column("request_id")]
        public long RequestId { get; set; }

        // Nullable for new connection requests (no consumer yet)
        [Column("consumer_id")]
        public long? ConsumerId { get; set; }

        [Column("name")]
        [StringLength(200)]
        public string? Name { get; set; }

        [Column("email")]
        [StringLength(200)]
        public string? Email { get; set; }

        [Column("address")]
        [StringLength(500)]
        public string? Address { get; set; }

        [Column("request_type")]
        [StringLength(50)]
        [Required]
        public string RequestType { get; set; } = null!;
        // Allowed values: "new_connection", "connect", "disconnect"

        [Column("status")]
        [StringLength(50)]
        public string Status { get; set; } = "pending";
        // Allowed values: "pending", "approved", "rejected", "completed"

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("approved_at")]
        public DateTime? ApprovedAt { get; set; }

        [Column("remarks")]
        public string? Remarks { get; set; }

        // ActionBy is now a foreign key to User table
        [Column("action_by")]
        public long? ActionBy { get; set; }

        // Relationships (Navigation Properties)
        [ForeignKey("ConsumerId")]
        public virtual Consumer? Consumer { get; set; }

        [ForeignKey("ActionBy")]
        public virtual User? ActionByUser { get; set; }
    }
}
