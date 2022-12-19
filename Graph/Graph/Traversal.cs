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
            logger.AddText("Начат алгоритм обхода в глубину");

            AnimaztionPainter animaztionPainter = new AnimaztionPainter(AnimaztionPainter.AlgorithmType.Traversal);
            Stack<Vertice> stack = new Stack<Vertice>();
            List<Vertice> notVisitedV = new List<Vertice>(graph.AllVertices);
            notVisitedV.Reverse();
            logger.AddText($"Добавляем в стек вершину с номером {graph.AllVertices[0].Id+1}");
            stack.Push(graph.AllVertices[0]);
   
            while (stack.Count != 0)
            {
                Vertice vert = stack.Pop();
                if(!notVisitedV.Contains(vert))
                {
                    continue;
                }

                animaztionPainter.Shapes.Add(vert.Rect);
                
                foreach (Vertice vertice in notVisitedV)
                {
                    if (Connection.SearchConnection(vertice, vert, graph.Connections) != null)
                    {
                        logger.AddText($"Добавляем в стек вершину с номером {vertice.Id + 1}");
                        stack.Push(vertice);

                    }
                }
                logger.AddText($"Удаляем из стека вершину с номером {vert.Id + 1}");
                notVisitedV.Remove(vert);
                if (!animaztionPainter.Shapes.Contains(vert.Rect)) animaztionPainter.Shapes.Add(vert.Rect);
            }

            animaztionPainter.ShowAnimation();
        }

        public static void WidthTraversal(MyGraph graph)//аналогично - тестить
        {

            AnimaztionPainter animaztionPainter = new AnimaztionPainter(AnimaztionPainter.AlgorithmType.Traversal);

            List<Vertice> visitedV = new List<Vertice>();
            List<Vertice> notVisitedV = new List<Vertice>(graph.AllVertices);
            int vertCount = notVisitedV.Count;
            visitedV.Add(graph.AllVertices[0]);
            notVisitedV.Remove(graph.AllVertices[0]);
            animaztionPainter.Shapes.Add(graph.AllVertices[0].Rect);
            Vertice verticeNow = graph.AllVertices[0];
            Queue<Vertice> qVertice = new Queue<Vertice>();
            while (visitedV.Count != vertCount)
            {

                List<Connection> connections = new List<Connection>();
                foreach (Vertice vert in notVisitedV)
                {
                    Connection conn = Connection.SearchConnection(verticeNow, vert, graph.Connections);
                    if (conn != null && !(animaztionPainter.Shapes.Contains(vert.Rect) && animaztionPainter.Shapes.Contains(verticeNow.Rect)))
                    {
                        connections.Add(conn);
                        if (!animaztionPainter.Shapes.Contains(conn.Line)) animaztionPainter.Shapes.Add(conn.Line);
                        if (!animaztionPainter.Shapes.Contains(vert.Rect)) animaztionPainter.Shapes.Add(vert.Rect);
                        if (!visitedV.Contains(vert)) visitedV.Add(vert);
                        qVertice.Enqueue(vert);
                    }
                }

                Vertice vertice = qVertice.Dequeue();
                verticeNow = vertice;
                notVisitedV.Remove(vertice);
            }



            animaztionPainter.ShowAnimation();
        }

    }
}
