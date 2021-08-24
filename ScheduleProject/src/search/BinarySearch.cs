using System.Collections.Generic;

namespace ScheduleProject.search
{
    /// <summary>
    /// Класс бинарного поиска ближайшего числа.
    /// Необходима предварительная сортировка - реализуется при вставке в методе Insert
    /// </summary>
    class BinarySearch : ISearch
    {
        /// <summary>
        /// Коллекция интервалов
        /// </summary>
        private List<Interval> Intervals { get; }
        
        public BinarySearch()
        {
            Intervals = new List<Interval>();
        }

        /// <summary>
        /// Вставка элемента в коллекцию интервалов моментов
        /// </summary>
        /// <param name="momentItemInterval">Интервал момента времени</param>
        public void Insert(Interval momentItemInterval)
        {
            /// альтернатива: list.Add(); list.Sort()
            
            // сортировка
            int idxMax = 0;
            foreach (var item in Intervals)
            {
                // ищем индекс максимума в списке
                if (momentItemInterval > item)
                {
                    idxMax++;
                }
            }
            // вставляем после макс
            Intervals.Insert(idxMax++, momentItemInterval);
        }

        /// <summary>
        /// Содержит ли коллекция интервало заданное число
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
                if (val >= item.Low && val <= item.High)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Поиск ближайшего вперед
        /// </summary>
        /// <param name="val">Число, для которого нужно найти ближайшее</param>
        /// <returns>Ближайшее число</returns>
        public int Nearest(int val) => SearchNearest(val , true);

        /// <summary>
        /// Поиск ближайшего назад
        /// </summary>
        /// <param name="val">Число, для которого нужно найти ближайшее назад</param>
        /// <returns>Ближайшее число</returns>
        public int NearestPrev(int val) => SearchNearest(val, false);

        /// <summary>
        /// Алгоритм бинарного поиска ближайшего числа внутри списка Intervals 
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

            /// альтернитива: list.BinarySearch()

            int left = 0;
            int right = Intervals.Count - 1;
            int index = -1;
            int median;
            int near;
            int comp;

            while (left <= right)
            {
                // медианный индекс
                median = left + (right - left) / 2;

                // ближайшее внутри списка
                near = (isUp) ? Intervals[median].NearestUp(val) : Intervals[median].NearestDown(val);

                // разность
                comp = near - val;

                if (comp == 0)      // near = val
                {
                    return near;
                }

                if (comp < 0)       // val > near
                {
                    right = median - 1;
                    index = (!isUp) ? median : index;
                }
                else                // val < near
                {
                    left = median + 1;
                    index = (isUp) ? median : index;
                }
            }

            if (index == -1)
                return -1;

            return (isUp) ? Intervals[index].NearestUp(val) : Intervals[index].NearestDown(val);
        }

    }
}
