using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace System.Drawing.Text
{
    /// <summary>
    /// 系统字体帮助类
    /// </summary>
    public class FontManager
    {
        /// <summary>
        /// 获取系统字体名称
        /// </summary>
        /// <returns></returns>
        public static List<string> GetFontFamilyNames()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return GetFontFamilyNamesOnWindows();
            }
            else
            {
                return GetFontFamilyNamesOnUnix();
            }
        }

        private static List<string> GetFontFamilyNamesOnUnix()
        {
            var content = Cmder.Run("fc-list -f \"%{family}\n\"");
            return content.Split(new char[] { '\r', '\n', ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct()
                .ToList();
        }

        private static List<string> GetFontFamilyNamesOnWindows()
        {
            var fontFiles = Directory.GetFiles(@"C:\Windows\Fonts", "*.ttf");
            var fontCollection = new PrivateFontCollection();
            foreach (var fontFile in fontFiles)
            {
                fontCollection.AddFontFile(fontFile);
            }

            var fontNames = new List<string>();
            foreach (var fontFamily in fontCollection.Families)
            {
                fontNames.Add(fontFamily.Name);
            }

            return fontNames;
        }
    }
}