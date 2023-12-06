using System.ComponentModel.DataAnnotations;
using System.Globalization;

using Syncfusion.Licensing;

namespace SuperDoc.Application.Converters;

/// <summary>
/// Converts an enum value to a display-friendly string by retrieving the name from the associated DisplayAttribute.
/// </summary>
public class DisplayAttributeConverter : IValueConverter
{
    /// <summary>
    /// Converts an enum value to a display-friendly string.
    /// </summary>
    /// <param name="value">The enum value to be converted.</param>
    /// <param name="targetType">The type of the target property (ignored in this implementation).</param>
    /// <param name="parameter">The name of the property to retrieve from the DisplayAttribute (ignored in this implementation).</param>
    /// <param name="culture">The CultureInfo to use in the conversion (ignored in this implementation).</param>
    /// <returns>The value of the specified property from the associated DisplayAttribute, or the original value if not applicable.</returns>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || !value.GetType().IsEnum)
        {
            return value;
        }

        return ((Enum)value).GetAttributeOfType<DisplayAttribute>().Name;
    }

    /// <summary>
    /// Converts back a display-friendly string to the original enum value (not implemented).
    /// </summary>
    /// <returns>Throws a <see cref="NotImplementedException"/> since conversion back is not implemented.</returns>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
