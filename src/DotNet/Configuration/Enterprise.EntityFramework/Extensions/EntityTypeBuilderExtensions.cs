using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enterprise.EntityFramework.Extensions;

public static class EntityTypeBuilderExtensions
{
    public static EntityTypeBuilder AddAuditShadowProperties(this EntityTypeBuilder entityTypeBuilder)
    {
        PropertyBuilder<DateTime> dateCreatedPropertyBuilder = entityTypeBuilder.Property<DateTime>("DateCreated");
        //dateCreatedPropertyBuilder.HasDefaultValue(); // specifying Datetime.UtcNow updates the seed value for migrations every time...
        PropertyBuilder<DateTime?> dateModifiedPropertyBuilder = entityTypeBuilder.Property<DateTime?>("DateModified");
        return entityTypeBuilder;
    }
}