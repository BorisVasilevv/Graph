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

            //

            //Stack<Vertice> vertices = new Stack<Vertice>();
            //vertices.Push(LV[0]);
            //List<Vertice> used = new List<Vertice>();

            //Vertice v = vertices.Pop();
            //used.Add(v);
            //animaztionPainter.Shapes.Add(v.Rect);
            //do
            //{
            //    foreach (Connection con in LC)
            //    {
            //        Vertice vertCompare;

            //        if(vertices.Count==0)
            //        {
            //            vertCompare = v;
            //        }
            //        else
            //        {
            //            vertCompare = vertices.Peek();
            //        }
            //        if (con.Vertice1.Id == vertCompare.Id && !vertices.Contains(con.Vertice2)  && !used.Contains(con.Vertice2))
            //        {
            //            if (!animaztionPainter.Shapes.Contains(con.Vertice1.Rect)) animaztionPainter.Shapes.Add(con.Vertice1.Rect);
            //            if (!animaztionPainter.Shapes.Contains(con.Line)) animaztionPainter.Shapes.Add(con.Line);

            //            vertices.Push(con.Vertice2);
            //        }

            //    }
            //    Vertice vert=vertices.Pop();
            //    used.Add(vert);
            //    if(!animaztionPainter.Shapes.Contains(vert.Rect)) animaztionPainter.Shapes.Add(vert.Rect);
            //} while (vertices.Count!=0);

            //animaztionPainter.ShowAnimation();

            AnimaztionPainter animaztionPainter = new AnimaztionPainter(AnimaztionPainter.AlgorithmType.Traversal);
            Stack<Vertice> stack = new Stack<Vertice>();
            List<Vertice> notVisitedV = new List<Vertice>(graph.AllVertices);
            notVisitedV.Reverse();

            stack.Push(graph.AllVertices[0]);
   
            while (stack.Count != 0)
            {
                Vertice vert = stack.Pop();
                if(!notVisitedV.Contains(vert))
                {
                    continue;
                }

                animaztionPainter.Shapes.Add(vert.Rect);
                //foreach (Connection connection in graph.Connections)
                //{
                //    if(connection.Vertice1== vert&& notVisitedV.Contains(connection.Vertice2))
                //    {
                //        //if (!animaztionPainter.Shapes.Contains(connection.Line)) animaztionPainter.Shapes.Add(connection.Line);

                //        stack.Push(connection.Vertice2);
                //        notVisitedV.Remove(connection.Vertice2);
                //    }
                //    else if(connection.Vertice2==vert&& notVisitedV.Contains(connection.Vertice1))
                //    {
                //        //if (!animaztionPainter.Shapes.Contains(connection.Line)) animaztionPainter.Shapes.Add(connection.Line);

                //        stack.Push(connection.Vertice1);
                //        notVisitedV.Remove(connection.Vertice1);

                //    }

                //}



                foreach (Vertice vertice in notVisitedV)
                {
                    if (Connection.SearchConnection(vertice, vert, graph.Connections) != null)
                    {

                        stack.Push(vertice);

                    }
                }
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
