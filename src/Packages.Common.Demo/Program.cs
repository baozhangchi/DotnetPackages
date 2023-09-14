using System.Drawing.Text;
using Packages.Common;

var fontNames=FontManager.GetFontFamilyNames();
foreach (var fontName in fontNames)
{
    Console.WriteLine(fontName);
}