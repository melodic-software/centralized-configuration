using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Configuration.EntityFramework.Entities.Configurations
{
    public class ApplicationConfiguration : IEntityTypeConfiguration<ApplicationEntity>
    {
        public void Configure(EntityTypeBuilder<ApplicationEntity> builder)
        {
            // This is the fluent approach that is an alternative to using attributes on classes.

            builder.ToTable("Application");
        }
    }
}
