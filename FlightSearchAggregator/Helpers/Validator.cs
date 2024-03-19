using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace FlightSearchAggregator.Helpers;

/// <summary>
/// Provides utility methods for validating fields.
/// </summary>
public class Validator : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (IsNullOrZero(value)) return false;

        return true;
    }

    private bool IsNullOrZero(object value)
    {
        if (value == null)
        {
            return true;
        }

        if (value is decimal decimalValue)
        {
            return decimalValue == 0;
        }

        if (value is int intValue)
        {
            return intValue == 0;
        }

        if (value is bool boolValue)
        {
            return !boolValue;
        }

        if (value is Guid guidValue)
        {
            return guidValue == Guid.Empty;
        }

        return false;
    }

    /// <summary>
    /// Validates a string value and adds a model error if it's null or empty.
    /// </summary>
    /// <param name="value">The decimal value to validate.</param>
    /// <param name="fieldName">The name of the field being validated.</param>
    /// <returns>True if the value is valid; otherwise, false.</returns>
    public static void ValidateString(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{fieldName} cannot be null or empty", fieldName);
        }
    }

    /// <summary>
    /// Validates a string decimal value and throws an exception if it's invalid.
    /// </summary>
    /// <param name="decimalValue">The string decimal value to validate.</param>
    /// <param name="fieldName">The name of the field being validated.</param>
    /// <exception cref="ArgumentException">Thrown when the value is invalid (zero).</exception>
    public static void ValidateDecimalString(string decimalValue, string fieldName)
    {
        if (!decimal.TryParse(decimalValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal parsedValue))
        {
            throw new ArgumentException($"{fieldName} cannot be null or empty", fieldName);
        }

        ValidateDecimal(parsedValue, fieldName);
    }

    /// <summary>
    /// Validates a decimal value and throws an exception if it's invalid.
    /// </summary>
    /// <param name="value">The decimal value to validate.</param>
    /// <param name="fieldName">The name of the field being validated.</param>
    /// <exception cref="ArgumentException">Thrown when the value is invalid (zero).</exception>
    public static void ValidateDecimal(decimal value, string fieldName)
    {
        if (value == 0)
        {
            throw new ArgumentException($"Invalid {fieldName}cannot be zero", fieldName);
        }
    }
}
