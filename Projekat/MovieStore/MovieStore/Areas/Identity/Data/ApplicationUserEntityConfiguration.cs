using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MovieStore.Areas.Identity.Data
{
    public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(user => user.FirstName).HasMaxLength(50);
            builder.Property(user => user.LastName).HasMaxLength(50);     
        }
    }
}