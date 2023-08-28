#region

using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Packages.Windows
{
    /// <summary>
    ///     Http相关帮助类
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        ///     获取一个可用的端口号
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static int GetAvailablePort(IPAddress ip)
        {
            var listener = new TcpListener(ip, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }

        /// <summary>
        ///     发送Get请求并返回字符串
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static async Task<string> GetStringAsync(string url, Encoding encoding = null)
        {
            return (encoding ?? Encoding.UTF8).GetString(await GetByteArrayAsync(url));
        }

        /// <summary>
        ///     发送Get请求并返回字节数组
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<byte[]> GetByteArrayAsync(string url)
        {
            using (var client = new HttpClient())
            {
                return await client.GetByteArrayAsync(url);
            }
        }
    }
}