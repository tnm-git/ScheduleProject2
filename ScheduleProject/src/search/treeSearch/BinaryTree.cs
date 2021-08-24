using System;

namespace ScheduleProject.search
{
    /// <summary>
    /// Бинарное дерево интервалов с возможностью реализации балансировки по АВЛ
    /// Реализует операцию вставки, вставки с врщанием для баланс (АВЛ), поиска ближайшего вперед,
    /// поиска ближайшего назад
    /// </summary>
    class BinaryTree
    {
        private int closestKeyDifference;
        private int closestKey = -1;

        /// <summary>
        /// Число элементов дерева
        /// </summary>
        public int Count = 0;

        /// <summary>
        /// Получение числа уровней
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Число уровней</returns>
        private int Height(Node node)
        {
            if (node == null)
                return 0;

            return node.Height;
        }

        private int Max(int a, int b)
        {
            return (a > b) ? a : b;
        }

        /// <summary>
        /// Создание нового узла
        /// </summary>
        /// <param name="interval">Ключ</param>
        /// <returns>Новый узел</returns>
        private Node NewNode(Interval interval)
        {
            Node node = new Node();
            node.Key = new Interval(interval);
            node.Left = node.Right = null;
            Count++;
            return node;
        }

        /// <summary>
        /// Реализует правый поворот поддерева
        /// </summary>
        /// <param name="y">Корень, относительно которого производится поворот</param>
        /// <returns>Новый корень поддерева</returns>
        private Node RightRotate(Node y)
        {
            Node x = y.Left;
            Node T2 = x.Right;

            x.Right = y;
            y.Left = T2;

            // Обновляем высоту
            y.Height = Max(Height(y.Left), Height(y.Right)) + 1;
            x.Height = Max(Height(x.Left), Height(x.Right)) + 1;

            return x;
        }

        /// <summary>
        /// Реализует левый поворот поддерева
        /// </summary>
        /// <param name="x">Корень, относительно которого производится поворот</param>
        /// <returns>Новый корень поддерева</returns>
        private Node LeftRotate(Node x)
        {
            Node y = x.Right;
            Node T2 = y.Left;

            y.Left = x;
            x.Right = T2;

            // Обновляем высоту
            x.Height = Max(Height(x.Left), Height(x.Right)) + 1;
            y.Height = Max(Height(y.Left), Height(y.Right)) + 1;

            return y;
        }

        /// <summary>
        /// Возвращает баланс дерева
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>Разница высот левого и правого поддеревьев</returns>
        private int GetBalance(Node node)
        {
            if (node == null)
                return 0;

            return Height(node.Left) - Height(node.Right);
        }

        /// <summary>
        /// Вставка элемента в дерево
        /// </summary>
        /// <param name="root">Корень</param>
        /// <param name="interval">Ключ</param>
        /// <returns>Дерево</returns>
        public Node Insert(Node root, Interval interval)
        {
            // Дерево пустое
            if (root == null)
                return NewNode(interval);

            // Помещаем в левую ветвь, если минимум ключа меньше
            if (interval.Low < root.Key.Low)
            {
                root.Left = Insert(root.Left, interval);
            }
            else // Иначе помещаем в правую ветвь
            { 
                root.Right = Insert(root.Right, interval);
            }

            return root;
        }

        /// <summary>
        /// Вставка элемента в дерево АВЛ
        /// </summary>
        /// <param name="root">Корень</param>
        /// <param name="interval">Ключ</param>
        /// <returns>Дерево АВЛ</returns>
        public Node InsertAVL(Node root, Interval interval)
        {
            // 1 Стандартная процедура вставки

            // Дерево пустое
            if (root == null)
                return NewNode(interval);

            // Помещаем в левую ветвь, если минимум ключа меньше
            if (interval.Low < root.Key.Low)
            {
                root.Left = Insert(root.Left, interval);
            }
            else // Иначе помещаем в правую ветвь
            {
                root.Right = Insert(root.Right, interval);
            }

            // 2 Обновляем высоту относительно корня
            root.Height = 1 + Max(Height(root.Left), Height(root.Right));

            // 3 Получаем баланс дерева
            int balance = GetBalance(root);

            // левая граница ключа
            int low = interval.Low;

            if (balance > 1 && low < root.Left.Key.Low)
                return RightRotate(root);

            if (balance < -1 && low > root.Right.Key.Low)
                return LeftRotate(root);

            if (balance > 1 && low > root.Left.Key.Low)
            {
                root.Left = LeftRotate(root.Left);
                return RightRotate(root);
            }

            if (balance < -1 && low < root.Right.Key.Low)
            {
                root.Right = RightRotate(root.Right);
                return LeftRotate(root);
            }

            return root;
        }

        /// <summary>
        /// Центрированный обход дерева
        /// </summary>
        /// <param name="root">Корень</param>
        public void InOrder(Node root)
        {
            if (root == null)
                return;

            InOrder(root.Left);

            Console.WriteLine($"[{root.Key.Low}, {root.Key.High}]");

            InOrder(root.Right);
        }

        /// <summary>
        /// Возвращает true, если интервал содержит заданную величину
        /// </summary>
        /// <param name="interval">Интервал</param>
        /// <param name="val">Величина</param>
        /// <returns>true, false</returns>
        private bool IsItContains(Interval interval, int val)
        {
            if (val >= interval.Low && val <= interval.High)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Поиск ближайшего числа в дереве
        /// </summary>
        /// <param name="root">Текущий узел</param>
        /// <param name="interval">Ключ</param>
        /// <returns>Значение</returns>
        private int NearestSearch(Node root, Interval interval)
        {
            if (root == null)
            {
                return closestKey;
            }

            if (IsItContains(root.Key, interval.Low))
            {
                return interval.Low;
            }

            int diff = root.Key.Low - interval.Low;

            if (root.Key.Low > interval.Low && diff < closestKeyDifference)
            {
                closestKeyDifference = diff;
                closestKey = root.Key.Low;
            }

            if (interval.Low < root.Key.Low)
            {
                return NearestSearch(root.Left, interval);
            }
            return NearestSearch(root.Right, interval);
        }

        /// <summary>
        /// Поиск ближайшего предыдущего числа в дереве
        /// </summary>
        /// <param name="root">Текущий узел</param>
        /// <param name="interval">Ключ</param>
        /// <returns>Значение</returns>
        private int NearestPrevSearch(Node root, Interval interval)
        {
            if (root == null)
            {
                return closestKey;
            }

            if (IsItContains(root.Key, interval.High))
            {
                return interval.High;
            }

            int diff = interval.High - root.Key.High;

            if (interval.High > root.Key.High && diff < closestKeyDifference)
            {
                closestKeyDifference = diff;
                closestKey = root.Key.High;
            }

            if (interval.Low < root.Key.Low)
            {
                return NearestPrevSearch(root.Left, interval);
            }
            return NearestPrevSearch(root.Right, interval);
        }

        /// <summary>
        /// Поиск ближайшего числа в дереве
        /// </summary>
        /// <param name="node">Текущий узел</param>
        /// <param name="val">Значение</param>
        /// <returns>Ближайшее</returns>
        public int NearestSearchWrapper(Node node, int val)
        {
            closestKeyDifference = int.MaxValue;
            closestKey = -1;
            return NearestSearch(node, new Interval(val));
        }

        /// <summary>
        /// Поиск ближайшего предыдущего числа в дереве
        /// </summary>
        /// <param name="node">Текущий узел</param>
        /// <param name="val">Значение</param>
        /// <returns>Ближайшее предыдущее значение</returns>
        public int NearestPrevSearchWrapper(Node node, int val)
        {
            closestKeyDifference = int.MaxValue;
            closestKey = -1;
            return NearestPrevSearch(node, new Interval(val));
        }
    }
}
