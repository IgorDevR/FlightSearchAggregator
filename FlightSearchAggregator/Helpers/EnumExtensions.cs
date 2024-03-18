using System.ComponentModel;

namespace FlightSearchAggregator.Helpers
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum enumValue)
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            if (field == null)
                return enumValue.ToString();

            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                return attribute.Description;
            }

            return enumValue.ToString();
        }
        public static TEnum ParseEnum<TEnum>(string value, TEnum defaultValue) where TEnum : struct
        {
            var normalizedValue = value.Replace(" ", "").ToLowerInvariant();
            if (Enum.TryParse<TEnum>(normalizedValue, true, out var result))
                return result;

            return defaultValue;
        }
    }
}
