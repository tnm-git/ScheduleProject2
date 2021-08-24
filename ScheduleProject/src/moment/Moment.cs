using System;

namespace ScheduleProject.moment
{
    /// <summary>
    /// Класс момента времени
    /// </summary>
    class Moment
    {        
        private readonly static int Count = 7;

        /// <summary>
        /// Число дней недели по месяцам
        /// </summary>
        private readonly static int[] dd = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        /// <summary>
        /// Максимальная дата, сигнализирует об отсутствии искомой величины
        /// </summary>
        public readonly static Moment MaxValue = new Moment(9999, 12, 31, 23, 59, 59, 999);

        /// <summary>
        /// Набор элементов момента времени
        /// </summary>
        private MomentItem[] momentItems = new MomentItem[Count];

        /// <summary>
        /// Конструктор по-умолчанию
        /// </summary>
        public Moment()
        {
            momentItems[0] = new MomentItem(2000, 2100, 2000); // год
            momentItems[1] = new MomentItem(1, 12, 1); // месяц
            momentItems[2] = new MomentItem(1, 32, 1); // день
            momentItems[3] = new MomentItem(0, 23, 0); // час
            momentItems[4] = new MomentItem(0, 59, 0); // минута
            momentItems[5] = new MomentItem(0, 59, 0);  // секунда
            momentItems[6] = new MomentItem(0, 999, 0);  // миллисекунда
        }

        /// <summary>
        /// Конструктор с параметрами, значениями величины
        /// </summary>
        /// <param name="year">Год</param>
        /// <param name="month">Месяц</param>
        /// <param name="day">День</param>
        /// <param name="hour">Час</param>
        /// <param name="minute">Минута</param>
        /// <param name="second">Секунда</param>
        /// <param name="msecond">Миллисекунда</param>
        public Moment(int year, int month, int day, int hour, int minute, int second, int msecond)
        {
            momentItems[0] = new MomentItem(2000, 2100, year);
            momentItems[1] = new MomentItem(1, 12, month);
            momentItems[2] = new MomentItem(1, 32, day);
            momentItems[3] = new MomentItem(0, 23, hour);
            momentItems[4] = new MomentItem(0, 59, minute);
            momentItems[5] = new MomentItem(0, 59, second);
            momentItems[6] = new MomentItem(0, 999, msecond);
        }

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param name="d">
        /// Объект класса Moment
        /// </param>
        public Moment(Moment d)
        {
            for (int i = 0; i < Count; i++)
            {
                momentItems[i] = new MomentItem(d[i]);
            }
        }

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param name="d">
        /// Объект класса DateTime
        /// </param>
        public Moment(DateTime d)
        {
            momentItems[0] = new MomentItem(2000, 2100, d.Year);
            momentItems[1] = new MomentItem(1, 12, d.Month);
            momentItems[2] = new MomentItem(1, 32, d.Day); 
            momentItems[3] = new MomentItem(0, 23, d.Hour);
            momentItems[4] = new MomentItem(0, 59, d.Minute);
            momentItems[5] = new MomentItem(0, 59, d.Second);
            momentItems[6] = new MomentItem(0, 999, d.Millisecond);
        }


        public static DateTime ToMomentTime(Moment d)
        {
            return new DateTime(d.momentItems[0].Val, d.momentItems[1].Val, 
                d.momentItems[2].Val, d.momentItems[3].Val, 
                d.momentItems[4].Val, d.momentItems[5].Val, 
                d.momentItems[6].Val);
        }

        public static Moment ToMoment(DateTime t)
        {
            return new Moment(t);
        }

        /// <summary>
        /// День недели
        /// </summary>
        /// <param name="d">
        /// Объект класса Moment
        /// </param>
        /// <returns>
        /// день недели от 0 до 6, где 0 - воскресенье
        /// </returns>
        public static int DayOfWeek(Moment d)
        {
            int day = d.momentItems[2].Val;
            int month = d.momentItems[1].Val;
            int year = d.momentItems[0].Val;

            if (month < 3)
            {
                year--;
                month += 10;
            }
            else
            {
                month -= 2;
            }

            return (day + (31 * month) / 12 + year * (1 + 1 / 4 - 1 / 100 + 1 / 400)) % 7;
        }

        /// <summary>
        /// Високосный год
        /// </summary>
        /// <param name="d">
        /// Объект класса Moment
        /// </param>
        /// <returns>
        /// true - високосный год, false - не високосный год
        /// </returns>
        private static bool IsLeap(Moment d)
        {
            int year = d.momentItems[0].Val;

            if (year % 400 == 0 || (year % 4 == 0 && year % 100 != 0))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Расчет числа дней для текущей даты (год, месяц)
        /// </summary>
        /// <param name="d">
        /// Объект класса Moment
        /// </param>
        /// <returns>
        /// Число дней для заданного месяца, года
        /// </returns>
        private static int NumDays(Moment d)
        {
            if (IsLeap(d) && d.momentItems[1].Val == 2)
                return 29;

            return dd[d.momentItems[1].Val - 1];
        }

        public static bool operator >(Moment d1, Moment d2) 
        {
            for (int i = 0; i < Count; i++)
            {
                if (d1.momentItems[i] > d2.momentItems[i])
                    return true;

                if (d1.momentItems[i] < d2.momentItems[i])
                    return false;
            }
            return false;
        }

        public static bool operator <(Moment d1, Moment d2) 
        {
            for (int i = 0; i < Count; i++)
            {
                if (d1.momentItems[i] < d2.momentItems[i])
                    return true;

                if (d1.momentItems[i] > d2.momentItems[i])
                    return false;
            }
            return false;
        }

        public static bool operator ==(Moment d1, Moment d2)
        {
            for (int i = 0; i < Count; i++)
            {
                if (d1.momentItems[i] != d2.momentItems[i])
                    return false;
            }
            return true;
        }

        public static bool operator !=(Moment d1, Moment d2)
        {
            if (d1 == d2)
                return false;

            return true;
        }

        public static Moment operator ++(Moment d)
        {
            Moment dd = new Moment();

            bool inc = true;
            for (int i = Count - 1; i >= 0; i--)
            {
                dd.momentItems[i] = new MomentItem(d.momentItems[i]);

                dd.momentItems[2].Max = NumDays(d);

                if (inc)
                    dd.momentItems[i]++;

                if (dd.momentItems[i] < d.momentItems[i]) 
                    inc = true;
                else
                    inc = false;
            }

            if (dd < d)
                return Moment.MaxValue;

            return dd;
        }

        public static Moment operator --(Moment d) 
        {
            Moment dd = new Moment(d);

            bool dec = true;
            for (int i = Count - 1; i >= 0; i--)
            {
                dd.momentItems[i] = new MomentItem(d.momentItems[i]);

                if (dec)
                {
                    // Если уменьшаем день, а его текущее значение равно минимуму
                    // значит последует переход месяца, нужно выставить ограничение 
                    if (i == 2 && dd.momentItems[i].Val == dd.momentItems[i].Min) 
                    {
                        Moment x = new Moment(dd);
                        x.momentItems[1]--;
                        dd.momentItems[2].Max = NumDays(x);
                    }
                    dd.momentItems[i]--;
                }
                    
                if (dd.momentItems[i] > d.momentItems[i])
                    dec = true;
                else
                    dec = false;
            }

            if (dd > d)
                return MaxValue;

            return dd;
        }

        public void IncrementDay()
        {
            momentItems[2]++;
        }

        public void DecrementDay()
        {
            momentItems[2]--;
        }


        /// <summary>
        /// Обнуление времени
        /// </summary>
        public void TimeToZero()
        {
            for (int i = 3; i < Count; i++)
                momentItems[i].Val = momentItems[i].Min;
        }

        /// <summary>
        /// Установка времени в макс значения
        /// </summary>
        public void TimeToMax()
        {
            for (int i = 3; i < Count; i++)
                momentItems[i].Val = momentItems[i].Max;
        }

        /// <summary>
        /// Установка последнего дня
        /// </summary>
        public void SetDayMax()
        {
            if (momentItems[2].Val == 32)
                momentItems[2].Val = NumDays(this);
        }


        /// <summary>
        /// Индексатор для обращения к объекту массива momentItems через индекс 
        /// объекта Moment
        /// </summary>
        /// <param name="index">
        /// Индекс
        /// </param>
        /// <returns>
        /// Объект MomentItem
        /// </returns>
        public MomentItem this[int index]
        {
            get
            {
                return momentItems[index];
            }
            private set
            {
                momentItems[index] = value;
            }
        }


        public override string ToString()
        {
            return $"{momentItems[0].Val}.{momentItems[1].Val}.{momentItems[2].Val} {DayOfWeek(this)} " +
                   $"{momentItems[3].Val}:{momentItems[4].Val}:{momentItems[5].Val}.{momentItems[6].Val}";
        }

        public override bool Equals(object obj)
        {
            if (this == (Moment)obj)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
