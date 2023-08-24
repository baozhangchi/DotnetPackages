// ReSharper disable once CheckNamespace

namespace System.IO
{
    /// <summary>
    ///     IO相关扩展方法
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class IOExtensions
    {
        /// <summary>
        ///     将现有目录复制到新目录，不允许覆盖现有文件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="path">目标路径</param>
        public static DirectoryInfo CopyTo(this DirectoryInfo source, string path)
        {
            return CopyTo(source, path, false);
        }

        /// <summary>
        ///     将现有目录复制到新目录，并允许覆盖现有目录
        /// </summary>
        /// <param name="source"></param>
        /// <param name="path">目标路径</param>
        /// <param name="overwrite">如果允许覆盖现有目录和文件，则为 true；否则为 false</param>
        /// <returns>为新文件；如果 overwrite 是 true，则为现有文件的覆盖。 如果文件存在且 overwrite 为 false，则引发 IOException</returns>
        public static DirectoryInfo CopyTo(this DirectoryInfo source, string path, bool overwrite)
        {
            var target = new DirectoryInfo(path);
            if (overwrite)
            {
                if (target.Exists)
                {
                    target.Delete(true);
                }

                target.Create();
            }
            else
            {
                if (!target.Exists)
                {
                    target.Create();
                }
            }

            foreach (var directoryInfo in source.GetDirectories())
            {
                var targetPath = directoryInfo.FullName.Replace(source.FullName, target.FullName);
                directoryInfo.CopyTo(targetPath, overwrite);
            }

            foreach (var fileInfo in source.GetFiles())
            {
                var targetPath = fileInfo.FullName.Replace(source.FullName, target.FullName);
                fileInfo.CopyTo(targetPath, overwrite);
            }

            return target;
        }
    }
}