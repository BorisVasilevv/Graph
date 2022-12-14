using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class PrimAlghoritm
    {
        public static List<Connection> AlgorithmByPrim(MyGraph graph)
        {

            List<Connection> resultConn = new List<Connection>();
            //неиспользованные ребра
            List<Connection> notUsedE = new List<Connection>(graph.Connections);

            //использованные вершины
            List<Vertice> usedV = new List<Vertice>();
            //неиспользованные вершины
            List<Vertice> notUsedV = new List<Vertice>(graph.AllVertices);

            int numberV=notUsedV.Count;
            //выбираем случайную начальную вершину
            Random rand = new Random();
            int randomNumber =rand.Next(0, numberV);
            Vertice randVertice = notUsedV[randomNumber];
            usedV.Add(randVertice);
            notUsedV.Remove(randVertice);

            while (notUsedV.Count > 0)
            {
                int minE = -1; //номер наименьшего ребра
                               //поиск наименьшего ребра
                for (int i = 0; i < notUsedE.Count; i++)
                {
                    if ((usedV.IndexOf(notUsedE[i].Vertice1) != -1) && (notUsedV.IndexOf(notUsedE[i].Vertice2) != -1) ||
                        (usedV.IndexOf(notUsedE[i].Vertice2) != -1) && (notUsedV.IndexOf(notUsedE[i].Vertice1) != -1))
                    {
                        if (minE != -1)
                        {
                            if (notUsedE[i].Length < notUsedE[minE].Length)
                                minE = i; 
                        }
                        else
                        {
                            minE = i;
                        }
                    }
                }

                //заносим новую вершину в список использованных и удаляем ее из списка неиспользованных
                if (usedV.IndexOf(notUsedE[minE].Vertice1) != -1)
                {
                    usedV.Add(notUsedE[minE].Vertice2);
                    notUsedV.Remove(notUsedE[minE].Vertice2);
                }
                else
                {
                    usedV.Add(notUsedE[minE].Vertice1);
                    notUsedV.Remove(notUsedE[minE].Vertice1);
                }
                //заносим новое ребро в дерево и удаляем его из списка неиспользованных
                resultConn.Add(notUsedE[minE]);
                notUsedE.RemoveAt(minE);
            }

            //Animize
            return resultConn;
        }
    }
}
