using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace SuperDoc.Application.Converters;

/// <summary>
/// Converts an enum value to a display-friendly string by retrieving the specified property from the associated DisplayAttribute.
/// </summary>
public class DisplayAttributeConverter : IValueConverter
{
    /// <summary>
    /// Converts an enum value to a display-friendly string.
    /// </summary>
    /// <param name="value">The enum value to be converted.</param>
    /// <param name="targetType">The type of the target property (ignored in this implementation).</param>
    /// <param name="parameter">The name of the property to retrieve from the DisplayAttribute (default is "Name").</param>
    /// <param name="culture">The CultureInfo to use in the conversion (ignored in this implementation).</param>
    /// <returns>The value of the specified property from the associated DisplayAttribute, or the original value if not applicable.</returns>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return value;
        }

        // Get the MemberInfo for the enum value, so that we can retrieve the DisplayAttribute.
        MemberInfo? property = value.GetType().GetMember(value.ToString() ?? string.Empty).FirstOrDefault();
        if (property == null)
        {
            return value;
        }

        // Get the DisplayAttribute associated with the enum value.
        DisplayAttribute? attribute = property.GetCustomAttribute<DisplayAttribute>();
        if (attribute == null)
        {
            return value;
        }

        // Get the property name to retrieve from the DisplayAttribute (default is "Name").
        string attributeProperty = parameter as string ?? "Name";

        // Return the value of the specified property from the DisplayAttribute.
        return attribute.GetType().GetProperty(attributeProperty)?.GetValue(attribute, null);
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
