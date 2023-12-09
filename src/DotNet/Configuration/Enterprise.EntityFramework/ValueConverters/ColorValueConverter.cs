using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Drawing;
using System.Linq.Expressions;

namespace Enterprise.EntityFramework.ValueConverters;

public class ColorValueConverter : ValueConverter<Color, string>
{
    public ColorValueConverter() : base(ColorToString, StringToColor)
    {

    }

    private static readonly Expression<Func<Color, string>> ColorToString = v => v.Name.ToString();
    private static readonly Expression<Func<string, Color>> StringToColor = v => Color.FromName(v);
}