#region

using System.ComponentModel;
using System.Globalization;
using System.Reflection;

#endregion

// ReSharper disable once CheckNamespace
namespace System
{
    /// <inheritdoc />
    public class EnumDescriptionTypeConverter : EnumConverter
    {
        #region Constructors

        /// <inheritdoc />
        public EnumDescriptionTypeConverter(Type type) : base(type)
        {
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (value != null)
                {
                    var field = value.GetType().GetField(value.ToString()!);
                    var attribute = field!.GetCustomAttribute<DescriptionAttribute>();
                    if (attribute != null)
                    {
                        return attribute.Description;
                    }
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion
    }
}