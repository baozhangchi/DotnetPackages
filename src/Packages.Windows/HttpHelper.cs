#region

using System.Net;
using System.Net.Sockets;

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
    }
}