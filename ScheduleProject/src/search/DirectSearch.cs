using System.Collections.Generic;

namespace ScheduleProject.search
{    
    /// <summary>
    /// Класс прямого поиска ближайшего числа
    /// </summary>
    class DirectSearch : ISearch
    {
        /// <summary>
        /// Коллекция интервалов
        /// </summary>
        private List<Interval> Intervals { get; }

        public DirectSearch()
        {
            Intervals = new List<Interval>();
        }

        /// <summary>
        /// Вставка элемента в коллекцию интервалов моментов
        /// </summary>
        /// <param name="momentItemInterval">Интервал момента времени</param>
        public void Insert(Interval momentItemInterval)
        {
            Intervals.Add(momentItemInterval);
        }

        /// <summary>
        /// Поиск ближайшего вперед
        /// </summary>
        /// <param name="val">Число, для которого нужно найти ближайшее</param>
        /// <returns>Ближайшее число</returns>
        public int Nearest(int val) => SearchNearest(val, true);

        /// <summary>
        /// Поиск ближайшего назад
        /// </summary>
        /// <param name="val">Число, для которого нужно найти ближайшее назад</param>
        /// <returns>Ближайшее число</returns>
        public int NearestPrev(int val) => SearchNearest(val, false);

        /// <summary>
        /// Содержит ли коллекция интервалов заданное число
        /// </summary>
        /// <param name="val">Заданное число</param>
        /// <returns>true - если содержит, false - если нет</returns>
        public bool Contains(int val)
        {
            // в коллекции нет объектов
            if (Intervals.Count == 0)
            {
                return true;
            }

            foreach (var item in Intervals)
            {
                // ищем индекс максимума в списке
                if (val >= item.Low && val <= item.High)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Алгоритм прямого поиска ближайшего числа внутри списка Intervals 
        /// </summary>
        /// <param name="val">
        /// Число, для которого нужно такое же/ближайшее
        /// </param>
        /// <param name="isUp">
        /// <paramref name="isUp"/> = true - поиск вперед, 
        /// <paramref name="isUp"/> = false - поиск назад
        /// </param>
        /// <returns>
        /// Ближайшее число
        /// </returns>
        private int SearchNearest(int val, bool isUp)
        {
            // в коллекции нет объектов
            if (Intervals.Count == 0) 
            {
                return val;
            }

            int diffMin = int.MaxValue;
            int nearestTemp;
            int nearest = -1;
            int diff;

            foreach (var l in Intervals)
            {
                if (isUp)
                {
                    nearestTemp = l.NearestUp(val);
                    diff = nearestTemp - val;
                }
                else
                {
                    nearestTemp = l.NearestDown(val);
                    diff = val - nearestTemp;
                }

                if (diff == 0)
                    return nearestTemp;

                if (nearestTemp == -1)
                    continue;

                if (diff < diffMin)
                {
                    diffMin = diff;
                    nearest = nearestTemp;
                }
            }

            return nearest;
        }
    }
}
