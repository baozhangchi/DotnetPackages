#region

using System.ComponentModel;
using System.Reflection;

#endregion

// ReSharper disable once CheckNamespace
namespace System
{
    /// <summary>
    /// </summary>
    public static class EnumExtensions
    {
        #region Methods

        /// <summary>
        ///     获取枚举值的Description
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field!.GetCustomAttribute<DescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.Description;
        }

        #endregion
    }
}