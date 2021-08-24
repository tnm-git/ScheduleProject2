
namespace ScheduleProject.search
{
    /// <summary>
    /// Класс поиска в бинарном дереве
    /// </summary>
    class TreeSearch : ISearch
    {        
        /// <summary>
        /// Дерево интервалов
        /// </summary>
        private BinaryTree binaryTree;

        /// <summary>
        /// Узел дерева
        /// </summary>
        private Node node;

        /// <summary>
        /// Тип дерева (алгоритм вставки)
        /// </summary>
        private bool isAVL;

        public TreeSearch()
        {
            isAVL = true;

            node = new Node();
            binaryTree = new BinaryTree();
        }

        /// <summary>
        /// Конструктор с параметром
        /// </summary>
        /// <param name="isAVL">Алгоритм вставки = true - АВЛ</param>
        public TreeSearch(bool isAVL)
        {
            this.isAVL = isAVL;

            node = new Node();
            binaryTree = new BinaryTree();
        }

        /// <summary>
        /// Вставка элемента в дерево интервалов
        /// </summary>
        /// <param name="momentItemInterval">Интервал момента времени</param>
        public void Insert(Interval momentItemInterval)
        {
            if (isAVL)
                node = binaryTree.InsertAVL(node, momentItemInterval); // в режиме самобалансировки
            else
                node = binaryTree.Insert(node, momentItemInterval); // без самобалансировки, эффективность такая же как у DirectSearch
        }

        /// <summary>
        /// Содержит ли дерево интервалов заданное число
        /// </summary>
        /// <param name="val">Заданное число</param>
        /// <returns>true - если содержит, false - если нет</returns>
        public bool Contains(int val)
        {
            if (binaryTree.Count == 0)
                return true;

            if (Nearest(val) != -1)
                return true;

            return false;
        }

        /// <summary>
        /// Поиск ближайшего вперед
        /// </summary>
        /// <param name="val">Число, для которого нужно найти ближайшее</param>
        /// <returns>Ближайшее число</returns>
        public int Nearest(int val)
        {
            if (binaryTree.Count == 0)
                return val;

            return binaryTree.NearestSearchWrapper(node, val);
        }

        /// <summary>
        /// Поиск ближайшего назад
        /// </summary>
        /// <param name="val">Число, для которого нужно найти ближайшее назад</param>
        /// <returns>Ближайшее предыдущее число</returns>
        public int NearestPrev(int val)
        {
            if (binaryTree.Count == 0)
                return val;

            return binaryTree.NearestPrevSearchWrapper(node, val);
        }

    }
}
