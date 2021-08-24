
namespace ScheduleProject.search
{
    /// <summary>
    /// Содержит интервал в виде левой, правой границ и шага.
    /// Реализует обработку: поиск ближайшего вперед/назад
    /// </summary>
    readonly public struct Interval
    {
        /// <summary>
        /// Шаг
        /// </summary>
        public int Step { get; }

        /// <summary>
        /// Левая граница
        /// </summary>
        public int Low { get; }

        /// <summary>
        /// Правая граница
        /// </summary>
        public int High { get; }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="min">Минимальное значение</param>
        /// <param name="max">Максимальное значение</param>
        /// <param name="step">Шаг</param>
        public Interval(int min, int max, int step)
        {
            Check(min, max);

            Low = min;
            High = max;
            Step = step;
        }

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param name="interval">Объект структуры Interval</param>
        public Interval(Interval interval)
        {
            Low = interval.Low;
            High = interval.High;
            Step = interval.Step;
        }

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param name="val">Значение</param>
        public Interval(int val)
        {
            Low = val;
            High = val;
            Step = 1;
        }

        /// <summary>
        /// Проверка корректности. В случае некорректных величин выдается исключение
        /// </summary>
        /// <param name="min">Минимум</param>
        /// <param name="max">Максимум</param>
        private static void Check(int min, int max)
        {
            if (min > max || max < min)
                throw new System.Exception("Interval values are not correct");
        }

        public static bool operator >(Interval val1, int val)
        {
            if (val1.Low > val && val1.High > val)
                return true;

            return false;
        }

        public static bool operator <(Interval val1, int val)
        {
            if (val1.Low < val && val1.High < val)
                return true;

            return false;
        }

        public static bool operator >(Interval val1, Interval val2)
        {
            if (val1.Low > val2.Low)
                return true;

            return false;
        }

        public static bool operator <(Interval val1, Interval val2)
        {
            if (val1.Low < val2.Low)
                return false;

            return true;
        }

        /// <summary>
        /// Поиск ближайшего числа внутри интервала (вперед)
        /// </summary>
        /// <param name="val">
        /// Число для которого нужно найти ближайшее или само число
        /// </param>
        /// <returns>
        /// Ближайшее число к <paramref name="val"/> или само число
        /// </returns>
        public int NearestUp(int val)
        {

            if (val < Low) // ближайшее меньше мин
                return Low;

            if (val > High) // нет ближайшего 
                return -1;

            // остаток от деления
            int rem = (val - Low) % Step;

            // расчет смещения
            int offset = val + (Step - rem);

            // если смещение больше максимального, то вышли из диап.
            int corrected = (offset > High) ? -1 : offset;

            // если остаток нулевой, то попали в точку, иначе выдаем со смещением
            return (rem == 0) ? val : corrected;
        }

        /// <summary>
        /// Поиск ближайшего числа внутри интервала (назад)
        /// </summary>
        /// <param name="val">
        /// Число для которого нужно найти ближайшее или само число
        /// </param>
        /// <returns>
        /// Ближайшее число к <paramref name="val"/> или само число
        /// </returns>
        public int NearestDown(int val)
        {
            if (val < Low) // нет ближайшего
                return -1;

            if (val > High) // ближайшее больше макс
                return High - (High - Low) % Step;

            // остаток от деления
            int rem = (val - Low) % Step;

            // коррекция
            return val - rem;
        }
    }
}
