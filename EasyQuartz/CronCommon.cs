using System;
using System.Collections.Generic;
using System.Text;

namespace EasyQuartz
{
    public static class CronCommon
    {
        /// <summary>
        /// 秒级间隔
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        public static string SecondInterval(int second)
        {
            second = second > 60 ? 60 : second;
            second = second <= 0 ? 1 : second;
            return $"*/{second} * * * * ?";
        }

        /// <summary>
        /// 分钟级间隔
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static string MinuteInterval(int interval)
        {
            interval = interval > 60 ? 60 : interval;
            interval = interval <= 0 ? 1 : interval;
            return $"0 */{interval} * * * ?";
        }

    }
}
