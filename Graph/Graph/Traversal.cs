using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace Graph
{
    public class Traversal
    {
        public static void DepthTraversal(MyGraph graph)//я хз, надо тестить
        {

            Logger logger = new Logger();
            logger.AddLine("Начат алгоритм обхода в глубину");
            AnimaztionPainter animaztionPainter = new AnimaztionPainter(AnimaztionPainter.AlgorithmType.Traversal);
            Stack<Vertice> stack = new Stack<Vertice>();
            List<Vertice> notVisitedV = new List<Vertice>(graph.AllVertices);
            notVisitedV.Reverse();
            logger.AddLine($"В списке непосещённых вершин находятся все вершины");
            logger.AddLine($"Добавляем в стек вершину с номером {graph.AllVertices[0].NameTextBlock.Text}");

            stack.Push(graph.AllVertices[0]);
   
            while (stack.Count != 0)
            {
                Vertice vert = stack.Pop();
                logger.AddLine($"Достаём из стека вершину с номером {vert.NameTextBlock.Text}");
                if (!notVisitedV.Contains(vert))
                {
                    continue;
                }

                animaztionPainter.Shapes.Add(vert.Rect);
                logger.AddLine($"Обходим все соединения связывающие вершину {vert.NameTextBlock.Text}\nДобавляем в стек все вершины из списка непосещеных связанных с {vert.NameTextBlock.Text}");
                foreach (Vertice vertice in notVisitedV)
                {
                    if (Connection.SearchConnection(vertice, vert, graph.Connections) != null)
                    {
                        logger.AddLine($"Добавляем в стек вершину с номером {vertice.NameTextBlock.Text}");
                        stack.Push(vertice);

                    }
                }
                logger.AddLine($"Удаляем из списка непосещённых вершин вершину с номером {vert.NameTextBlock.Text}");
                notVisitedV.Remove(vert);
                if (!animaztionPainter.Shapes.Contains(vert.Rect)) animaztionPainter.Shapes.Add(vert.Rect);
            }
            logger.AddLine("Алгорит обхода в глубину завершён");
            logger.AddLine(String.Empty);


            Logger.ShowAllLogToUser(MainWindow.MainCanvas, MainWindow.MainGrid);
            animaztionPainter.ShowAnimation();
        }

        public static void WidthTraversal(MyGraph graph)//аналогично - тестить
        {
            Logger logger = new Logger();
            AnimaztionPainter animaztionPainter = new AnimaztionPainter(AnimaztionPainter.AlgorithmType.Traversal);
            logger.AddLine("Начат алгоритм обхода в глубину");
            logger.AddLine("В списке непосещённых вершин находятся все вершины");
            List<Vertice> visitedV = new List<Vertice>();
            List<Vertice> notVisitedV = new List<Vertice>(graph.AllVertices);
            int vertCount = notVisitedV.Count;
            logger.AddLine($"Удаляем из списка непосещённых вершин {graph.AllVertices[0].NameTextBlock.Text}");
            logger.AddLine("так как с неё начинается обход");
            visitedV.Add(graph.AllVertices[0]);
            notVisitedV.Remove(graph.AllVertices[0]);
            animaztionPainter.Shapes.Add(graph.AllVertices[0].Rect);
            Vertice verticeNow = graph.AllVertices[0];
            Queue<Vertice> qVertice = new Queue<Vertice>();
            while (visitedV.Count != vertCount)
            {
                logger.AddLine($"Проходим по всем соединениям связывающими {verticeNow.NameTextBlock.Text}\nДобавляем в очередь все вершины из списка непосещеных связанных с {verticeNow.NameTextBlock.Text}");
                //List<Connection> connections = new List<Connection>();
                foreach (Vertice vert in notVisitedV)
                {
                    Connection conn = Connection.SearchConnection(verticeNow, vert, graph.Connections);
                    if (conn != null && !(animaztionPainter.Shapes.Contains(vert.Rect) && animaztionPainter.Shapes.Contains(verticeNow.Rect)))
                    {
                        //connections.Add(conn);
                        if (!animaztionPainter.Shapes.Contains(conn.Line)) animaztionPainter.Shapes.Add(conn.Line);
                        if (!animaztionPainter.Shapes.Contains(vert.Rect)) animaztionPainter.Shapes.Add(vert.Rect);
                        if (!visitedV.Contains(vert)) visitedV.Add(vert);
                        logger.AddLine($"Добавляем в очередь вершину {vert.NameTextBlock.Text}");
                        qVertice.Enqueue(vert);
                    }
                }

                Vertice vertice = qVertice.Dequeue();
                verticeNow = vertice;
                logger.AddLine($"Добавляем в очередь вершину {vertice.NameTextBlock.Text}\nи удаляем ёё из списка непосещённых вершин");
                notVisitedV.Remove(vertice);
            }

            logger.AddLine("Алгорит обхода в ширину завершён");
            logger.AddLine(String.Empty);

            Logger.ShowAllLogToUser(MainWindow.MainCanvas, MainWindow.MainGrid);
            animaztionPainter.ShowAnimation();
        }

    }
}
