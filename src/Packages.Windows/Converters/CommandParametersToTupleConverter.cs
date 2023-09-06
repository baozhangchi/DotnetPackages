#region

using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Data;

#endregion

namespace Packages.Windows.Converters
{
    /// <inheritdoc />
    public class CommandParametersToTupleConverter : IMultiValueConverter
    {
        private static CommandParametersToTupleConverter _instance;

        /// <summary>
        ///     默认实例
        /// </summary>
        public static CommandParametersToTupleConverter Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                return _instance = new CommandParametersToTupleConverter();
            }
        }

        /// <inheritdoc />
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length > 0)
            {
                var methods = typeof(Tuple).GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .Where(x => x.Name == "Create").ToList();
                var method = methods.FirstOrDefault(x => x.GetParameters().Length == values.Length);
                return method != null
                    ? method.MakeGenericMethod(values.Select(x => x.GetType()).ToArray()).Invoke(null, values)
                    : null;
            }

            return null;
        }

        /// <inheritdoc />
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}