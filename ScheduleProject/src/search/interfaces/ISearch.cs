
namespace ScheduleProject.search
{

    /// <summary>
    /// Интерфейс поиска ближайшего числа (момента)
    /// </summary>
    interface ISearch
    {
        /// <summary>
        /// Алгоритм вставки интервала в коллекцию
        /// </summary>
        /// <param name="dateItemInterval">
        /// Интервал
        /// </param>
        public void Insert(Interval dateItemInterval);

        /// <summary>
        /// Получение ближайшего числа (вперед)
        /// </summary>
        /// <param name="val">
        /// Число, для которого нужно найти ближайшее
        /// </param>
        /// <returns>
        /// Ближайшее число
        /// </returns>
        public int Nearest(int val);

        /// <summary>
        /// Получение ближайшего числа (назад)
        /// </summary>
        /// <param name="val">
        /// Число, для которого нужно найти ближайшее
        /// </param>
        /// <returns>
        /// Ближайшее число
        /// </returns>
        public int NearestPrev(int val);

        /// <summary>
        /// Содержит ли коллекция заданный момент
        /// </summary>
        /// <param name="val">Проверяемый момент</param>
        /// <returns>true - содержит, false - нет</returns>
        public bool Contains(int val);
    }
}
