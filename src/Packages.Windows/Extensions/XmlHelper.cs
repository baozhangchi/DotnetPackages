#region

using System.IO;
using System.Text;
using System.Xml.Serialization;

#endregion

namespace Packages.Windows.Extensions
{
    /// <summary>
    ///     Xml帮助类
    /// </summary>
    public static class XmlHelper
    {
        /// <summary>
        ///     字符串反序列化成实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static T Deserialize<T>(this string xml, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            return encoding.GetBytes(xml).Deserialize<T>();
        }

        /// <summary>
        ///     Byte数组反序列成实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static T Deserialize<T>(this byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stream);
            }
        }

        /// <summary>
        ///     实体序列化成字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="hasNamespaces"></param>
        /// <returns></returns>
        public static string Serialize<T>(this T obj, bool hasNamespaces = true)
        {
            using (var writer = new StringWriter())
            {
                var serializer = new XmlSerializer(typeof(T));
                if (!hasNamespaces)
                {
                    var namespaces = new XmlSerializerNamespaces();
                    namespaces.Add("", "");
                    serializer.Serialize(writer, obj, namespaces);
                }
                else
                {
                    serializer.Serialize(writer, obj);
                }

                return writer.ToString();
            }
        }

        /// <summary>
        ///     实体序列化成Byte数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="encoding"></param>
        /// <param name="hasNamespaces"></param>
        /// <returns></returns>
        public static byte[] Serialize<T>(this T obj, Encoding encoding = null, bool hasNamespaces = true)
        {
            var xml = obj.Serialize(hasNamespaces);
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            return encoding.GetBytes(xml);
        }
    }
}