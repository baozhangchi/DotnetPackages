#region

using System.Windows.Markup;

#endregion

namespace Packages.Windows.MarkupExtensions;

/// <inheritdoc />
public class EnumBindingSourceExtension : MarkupExtension
{
    #region Fields

    private Type? _enumType;

    #endregion

    #region Properties

    /// <summary>
    ///     枚举类型
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public Type? EnumType
    {
        get => _enumType;
        set
        {
            if (_enumType != value)
            {
                if (value != null)
                {
                    var enumType = Nullable.GetUnderlyingType(value) ?? value;
                    if (!enumType.IsEnum)
                    {
                        throw new ArgumentException("必须是一个枚举类型");
                    }
                }

                _enumType = value;
            }
        }
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (_enumType == null)
        {
            throw new InvalidOperationException("必须先指定EnumType");
        }

        var enumType = Nullable.GetUnderlyingType(_enumType) ?? _enumType;
        var enumValues = Enum.GetValues(enumType);
        if (enumType == _enumType)
        {
            return enumValues;
        }

        var tempArray = Array.CreateInstance(enumType, enumValues.Length + 1);
        enumValues.CopyTo(tempArray, 1);
        return tempArray;
    }

    #endregion
}