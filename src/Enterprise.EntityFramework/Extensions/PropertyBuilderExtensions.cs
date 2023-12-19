using System.Drawing;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Enterprise.EntityFramework.Extensions;

public static class PropertyBuilderExtensions
{
    /// <summary>
    /// Use a value converter for the Color struct.
    /// The string value is persisted in the provider, and is converted back to a struct using the persisted name from the provider.
    /// </summary>
    /// <param name="propertyBuilder"></param>
    /// <returns></returns>
    public static PropertyBuilder<Color> ConvertColor(this PropertyBuilder<Color> propertyBuilder)
    {
        Expression<Func<Color, string>> convertToProviderExpression = c => c.ToString();
        Expression<Func<string, Color>> convertFromProviderExpression = s => Color.FromName(s);
        propertyBuilder = propertyBuilder.HasConversion(convertToProviderExpression, convertFromProviderExpression);
        return propertyBuilder;
    }
}