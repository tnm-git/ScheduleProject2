using System;
using System.Collections.Generic;
using ScheduleProject.search;

namespace ScheduleProject.formatter
{
    /// <summary>
    /// Класс обработки паттерна расписания (формата)
    /// </summary>
    static class Formatter
    {
        /// <summary>
        /// Набор паттернов. Можно добавлять свои паттерны, 
        /// в случае одинаковых выдастся исключение в CheckEqualsPatterns() 
        /// </summary>
        private readonly static string[] patterns =
        {
            "yyyy.MM.dd w HH:mm:ss.fff", // #0
            "yyyy.MM.dd HH:mm:ss.fff", // #1
            "HH:mm:ss.fff", // #2
            "yyyy.MM.dd w HH:mm:ss", // #3
            "yyyy.MM.dd HH:mm:ss", // #4
            "HH:mm:ss", // #5
        };

        /// <summary>
        /// Набор разделителей сегментов паттернов
        /// </summary>
        public readonly static char[] Separators = { ' ', '.', ':' };

        /// <summary>
        /// Кодовые последовательности 
        /// </summary>
        public static List<int[]> PatternsIndexes;

        /// <summary>
        /// Словарь соответствия сегмента, его индексу
        /// в кодовой последовательности
        /// </summary>
        private static Dictionary<string, int> itemsDecoder =
            new Dictionary<string, int>
        {
            { "yyyy", 0}, // год
            { "MM",   1}, // месяц
            { "dd",   2}, // день

            { "w",    7}, // день недели

            { "HH",   3}, // часы
            { "mm",   4}, // минуты
            { "ss",   5}, // секунды
            { "fff",  6}  // миллисекунды
        };

        /// <summary>
        /// Статический конструктор по-умолчанию
        /// </summary>
        static Formatter()
        {
            CheckEqualsPatterns();
            PatternsCoder();
        }

        /// <summary>
        /// Разделение массива элементов char на коллекцию массивов char
        /// </summary>
        /// <param name="str">Массив элементов char (строка)</param>
        /// <param name="seps">Набор разделителей, на основе которых производится разделение</param>
        /// <param name="isToSegments">Способ разделения, 
        /// = true - выделение разделителей,
        /// = false - выделение строк между разделителями</param>
        /// <returns>Коллекция массивов char</returns>
        public static List<char[]> Split(char[] str, char[] seps, bool isToSegments)
        {
            List<char[]> segments = new List<char[]>();
            int length = str.Length;
            int lastIdx = 0;
            char[] chars;

            if (isToSegments)
            {
                // чтобы не записывать дополнительный сегмент после выхода из цикла:
                chars = Chars.InsertToEnd(str, seps[0]);
                length++;
            }
            else
            {
                chars = Chars.Copy(str);
            }

            for (int i = 0; i < length; i++)
            {
                if (Chars.Find(seps, chars[i]))
                {
                    if (isToSegments) // добавляем строки в коллекцию
                    {
                        segments.Add(Chars.SubChars(chars, lastIdx, i));
                        lastIdx = i + 1;
                    }
                    else // добавляем разделители в коллекцию
                    {
                        segments.Add(new char[] { chars[i] });
                    }
                }
            }

            return segments;
        }

        /// <summary>
        /// Формирование объекта структуры Interval по полученным перечислениям
        /// </summary>
        /// <param name="input">Перечисления (см. в коде таблицу)</param>
        /// <returns>Объект структуры Interval</returns>
        public static Interval GetInterval(char[] input)
        {
            List<char[]> splitStrings = Split(input, new char[] { '-', '/' }, true);

            int step = 1;
            int min = -1;
            int max = -1;

            bool isEvery = (Chars.Find(input, '*')) ? true : false;
            bool isRange = (Chars.Find(input, '-')) ? true : false;
            bool isStep = (Chars.Find(input, '/')) ? true : false;

            /*

            // X          isEvery = false     isStep = false      isRange = false     
            // X-Y        isEvery = false     isStep = false      isRange = true 
            // *          isEvery = true      isStep = false      isRange = false       
            // * /W       isEvery = true      isStep = true       isRange = false 
            // X-Y/W      isEvery = false     isStep = true       isRange = true 

            */

            if (!isEvery & !isStep & !isRange) // X 
            {
                min = int.Parse(splitStrings[0]);
                max = min;
            }
            else if (!isEvery & !isStep & isRange) // X-Y
            {
                min = int.Parse(splitStrings[0]);
                max = int.Parse(splitStrings[1]);
            }
            else if (isEvery & isStep & !isRange) //   * /W
            {
                step = int.Parse(splitStrings[1]);
            }
            else if (!isEvery & isStep & isRange) //   X-Y/W
            {
                min = int.Parse(splitStrings[0]);
                max = int.Parse(splitStrings[1]);
                step = int.Parse(splitStrings[2]);
            }

            return new Interval(min, max, step); // (isEvery & !isStep & !isRange) //  *
        }

        /// <summary>
        /// Детектирование паттерна
        /// </summary>
        /// <param name="str">Расписание в формате массива char</param>
        /// <returns>Номер паттерна (кодовой последовательности)</returns>
        public static int DetectPattern(char[] str)
        {
            List<char[]> checkedList = Split(str, Separators, false);
            List<char[]> patList;

            for (int i = 0; i < patterns.Length; i++)
            {
                patList = Split(patterns[i].ToCharArray(), Separators, false);

                if (patList.Count.Equals(checkedList.Count))
                {
                    bool isEqual = true;

                    for (int j = 0; j < patList.Count; j++)
                    {
                        if (!Chars.IsEqual(patList[j], checkedList[j]))
                        {
                            isEqual = false;
                        }
                    }

                    if (isEqual)
                    {
                        Console.WriteLine($"Detected pattern is #{i}");
                        return i;
                    }
                }
            }
            throw new Exception("No detected pattern");
        }

        /// <summary>
        /// Кодирование паттернов в кодовые проследовательности (на основе словаря соответствия) 
        /// </summary>
        private static void PatternsCoder()
        {
            PatternsIndexes = new List<int[]>();
            for (int i = 0; i < patterns.Length; i++)
            {
                List<int> numbers = new List<int>();
                List<char[]> pattern = Split(patterns[i].ToCharArray(), Separators, true);
                foreach (var pat in pattern)
                {
                    numbers.Add(itemsDecoder[new string(pat)]);
                }
                PatternsIndexes.Add(numbers.ToArray());

                /*
                 вывод
                int[] cc = PatternsIndexes[i];
                string p = string.Empty;
                for (int j = 0; j < cc.Length; j++)
                {
                    p += " " + cc[j];
                }
                Console.WriteLine($"Code[{i}] = " + p);
                */
            }
        }

        /// <summary>
        /// Проверка на сущестсование одинаковых паттернов
        /// </summary>
        private static void CheckEqualsPatterns() 
        {
            // Если одинаковые паттерны существуют, то нельзя
            // однозначно декодировать последовательность 

            string[] patternsSeparators = GetPatternsSeparators();

            for (int i = 0; i < patternsSeparators.Length; i++)
            {
                for (int j = 0; j < patternsSeparators.Length; j++)
                {
                    if (i != j && patternsSeparators[i] == patternsSeparators[j])
                    {
                        throw new Exception("Equals patterns detected");
                    }
                }
            }
        }

        /// <summary>
        /// Получение раздетителей паттернов
        /// </summary>
        /// <returns>Массив строк разделителей</returns>
        private static string[] GetPatternsSeparators()
        {
            string[] patternsSeparators = new string[patterns.Length];
            for (int i = 0; i < patterns.Length; i++)
            {
                List<char[]> patList = Split(patterns[i].ToCharArray(), Separators, false);

                patternsSeparators[i] = string.Empty;
                foreach (var item in patList)
                {
                    patternsSeparators[i] += item[0];
                }
                //Console.WriteLine($"Pattern[{i}] = " + patternsSeparators[i]);
            }
            return patternsSeparators;
        }
    }
}
