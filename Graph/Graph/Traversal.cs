using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace Graph
{
    public class Traversal
    {
        static Graph MainGraph= MainWindow.MainGraph;
        public static void DFS(List<Vertice> LV, List<Connection> LC)//я хз, надо тестить
        {
            Stack<Vertice> vertices = new Stack<Vertice>();
            vertices.Push(LV[0]);
            List<Vertice> used = new List<Vertice>();
            List<Shape> shapes= new List<Shape>();
            Vertice v = vertices.Pop();
            used.Add(v);
            shapes.Add(v.Rect);
            do
            {
                foreach (Connection con in LC)
                {
                    Vertice vertCompare;

                    if(vertices.Count==0)
                    {
                        vertCompare = v;
                    }
                    else
                    {
                        vertCompare = vertices.Peek();
                    }
                    if (con.Vertice1.Id == vertCompare.Id && !vertices.Contains(con.Vertice2)  && !used.Contains(con.Vertice2))
                    {
                        if (!shapes.Contains(con.Vertice1.Rect))  shapes.Add(con.Vertice1.Rect);
                        if (!shapes.Contains(con.Line))  shapes.Add(con.Line);
                        
                        vertices.Push(con.Vertice2);
                    }

                }
                Vertice vert=vertices.Pop();
                used.Add(vert);
                if(!shapes.Contains(vert.Rect)) shapes.Add(vert.Rect);
            } while (vertices.Count!=0);

            AnimaztionPainter animaztionPainter = new AnimaztionPainter(AnimaztionPainter.AlgorithmType.Traversal);
            animaztionPainter.Shapes=shapes;
            animaztionPainter.ShowAnimation();
        }

        public static void BFS(List<Vertice> LV, List<Connection> LC)//аналогично - тестить
        {
            List<Shape> shapes = new List<Shape>();
            List<Vertice> visitedV = new List<Vertice>();
            List<Vertice> notVisitedV = new List<Vertice>(LV);
            int vertCount = notVisitedV.Count;
            visitedV.Add(LV[0]);
            notVisitedV.Remove(LV[0]);
            shapes.Add(LV[0].Rect);
            Vertice verticeNow = LV[0];
            Queue<Vertice> qVertice = new Queue<Vertice>();
            while (visitedV.Count!=vertCount)
            {

                List<Connection> connections = new List<Connection>();
                foreach(Vertice vert in notVisitedV)
                {
                    Connection conn = Connection.SearchConnection(verticeNow, vert,MainGraph.Connections);
                    if (conn != null&&!(shapes.Contains(vert.Rect)&& shapes.Contains(verticeNow.Rect)))
                    {
                        connections.Add(conn);
                        if(!shapes.Contains(conn.Line)) shapes.Add(conn.Line);
                        if (!shapes.Contains(vert.Rect)) shapes.Add(vert.Rect);
                        if (!visitedV.Contains(vert)) visitedV.Add(vert);
                        qVertice.Enqueue(vert);
                    }                  
                }

                Vertice vertice = qVertice.Dequeue();
                verticeNow = vertice;               
                notVisitedV.Remove(vertice);
            }

            AnimaztionPainter animaztionPainter = new AnimaztionPainter(AnimaztionPainter.AlgorithmType.Traversal);
            animaztionPainter.Shapes = shapes;
            animaztionPainter.ShowAnimation();
        }

    }
}
