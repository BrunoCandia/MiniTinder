using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;
// using System.ComponentModel.DataAnnotations.Schema; (not needed - configure FK via Fluent API)

namespace API.Entities
{
    public class Member
    {
        public Guid Id { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? ImageUrl { get; set; }
        public required string DisplayName { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public required string Gender { get; set; }
        public string? Description { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }

        // Navigation properties
        //[ForeignKey(nameof(Id))]
        public User User { get; set; } = null!;

        public List<Photo> Photos { get; set; } = new();
    }

    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
                // Configure one-to-one relationship: Member.Id is both PK and FK to User.Id
                builder.HasOne(m => m.User)
                       .WithOne(u => u.Member)
                       .HasForeignKey<Member>(m => m.Id);
        }
    }
}
