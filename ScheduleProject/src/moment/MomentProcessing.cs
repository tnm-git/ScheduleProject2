using System;
using System.Collections.Generic;
using ScheduleProject.formatter;
using ScheduleProject.search;

namespace ScheduleProject.moment
{
    /// <summary>
    /// Класс обработки моментов времени
    /// </summary>
    class MomentProcessing
    {
        /// <summary>
        /// Максимальное число элементов момента времени.
        /// </summary>
        public readonly int Count = 8;

        /// <summary>
        /// Набор объектов содержащих величины интервалов, алгоритмы поиска
        /// </summary>
        private Searcher[] searchers;

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param name="input">
        /// Расписание в формате строки
        /// </param>
        public MomentProcessing(string input)
        {
            searchers = new Searcher[Count];

            /*
            // Custom
            searchers[0] = new Searcher(2000, 2100, new TreeSearch(false)); // годы
            searchers[1] = new Searcher(1, 12); // месяцы
            searchers[2] = new Searcher(1, 32, new BinarySearch()); // дни
            searchers[3] = new Searcher(0, 23); // часы

            searchers[4] = new Searcher(0, 59, new BinarySearch()); // минуты
            searchers[5] = new Searcher(0, 59, new BinarySearch()); // секунды
            searchers[6] = new Searcher(0, 999, new TreeSearch(true)); // миллисекунды

            searchers[7] = new Searcher(0, 6); // дни недели
            */

            // Direct
            searchers[0] = new Searcher(2000, 2100); // годы
            searchers[1] = new Searcher(1, 12); // месяцы
            searchers[2] = new Searcher(1, 32); // дни
            searchers[3] = new Searcher(0, 23); // часы

            searchers[4] = new Searcher(0, 59); // минуты
            searchers[5] = new Searcher(0, 59); // секунды
            searchers[6] = new Searcher(0, 999); // миллисекунды

            searchers[7] = new Searcher(0, 6); // дни недели

            /*
            // Binary
            searchers[0] = new Searcher(2000, 2100, new BinarySearch()); // годы
            searchers[1] = new Searcher(1, 12, new BinarySearch()); // месяцы
            searchers[2] = new Searcher(1, 32, new BinarySearch()); // дни
            searchers[3] = new Searcher(0, 23, new BinarySearch()); // часы

            searchers[4] = new Searcher(0, 59, new BinarySearch()); // минуты
            searchers[5] = new Searcher(0, 59, new BinarySearch()); // секунды
            searchers[6] = new Searcher(0, 999, new BinarySearch()); // миллисекунды

            searchers[7] = new Searcher(0, 6, new BinarySearch()); // дни недели

            */

            /*
            // Tree
            searchers[0] = new Searcher(2000, 2100, new TreeSearch()); // годы
            searchers[1] = new Searcher(1, 12, new TreeSearch()); // месяцы
            searchers[2] = new Searcher(1, 32, new TreeSearch()); // дни
            searchers[3] = new Searcher(0, 23, new TreeSearch()); // часы

            searchers[4] = new Searcher(0, 59, new TreeSearch()); // минуты
            searchers[5] = new Searcher(0, 59, new TreeSearch()); // секунды
            searchers[6] = new Searcher(0, 999, new TreeSearch()); // миллисекунды

            searchers[7] = new Searcher(0, 6, new TreeSearch()); // дни недели
            */

            InsertSearchCollection(input);
        }

        /// <summary>
        /// Поиск паттерна по заданному расписанию
        /// </summary>
        /// <param name="s">Распиание в формате строки</param>
        /// <returns>Индекс паттерна, если -1, то паттерн не найден (исключение)</returns>
        private int DetectedPattern(string s)
        {
            return Formatter.DetectPattern(s.ToCharArray());
        }

        /// <summary>
        /// Декодирование элементов момента времени
        /// </summary>
        /// <param name="s">Расписание в формате строки</param>
        private void InsertSearchCollection(string s)
        {
            int detectedPattern = DetectedPattern(s);

            if (detectedPattern == -1)
                return;

            List<char[]> list = Formatter.Split(s.ToCharArray(), Formatter.Separators, true);

            int[] indexes = Formatter.PatternsIndexes[detectedPattern];

            int itemIndex = 0;
            foreach (var chars in list)
            {
                //Console.WriteLine(new string(chars));

                InsertToSearchers(chars, indexes[itemIndex]);
                itemIndex++;
            }
        }

        /// <summary>
        /// Запись в массив объектов класса Searchers
        /// </summary>
        /// <param name="input">Массив перечислений, относящихся к itemIndex</param>
        /// <param name="itemIndex">Индекс элемента момента времени</param>
        private void InsertToSearchers(char[] input, int itemIndex)
        {
            List<char[]> list = Formatter.Split(input, new char[] { ',' }, true);

            Interval interval;

            foreach (var chars in list)
            {
                interval = Formatter.GetInterval(chars);

                //Console.WriteLine($"Min = {interval.Low}\tMax = {interval.High}\tStep = {interval.Step}\tItemIndex = {itemIndex}");
                
                searchers[itemIndex].Insert(interval);
            }
        }

        /// <summary>
        /// Ближайший момент времени в формате DateTime
        /// </summary>
        /// <param name="dateTime">Заданный момент времени</param>
        /// <returns>Ближайшая дата, время. Date.MaxValue если не найден</returns>
        public DateTime Nearest(DateTime dateTime)
        {
            Moment input = Moment.ToMoment(dateTime);
            return Moment.ToMomentTime(IterateUpWrapper(input));
        }

        /// <summary>
        /// Ближайший предыдущий момент времени в формате DateTime
        /// </summary>
        /// <param name="dateTime">Заданный момент времени</param>
        /// <returns>Ближайшая предыдущая дата, время. Date.MaxValue если не найден</returns>
        public DateTime NearestPrev(DateTime dateTime)
        {
            Moment input = Moment.ToMoment(dateTime);
            return Moment.ToMomentTime(IterateDownWrapper(input));
        }

        /// <summary>
        /// Ближайший момент времени, не совпадающий с заданным в формате DateTime
        /// </summary>
        /// <param name="dateTime">Заданный момент времени</param>
        /// <returns>Ближайшая дата, время, не совпадающая с заданной. Date.MaxValue если не найден</returns>
        public DateTime Next(DateTime dateTime)
        {
            Moment input = Moment.ToMoment(dateTime);
            Moment nearest = IterateUpWrapper(input);

            if (nearest != input || nearest == Moment.MaxValue)
                return Moment.ToMomentTime(nearest);

            Moment temp = new Moment(input);
            Moment next = IterateUpWrapper(++temp);

            if (input == next)
                return Moment.ToMomentTime(Moment.MaxValue);

            return Moment.ToMomentTime(next);
        }

        /// <summary>
        /// Ближайший предыдущий момент времени, не совпадающий с заданным в формате DateTime
        /// </summary>
        /// <param name="dateTime">Заданный момент времени</param>
        /// <returns>Ближайшая предыдущая дата, время, не совпадающая с заданной. Date.MaxValue если не найден</returns>
        public DateTime NextPrev(DateTime dateTime)
        {
            Moment input = Moment.ToMoment(dateTime);
            Moment nearest = IterateDownWrapper(input);

            if (nearest != input || nearest == Moment.MaxValue)
                return Moment.ToMomentTime(nearest);

            Moment temp = new Moment(input); 
            Moment next = IterateDownWrapper(--temp);

            if (input == next)
                return Moment.ToMomentTime(Moment.MaxValue);

            return Moment.ToMomentTime(next);
        }

        /// <summary>
        /// Поиск ближайшего момента времени, соответствующего заданным дням недели
        /// </summary>
        /// <param name="moment">Заданный момент времени</param>
        /// <returns>Ближайший момент времени. Date.MaxValue если не найден</returns>
        private Moment IterateUpWrapper(Moment moment)
        {
            Moment nearestMoment = IterateUp(moment);

            // если бл. даты нет, сразу выход
            if (nearestMoment < moment)
                return Moment.MaxValue;

            // если день недели совпал - выходим
            if (searchers[7].Find(Moment.DayOfWeek(nearestMoment)))
                return nearestMoment;

            // обнуляем время
            nearestMoment.TimeToZero();
            moment = new Moment(nearestMoment); // copy

            int iter = 0;
            while (iter < 1000)
            {
                //Console.WriteLine();
                //Console.WriteLine($"ITER = {iter}");
                //Console.WriteLine($"INP  = {moment}");
                //Console.WriteLine($"DATE = {nearestMoment}");
                //Console.WriteLine($"DOW  = {Moment.DayOfWeek(nearestMoment)}");

                if (nearestMoment > moment)
                {
                    //Console.WriteLine($">");
                    moment = new Moment(nearestMoment); // copy
                }
                else if (nearestMoment < moment)
                {
                    //Console.WriteLine($"<");
                    return Moment.MaxValue;
                }
                else
                {
                    //Console.WriteLine($"=");

                    // если день недели совпал - сразу выходим
                    if (searchers[7].Find(Moment.DayOfWeek(nearestMoment)))
                        return nearestMoment;
                }

                // если не совпал, увеличиваем день
                moment.IncrementDay();
                nearestMoment = new Moment(IterateUp(moment));

                iter++;
            }
            throw new Exception("Search iteration is 1000");
        }

        /// <summary>
        /// Поиск ближайшего момента времени
        /// </summary>
        /// <param name="moment">Заданный момент времени</param>
        /// <returns>Ближайший момент времени. Date.MaxValue если не найден</returns>
        private Moment IterateUp(Moment moment)
        {
            if (moment == Moment.MaxValue)
                return Moment.MaxValue;

            Moment nearestMoment = new Moment();

            bool overFlow = false;

            for (int idx = Count - 2; idx >= 0; idx--)
            {
                if (overFlow)
                {
                    MomentItem temp = new MomentItem(moment[idx]);
                    nearestMoment[idx].Val = searchers[idx].Nearest((++temp).Val);
                }
                else
                {
                    nearestMoment[idx].Val = searchers[idx].Nearest(moment[idx].Val);
                }

                overFlow = false;

                if (nearestMoment[idx].Val == -1)
                {
                    nearestMoment[idx].Val = searchers[idx].NearestToMin();
                    overFlow = true;
                }

                if (nearestMoment[idx].Val != moment[idx].Val)
                {
                    if (idx == Count - 2)
                        continue;

                    for (int j = idx + 1; j < Count - 1; j++)
                    {
                        nearestMoment[j].Val = searchers[j].NearestToMin();
                    }
                }
            }

            nearestMoment.SetDayMax();

            if (nearestMoment < moment)
                return Moment.MaxValue;

            return nearestMoment;
        }

        /// <summary>
        /// Поиск ближайшего предыдущего момента времени, соответствующего заданным дням недели
        /// </summary>
        /// <param name="moment">Заданный момент времени</param>
        /// <returns>Ближайший предыдущий момент времени. Date.MaxValue если не найден</returns>
        private Moment IterateDownWrapper(Moment moment)
        {
            Moment nearestMoment = IterateDown(moment);

            // если бл. даты нет, сразу выход
            if (nearestMoment > moment)
                return Moment.MaxValue;

            // если день недели совпал - выходим
            if (searchers[7].Find(Moment.DayOfWeek(nearestMoment)))
                return nearestMoment;

            // устанавливаем время в макс
            nearestMoment.TimeToMax();
            moment = new Moment(nearestMoment); // copy

            int iter = 0;
            while (iter < 1000)
            {
                //Console.WriteLine();
                //Console.WriteLine($"ITER = {iter}");
                //Console.WriteLine($"INP  = {moment}");
                //Console.WriteLine($"DATE = {nearestMoment}");
                //Console.WriteLine($"DOW  = {Moment.DayOfWeek(nearestMoment)}");

                if (nearestMoment < moment)
                {
                    //Console.WriteLine($"<");
                    moment = new Moment(nearestMoment); // copy
                }
                else if (nearestMoment > moment)
                {
                    //Console.WriteLine($"<");
                    return Moment.MaxValue;
                }
                else
                {
                    //Console.WriteLine($"=");

                    // если день недели совпал - сразу выходим
                    if (searchers[7].Find(Moment.DayOfWeek(nearestMoment)))
                        return nearestMoment;
                }

                // если не совпал, увеличиваем день
                moment.DecrementDay();
                nearestMoment = new Moment(IterateDown(moment));

                iter++;
            }
            throw new Exception("Search iteration is 1000");
        }

        /// <summary>
        /// Поиск ближайшего предыдущего момента времени
        /// </summary>
        /// <param name="moment">Заданный момент времени</param>
        /// <returns>Ближайший предыдущий момент времени. Date.MaxValue если не найден</returns>
        private Moment IterateDown(Moment moment)
        {
            if (moment == Moment.MaxValue)
                return Moment.MaxValue;

            Moment nearestMoment = new Moment();

            bool overFlow = false;

            for (int idx = Count - 2; idx >= 0; idx--)
            {
                if (overFlow)
                {
                    MomentItem temp = new MomentItem(moment[idx]);
                    nearestMoment[idx].Val = searchers[idx].NearestPrev((--temp).Val);
                }
                else
                {
                    nearestMoment[idx].Val = searchers[idx].NearestPrev(moment[idx].Val);
                }

                overFlow = false;

                if (nearestMoment[idx].Val == -1)
                {
                    nearestMoment[idx].Val = searchers[idx].NearestToMax();
                    overFlow = true;
                }

                if (nearestMoment[idx].Val != moment[idx].Val)
                {
                    if (idx == Count - 2)
                        continue;

                    for (int j = idx + 1; j < Count - 1; j++)
                    {
                        nearestMoment[j].Val = searchers[j].NearestToMax();
                    }
                }
            }

            nearestMoment.SetDayMax();

            if (nearestMoment > moment)
                return Moment.MaxValue;

            return nearestMoment;
        }
    }
}
