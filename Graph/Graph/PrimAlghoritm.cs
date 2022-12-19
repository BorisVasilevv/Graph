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
            AnimaztionPainter animaztionPainter = new AnimaztionPainter(AnimaztionPainter.AlgorithmType.Prim);

            List<Connection> usedE = new List<Connection>();
            //неиспользованные ребра
            List<Connection> notUsedE = new List<Connection>(graph.Connections);

            //использованные вершины
            List<Vertice> usedV = new List<Vertice>();
            //неиспользованные вершины
            List<Vertice> notUsedV = new List<Vertice>(graph.AllVertices);

            Logger logger = new Logger();

            int numberV=notUsedV.Count;

            
            //выбираем случайную начальную вершину
            Random rand = new Random();
            int randomNumber =rand.Next(0, numberV);
            Vertice randVertice = notUsedV[randomNumber];
            logger.AddLine($"Выбираем случайную начальную вершину\nНа этот раз это вершина {randVertice.NameTextBlock.Text}");
            usedV.Add(randVertice);
            animaztionPainter.Shapes.Add(randVertice.Rect);
            notUsedV.Remove(randVertice);
            logger.AddLine($"Удалим вершину {randVertice.NameTextBlock.Text} из списка непосещённых");
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

                animaztionPainter.Shapes.Add(notUsedE[minE].Line);

                //заносим новую вершину в список использованных и удаляем ее из списка неиспользованных
                if (usedV.IndexOf(notUsedE[minE].Vertice1) != -1)
                {
                    usedV.Add(notUsedE[minE].Vertice2);
                    animaztionPainter.Shapes.Add(notUsedE[minE].Vertice2.Rect);
                    notUsedV.Remove(notUsedE[minE].Vertice2);
                }
                else
                {
                    usedV.Add(notUsedE[minE].Vertice1);
                    animaztionPainter.Shapes.Add(notUsedE[minE].Vertice1.Rect);
                    
                    notUsedV.Remove(notUsedE[minE].Vertice1);
                }
                //заносим новое ребро в дерево и удаляем его из списка неиспользованных
                usedE.Add(notUsedE[minE]);
                notUsedE.RemoveAt(minE);
            }

            animaztionPainter.ShowAnimation();
            //Animize
            MainWindow.IsUserCanUseButtons = true;
            return usedE;
        }
    }
}
