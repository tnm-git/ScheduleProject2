
namespace ScheduleProject.moment
{
    /// <summary>
    /// Класс элемента момента времени
    /// </summary>
    public class MomentItem
    {
        public int Max { set; get; }

        public int Min { set; get; }

        public int Val { set; get; }


        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="min">Минимальное значение величины</param>
        /// <param name="max">Максимальное значение величины</param>
        /// <param name="val">Величина</param>
        public MomentItem(int min, int max, int val)
        {
            this.Min = min;
            this.Max = max;
            this.Val = val;
        }

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param name="daItem">Объект класса MomentItem</param>
        public MomentItem(MomentItem daItem)
        {
            this.Min = daItem.Min;
            this.Max = daItem.Max;
            this.Val = daItem.Val;
        }

        public static bool operator ==(MomentItem d1, MomentItem d2)
        {
            if (d1.Val == d2.Val)
                return true;

            return false;
        }

        public static bool operator !=(MomentItem d1, MomentItem d2)
        {
            if (d1.Val != d2.Val)
                return true;

            return false;
        }

        public static bool operator >(MomentItem d1, MomentItem d2)
        {
            if (d1.Val > d2.Val)
                return true;

            return false;
        }

        public static bool operator <(MomentItem d1, MomentItem d2)
        {
            if (d1.Val < d2.Val)
                return true;

            return false;
        }

        public static MomentItem operator ++(MomentItem d)
        {
            MomentItem dd = new MomentItem(d);

            if (dd.Val < dd.Max)
                dd.Val++;
            else
                dd.Val = dd.Min;
            return dd;
        }

        public static MomentItem operator --(MomentItem d)
        {
            MomentItem dd = new MomentItem(d);

            if (dd.Val > dd.Min)
                dd.Val--;
            else
                dd.Val = dd.Max;
            return dd;
        }

        public override bool Equals(object obj)
        {
            if (this == (MomentItem)obj)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
