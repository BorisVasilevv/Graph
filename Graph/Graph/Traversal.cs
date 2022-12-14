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
        static MyGraph MainGraph= MainWindow.MainGraph;
        public static void DFS(List<Vertice> LV, List<Connection> LC)//я хз, надо тестить
        {

            AnimaztionPainter animaztionPainter = new AnimaztionPainter(AnimaztionPainter.AlgorithmType.Traversal);

            Stack<Vertice> vertices = new Stack<Vertice>();
            vertices.Push(LV[0]);
            List<Vertice> used = new List<Vertice>();

            Vertice v = vertices.Pop();
            used.Add(v);
            animaztionPainter.Shapes.Add(v.Rect);
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
                        if (!animaztionPainter.Shapes.Contains(con.Vertice1.Rect)) animaztionPainter.Shapes.Add(con.Vertice1.Rect);
                        if (!animaztionPainter.Shapes.Contains(con.Line)) animaztionPainter.Shapes.Add(con.Line);
                        
                        vertices.Push(con.Vertice2);
                    }

                }
                Vertice vert=vertices.Pop();
                used.Add(vert);
                if(!animaztionPainter.Shapes.Contains(vert.Rect)) animaztionPainter.Shapes.Add(vert.Rect);
            } while (vertices.Count!=0);

  
           
            animaztionPainter.ShowAnimation();
        }

        public static void BFS(List<Vertice> LV, List<Connection> LC)//аналогично - тестить
        {

            AnimaztionPainter animaztionPainter = new AnimaztionPainter(AnimaztionPainter.AlgorithmType.Traversal);

            List<Vertice> visitedV = new List<Vertice>();
            List<Vertice> notVisitedV = new List<Vertice>(LV);
            int vertCount = notVisitedV.Count;
            visitedV.Add(LV[0]);
            notVisitedV.Remove(LV[0]);
            animaztionPainter.Shapes.Add(LV[0].Rect);
            Vertice verticeNow = LV[0];
            Queue<Vertice> qVertice = new Queue<Vertice>();
            while (visitedV.Count!=vertCount)
            {

                List<Connection> connections = new List<Connection>();
                foreach(Vertice vert in notVisitedV)
                {
                    Connection conn = Connection.SearchConnection(verticeNow, vert,MainGraph.Connections);
                    if (conn != null&&!(animaztionPainter.Shapes.Contains(vert.Rect)&& animaztionPainter.Shapes.Contains(verticeNow.Rect)))
                    {
                        connections.Add(conn);
                        if(!animaztionPainter.Shapes.Contains(conn.Line)) animaztionPainter.Shapes.Add(conn.Line);
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
