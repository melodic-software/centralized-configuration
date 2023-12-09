using System.Drawing;
using Enterprise.EntityFramework.ValueConverters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enterprise.EntityFramework.Extensions;

public static class PropertiesConfigurationBuilderExtensions
{
    public static PropertiesConfigurationBuilder<Color> HaveConversion(this PropertiesConfigurationBuilder<Color> configurationBuilder)
    {
        return configurationBuilder.HaveConversion(typeof(ColorValueConverter));
    }
}