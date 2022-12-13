using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Graph
{
    public class Traversal
    {

        public static void DFS(List<Vertice> LV, List<Connection> LC)//я хз, надо тестить
        {
            Stack<Vertice> vertices = new Stack<Vertice>();
            vertices.Push(LV[0]);
            List<Vertice> used = new List<Vertice>();
            Vertice v = vertices.Pop();
            used.Add(v);

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
                        vertices.Push(con.Vertice2);
                    }

                }
                used.Add(vertices.Pop());
            } while (vertices.Count!=0);
            AnimaztionPainter animaztionPainter = new AnimaztionPainter(used);
            animaztionPainter.ShowAnimation();
            //return used
        }

        public static void BFS(List<Vertice> LV, List<Connection> LC)//аналогично - тестить
        {
            Queue<Vertice> qVertice = new Queue<Vertice>();
            qVertice.Enqueue(LV[0]);
            List<Vertice> next = new List<Vertice>();
            next.Add(LV[0]);
            while (qVertice.Count!=0)
            {
                Vertice tmp = qVertice.Dequeue();
                foreach (Connection con in LC)
                {
                    Vertice compareVerice;
                    if(qVertice.Count==0)
                    {
                        compareVerice = tmp;
                    }
                    else
                    {
                        compareVerice = qVertice.Peek();
                    }

                    if (con.Vertice1.Id == compareVerice.Id && !next.Contains(con.Vertice2) && !qVertice.Contains(con.Vertice2))
                    {
                        next.Add(con.Vertice2);
                        qVertice.Enqueue(con.Vertice2);
                    }

                }
            }
            AnimaztionPainter animaztionPainter = new AnimaztionPainter(next);
            animaztionPainter.ShowAnimation();
            //return next;
        }

    }
}
