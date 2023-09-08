﻿#region

using System.Collections.Generic;
using System.Linq;

#endregion

// ReSharper disable once CheckNamespace
namespace System
{
    /// <summary>
    ///     身份证号码相关
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class IDCardNoHelper
    {
        private static readonly List<AreaCodeItem> AreaCodeItems;
        private static readonly Random Random;
        private static int _startYear;
        private static int _endYear;
        private static readonly int[] Coefficient = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };

        private static readonly Dictionary<int, string> RemainderCheckCodeMap = new()
        {
            { 0, "1" }, { 1, "0" }, { 2, "X" }, { 3, "9" }, { 4, "8" }, { 5, "7" }, { 6, "6" }, { 7, "5" }, { 8, "4" },
            { 9, "3" }, { 10, "2" }
        };

        static IDCardNoHelper()
        {
            AreaCodeItems = new List<AreaCodeItem>();
            var content = Packages.Avalonia.Properties.Resources.行政区划代码;
            var index = 0;
            foreach (var line in content.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (index > 0)
                {
                    var items = line.Split(',');
                    AreaCodeItems.Add(new AreaCodeItem
                    {
                        Name = items[0],
                        Code = items[1]
                    });
                }

                index++;
            }

            Random = new Random();
            _startYear = DateTime.Now.Year - 65;
            _endYear = DateTime.Now.Year - 18;
        }

        /// <summary>
        ///     设置起止年份
        /// </summary>
        /// <param name="startYear">开始年份</param>
        /// <param name="endYear">结束年份</param>
        public static void SetYearRange(int startYear, int endYear)
        {
            _startYear = Math.Min(startYear, endYear);
            _endYear = Math.Max(startYear, endYear);
        }

        /// <summary>
        ///     生成身份证号码
        /// </summary>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public static IEnumerable<string> GenerateIdCardNos(int count = 1)
        {
            for (var i = 0; i < count; i++)
            {
                var areaCode = AreaCodeItems[Random.Next(0, AreaCodeItems.Count)].Code;
                var year = Random.Next(_startYear, _endYear + 1);
                var month = Random.Next(1, 13);
                var day = new[] { 1, 3, 5, 7, 8, 10, 12 }.Contains(month) ? Random.Next(1, 32) :
                    new[] { 4, 6, 9, 11 }.Contains(month) ? Random.Next(1, 31) : Random.Next(1, 29);
                yield return CalculateIdCardNo($"{areaCode}{year:0000}{month:00}{day:00}{Random.Next(100, 1000):000}");
            }
        }

        private static string CalculateIdCardNo(string input)
        {
            var nums = input.ToLower().Select(x => int.Parse(x.ToString())).ToList();
            var sum = 0;

            for (var i = 0; i < nums.Count(); i++)
            {
                sum += nums[i] * Coefficient[i];
            }

            var remainder = sum % 11;

            return $"{input}{RemainderCheckCodeMap[remainder]}";
        }

        /// <summary>
        ///     生成身份证号码
        /// </summary>
        /// <param name="areaCode">行政区划代码</param>
        /// <returns></returns>
        public static string GenerateIdCardNo(string areaCode = null)
        {
            if (string.IsNullOrWhiteSpace(areaCode) || areaCode.Length != 6 ||
                !AreaCodeItems.Any(x => x.Code.Equals(areaCode)))
            {
                areaCode = AreaCodeItems[Random.Next(0, AreaCodeItems.Count)].Code;
            }

            var year = Random.Next(_startYear, _endYear + 1);
            var month = Random.Next(1, 13);
            var day = new[] { 1, 3, 5, 7, 8, 10, 12 }.Contains(month) ? Random.Next(1, 32) :
                new[] { 4, 6, 9, 11 }.Contains(month) ? Random.Next(1, 31) : Random.Next(1, 29);
            return CalculateIdCardNo($"{areaCode}{year:0000}{month:00}{day:00}{Random.Next(100, 1000):000}");
        }

        internal class AreaCodeItem
        {
            public string Name { get; set; }

            public string Code { get; set; }
        }
    }
}