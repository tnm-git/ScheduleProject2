
namespace ScheduleProject.search
{
    /// <summary>
    /// Класс узла дерева
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Высота (число уровней)
        /// </summary>
        public int Height;

        /// <summary>
        /// Текущий ключ узла
        /// </summary>
        public Interval Key;

        /// <summary>
        /// Ссылка не левое поддерево
        /// </summary>
        public Node Left;

        /// <summary>
        /// Ссылка не правое поддерево
        /// </summary>
        public Node Right;
    };
}
