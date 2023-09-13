#region

using System.Collections.Generic;
using System.Globalization;

#endregion

namespace System
{
    /// <summary>
    ///     日期扩展方法
    /// </summary>
    public static class DateTimeExtensions
    {
        private const string TIAN_GAN = "甲乙丙丁戊己庚辛壬癸";
        private const string DI_ZHI = "子丑寅卯辰巳午未申酉戌亥";
        private const string CHINESE_ZODIAC = "鼠牛虎兔龙蛇马羊猴鸡狗猪";
        private const string CM = "一二三四五六七八九十冬腊";
        private static readonly ChineseLunisolarCalendar ChineseLunisolarCalendar = new ChineseLunisolarCalendar();
        private static readonly List<string> DayList = new List<string>();

        static DateTimeExtensions()
        {
            for (var i = 1; i <= 30; i++)
            {
                var d = i % 10;
                if (d == 0)
                {
                    d = 10;
                }

                if (i < 11)
                {
                    DayList.Add($"初{CM[d - 1]}");
                }
                else if (i < 20)
                {
                    DayList.Add($"十{CM[d - 1]}");
                }
                else if (i == 20)
                {
                    DayList.Add("二十");
                }
                else if (i < 30)
                {
                    DayList.Add($"二十{CM[d - 1]}");
                }
                else
                {
                    DayList.Add("三十");
                }
            }
        }

        /// <summary>
        ///     公历转农历
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string ConvertToLunarDate(this DateTime date)
        {
            if (date > ChineseLunisolarCalendar.MaxSupportedDateTime ||
                date < ChineseLunisolarCalendar.MinSupportedDateTime)
            {
                //日期范围：1901 年 2 月 19 日 - 2101 年 1 月 28 日
                throw new Exception(
                    $"日期超出范围！必须在{ChineseLunisolarCalendar.MinSupportedDateTime:yyyy-MM-dd}到{ChineseLunisolarCalendar.MaxSupportedDateTime:yyyy-MM-dd}之间！");
            }

            var year = ChineseLunisolarCalendar.GetYear(date);
            var yearIndex = ChineseLunisolarCalendar.GetSexagenaryYear(date);
            var tg = ChineseLunisolarCalendar.GetCelestialStem(yearIndex);
            var dz = ChineseLunisolarCalendar.GetTerrestrialBranch(yearIndex);
            var flag = ChineseLunisolarCalendar.GetLeapMonth(year);
            var month = flag > 0
                ? ChineseLunisolarCalendar.GetMonth(date) - 1
                : ChineseLunisolarCalendar.GetMonth(date);
            var day = ChineseLunisolarCalendar.GetDayOfMonth(date);
            var str = flag > 0
                ? $"{TIAN_GAN[tg - 1]}{DI_ZHI[dz - 1]}年闰{CM[month - 1]}月{DayList[day - 1]}"
                : $"{TIAN_GAN[tg - 1]}{DI_ZHI[dz - 1]}年{CM[month - 1]}月{DayList[day - 1]}";
            return str;
        }

        /// <summary>
        ///     获取生肖
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetChineseZodiac(this DateTime date)
        {
            if (date > ChineseLunisolarCalendar.MaxSupportedDateTime ||
                date < ChineseLunisolarCalendar.MinSupportedDateTime)
            {
                //日期范围：1901 年 2 月 19 日 - 2101 年 1 月 28 日
                throw new Exception(
                    $"日期超出范围！必须在{ChineseLunisolarCalendar.MinSupportedDateTime:yyyy-MM-dd}到{ChineseLunisolarCalendar.MaxSupportedDateTime:yyyy-MM-dd}之间！");
            }

            var yearIndex = ChineseLunisolarCalendar.GetSexagenaryYear(date);
            var dz = ChineseLunisolarCalendar.GetTerrestrialBranch(yearIndex);
            return CHINESE_ZODIAC[dz - 1].ToString();
        }
    }
}