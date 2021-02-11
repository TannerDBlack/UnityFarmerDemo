using System.Data.Common;
using System.Timers;
using UnityEngine;

namespace IdleFarmer
{
    

    public class DayTimer : Timer
    {
        
        public delegate void DayElapsed(int day);
        private int _dayCount=0;
        public event DayElapsed NotifyDaysOnElapse;

        #region Singleton
        public static int DayLengthInSeconds {
            set => GetInstance().Interval =value * 1000;
            get => (int) GetInstance().Interval / 1000;
        }

        private static DayTimer _instance;

        public static DayTimer GetInstance()
        {
            return _instance ?? (_instance = new DayTimer());
        }
        
        #endregion

        private DayTimer()
        {
            Interval = 5000;
            Elapsed += ProcessDay;
        }
        
        private void ProcessDay(object sender, ElapsedEventArgs e)
        {
            NotifyDaysOnElapse?.Invoke(_dayCount++);
        }
    }
}