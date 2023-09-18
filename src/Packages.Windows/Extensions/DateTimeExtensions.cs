#region

using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

#endregion

namespace System
{
    /// <summary>
    ///     日期扩展方法
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        ///     公历转农历字符串
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string ToLunarString(this DateTime date)
        {
            return ((LunarDate)date).ToString();
        }

        /// <summary>
        ///     转换成农历日期
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static LunarDate ToLunarDate(this DateTime date)
        {
            return LunarDate.FromDateTime(date);
        }
    }

    /// <summary>
    ///     农历日期
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Auto)]
    public struct LunarDate : ISerializable, IEquatable<LunarDate>, IEqualityComparer<LunarDate>
    {
        private const string TIAN_GAN = "甲乙丙丁戊己庚辛壬癸";
        private const string DI_ZHI = "子丑寅卯辰巳午未申酉戌亥";
        private const string CHINESE_ZODIAC = "鼠牛虎兔龙蛇马羊猴鸡狗猪";
        private const string CM = "一二三四五六七八九十冬腊";
        private static readonly ChineseLunisolarCalendar ChineseLunisolarCalendar = new ChineseLunisolarCalendar();
        private static readonly List<string> DayList = new List<string>();
        private DateTime _originDate;

        static LunarDate()
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
        ///     年
        /// </summary>
        public int Year { get; private set; }

        /// <summary>
        ///     月
        /// </summary>
        public int Month { get; private set; }

        /// <summary>
        ///     日
        /// </summary>
        public int Day { get; private set; }

        /// <summary>
        ///     初始化一个农历日期
        /// </summary>
        /// <param name="date"></param>
        /// <exception cref="Exception"></exception>
        public LunarDate(DateTime date) : this()
        {
            SetOriginDate(date);
        }

        /// <summary>
        ///     返回一个新的LunarDate，它将指定的年数加到此实例的值上
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public LunarDate AddYears(int value)
        {
            return new LunarDate(_originDate.AddYears(value));
        }

        /// <summary>
        ///     返回一个新的LunarDate，它将指定的月数加到此实例的值上
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public LunarDate AddMonths(int value)
        {
            return new LunarDate(_originDate.AddMonths(value));
        }

        /// <summary>
        ///     返回一个新的LunarDate，它将指定的天数加到此实例的值上
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public LunarDate AddDays(int value)
        {
            return new LunarDate(_originDate.AddDays(value));
        }

        private void SetOriginDate(DateTime date)
        {
            if (date > ChineseLunisolarCalendar.MaxSupportedDateTime ||
                date < ChineseLunisolarCalendar.MinSupportedDateTime)
            {
                //日期范围：1901 年 2 月 19 日 - 2101 年 1 月 28 日
                throw new Exception(
                    $"日期超出范围！必须在{ChineseLunisolarCalendar.MinSupportedDateTime:yyyy-MM-dd}到{ChineseLunisolarCalendar.MaxSupportedDateTime:yyyy-MM-dd}之间！");
            }

            _originDate = date.Date;
            var year = ChineseLunisolarCalendar.GetYear(date);
            var month = ChineseLunisolarCalendar.GetMonth(date);
            var day = ChineseLunisolarCalendar.GetDayOfMonth(date);
            var yearIndex = ChineseLunisolarCalendar.GetSexagenaryYear(date);
            var tg = ChineseLunisolarCalendar.GetCelestialStem(yearIndex);
            var dz = ChineseLunisolarCalendar.GetTerrestrialBranch(yearIndex);
            var leapMonth = ChineseLunisolarCalendar.GetLeapMonth(year);
            var isLeap = false;
            if (leapMonth > 0)
            {
                isLeap = leapMonth == month;
                month = leapMonth <= month
                    ? month - 1
                    : month;
            }

            Year = year;
            Month = month;
            Day = day;
            IsLeap = isLeap;
            CelestialStem = $"{TIAN_GAN[tg - 1]}";
            TerrestrialBranch = $"{DI_ZHI[dz - 1]}";
            ChineseZodiac = $"{CHINESE_ZODIAC[dz - 1]}";
        }

        /// <summary>
        ///     生肖
        /// </summary>
        public string ChineseZodiac { get; private set; }

        /// <summary>
        ///     地支
        /// </summary>
        public string TerrestrialBranch { get; private set; }

        /// <summary>
        ///     天罡
        /// </summary>
        public string CelestialStem { get; private set; }

        /// <summary>
        ///     是否是闰月
        /// </summary>
        public bool IsLeap { get; private set; }

        /// <summary>
        ///     把当前农历日期转成字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var str = IsLeap
                ? $"{CelestialStem}{TerrestrialBranch}年闰{CM[Month - 1]}月{DayList[Day - 1]}"
                : $"{CelestialStem}{TerrestrialBranch}年{CM[Month - 1]}月{DayList[Day - 1]}";
            return str;
        }

        /// <summary>
        ///     实例化一个LunarDate
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="isLeap"></param>
        public LunarDate(int year, int month, int day, bool isLeap = false) : this()
        {
            var date = ChineseLunisolarCalendar.ToDateTime(year, isLeap ? month + 1 : month, day, 0, 0, 0, 0);
            SetOriginDate(date);
        }

        /// <summary>
        /// </summary>
        /// <param name="lunarDate"></param>
        /// <returns></returns>
        public static implicit operator DateTime(LunarDate lunarDate)
        {
            return lunarDate._originDate;
        }

        /// <summary>
        /// </summary>
        /// <param name="date"></param>
        public static explicit operator LunarDate(DateTime date)
        {
            return new LunarDate(date);
        }

        /// <summary>
        ///     返回一个LunarDate
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static LunarDate FromDateTime(DateTime date)
        {
            return new LunarDate(date);
        }

        /// <summary>
        ///     抓换成公历
        /// </summary>
        /// <returns></returns>
        public DateTime ToDateTime()
        {
            return _originDate;
        }


#if NET6_0_OR_GREATER
        /// <summary>
        /// 初始化一个农历日期
        /// </summary>
        /// <param name="date"></param>
        public LunarDate(DateOnly date) : this()
        {
            SetOriginDate(date.ToDateTime(new TimeOnly(0, 0, 0)));
        }


        /// <summary>
        /// </summary>
        /// <param name="date"></param>
        public static implicit operator DateOnly(LunarDate lunarDate) => DateOnly.FromDateTime(lunarDate._originDate);

        /// <summary>
        /// </summary>
        /// <param name="date"></param>
        public static explicit operator LunarDate(DateOnly date) => new LunarDate(date);

        /// <summary>
        /// 返回一个LunarDate
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static LunarDate FromDateOnly(DateOnly date)
        {
            return new LunarDate(date);
        }

        /// <summary>
        ///     抓换成公历日期
        /// </summary>
        /// <returns></returns>
        public DateOnly ToDateOnly()
        {
            return DateOnly.FromDateTime(_originDate);
        }
#endif
        /// <inheritdoc />
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Year), Year);
            info.AddValue(nameof(Month), Month);
            info.AddValue(nameof(IsLeap), IsLeap);
            info.AddValue(nameof(Day), Day);
        }

        private LunarDate(SerializationInfo info, StreamingContext context) : this()
        {
            var year = info.GetInt32(nameof(Year));
            var month = info.GetInt32(nameof(Month));
            var isLeap = info.GetBoolean(nameof(IsLeap));
            var day = info.GetInt32(nameof(Day));
            SetOriginDate(ChineseLunisolarCalendar.ToDateTime(year, isLeap ? month + 1 : month, day, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        public bool Equals(LunarDate other)
        {
            return Year == other.Year && Month == other.Month && Day == other.Day && IsLeap == other.IsLeap;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is LunarDate other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Year;
                hashCode = (hashCode * 397) ^ Month;
                hashCode = (hashCode * 397) ^ Day;
                hashCode = (hashCode * 397) ^ IsLeap.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        ///     ==
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(LunarDate left, LunarDate right)
        {
            return left.Equals(right);
        }

        /// <summary>
        ///     !=
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(LunarDate left, LunarDate right)
        {
            return !left.Equals(right);
        }

        /// <inheritdoc />
        public bool Equals(LunarDate x, LunarDate y)
        {
            return x.Year == y.Year && x.Month == y.Month && x.Day == y.Day && x.IsLeap == y.IsLeap;
        }

        /// <inheritdoc />
        public int GetHashCode(LunarDate obj)
        {
            unchecked
            {
                var hashCode = obj.Year;
                hashCode = (hashCode * 397) ^ obj.Month;
                hashCode = (hashCode * 397) ^ obj.Day;
                hashCode = (hashCode * 397) ^ obj.IsLeap.GetHashCode();
                return hashCode;
            }
        }
    }
}