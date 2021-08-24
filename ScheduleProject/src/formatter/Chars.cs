using System;

namespace ScheduleProject.formatter
{
    /// <summary>
    /// Класс работы с массивами char
    /// </summary>
    static class Chars
    {
        public static char[] Copy(char[] source)
        {
            int length = source.Length;
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = source[i];
            }
            return chars;
        }

        public static char[] InsertToEnd(char[] source, char sym)
        {
            int length = source.Length;
            char[] chars = new char[length + 1];
            for (int i = 0; i < length; i++)
            {
                chars[i] = source[i];
            }
            chars[length] = sym;
            return chars;
        }

        public static char[] SubChars(char[] source, int start, int end)
        {
            if (start > source.Length - 1 || start < 0 || end > source.Length - 1 || 
                end < 0 || end < start || start > end)
            {
                throw new Exception("No correct indexes");
            }

            char[] chars = new char[end - start];
            for (int j = 0; j < end - start; j++)
            {
                chars[j] = source[start + j];
            }
            return chars;
        }

        public static char[] SubChars(char[] source, int start)
        {
            int length = source.Length;
            if (start > length - 1 || start < 0)
            {
                throw new Exception("No correct start index");
            }
            
            char[] chars = new char[length - start];
            for (int j = 0; j < length - start; j++)
            {
                chars[j] = source[start + j];
            }
            return chars;
        }

        public static bool Find(char[] source, char sym)
        {
            for (int k = 0; k < source.Length; k++)
            {
                if (source[k] == sym)
                {
                    return true;
                }
            }
            return false;
        }

        public static int IndexOf(char[] source, char sym)
        {
            for (int k = 0; k < source.Length; k++)
            {
                if (source[k] == sym)
                {
                    return k;
                }
            }
            return -1;
        }

        public static bool IsEqual(char[] chars1, char[] chars2)
        {
            if (chars1.Length != chars2.Length)
            {
                throw new Exception("Vectors must be the same length!");
            }

            for (int k = 0; k < chars1.Length; k++)
            {
                if (chars1[k] != chars2[k])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
