
namespace ScheduleProject.search
{
    /// <summary>
    /// Класс поиска
    /// </summary>
    class Searcher
    {
        public int Min { protected set; get; }
        public int Max { protected set; get; }

        /// <summary>
        /// Интерфейс поиска
        /// </summary>
        public ISearch Search { get; protected set; }

        /// <summary>
        /// Конструктор без указания метода поиска, по-умолчанию метод поиска - прямой
        /// </summary>
        /// <param name="min">
        /// Минимальное значение момента времени
        /// </param>
        /// <param name="max">
        /// Максимальное значение момента времени
        /// </param>
        public Searcher(int min, int max)
        {
            Min = min;
            Max = max;
            Search = new DirectSearch(); // by default
        }

        /// <summary>
        /// Конструктор с указанием метода поиска
        /// </summary>
        /// <param name="min">
        /// Минимальное значение момента времени
        /// </param>
        /// <param name="max">
        /// Максимальное значение момента времени
        /// </param>
        /// <param name="search">
        /// Метод поиска
        /// </param>
        public Searcher(int min, int max, ISearch search)
        {
            Min = min;
            Max = max;
            Search = search;
        }

        /// <summary>
        /// Вставка интервала, реализуется предварительная проверка
        /// </summary>
        /// <param name="interval">
        /// Интервал моментов
        /// </param>
        public void Insert(Interval interval)
        {
            if (interval.Low == -1 && interval.High == -1)
            {
                Search.Insert(new Interval(Min, Max, interval.Step));
            }
            else
            {
                if (interval < Min)
                    throw new System.Exception("Interval values less then item Min");

                if (interval > Max)
                    throw new System.Exception("Interval values more then item Max");

                Search.Insert(interval);
            }
        }
            

        public int Nearest(int val) => Search.Nearest(val);

        public int NearestPrev(int val) => Search.NearestPrev(val);

        public int NearestToMin() => Search.Nearest(Min); 

        public int NearestToMax() => Search.NearestPrev(Max);

        public bool Find(int val) => Search.Contains(val);

    }
}
