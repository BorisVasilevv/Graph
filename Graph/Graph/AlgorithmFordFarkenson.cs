﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Graph
{
    public class AlgorithmFordFarkenson
    {

        static bool Bfs(int[,] rGraph, int s, int t, int[] parent)
        {
            int V = (int)Math.Sqrt(rGraph.Length);
            // Create a visited array and mark
            // all vertices as not visited
            bool[] visited = new bool[V];
            for (int i = 0; i < V; ++i)
                visited[i] = false;

            // Create a queue, enqueue source vertex and mark
            // source vertex as visited
            List<int> queue = new List<int>();
            queue.Add(s);
            visited[s] = true;
            parent[s] = -1;

            // Standard BFS Loop
            while (queue.Count != 0)
            {
                int u = queue[0];
                queue.RemoveAt(0);

                for (int v = 0; v < V; v++)
                {
                    if (visited[v] == false
                        && rGraph[u, v] > 0)
                    {
                        // If we find a connection to the sink
                        // node, then there is no point in BFS
                        // anymore We just have to set its parent
                        // and can return true
                        if (v == t)
                        {
                            parent[v] = u;
                            return true;
                        }
                        queue.Add(v);
                        parent[v] = u;
                        visited[v] = true;
                    }
                }
            }
            // We didn't reach sink in BFS starting from source,
            // so return false
            return false;
        }

        // Returns the maximum flow
        // from s to t in the given graph
        static int FordFulkerson(ref int[,] graph, int s, int t)
        {
            List<int> logVert = new List<int>();

            Logger logger = new Logger();

            int V = (int)Math.Sqrt(graph.Length);

            int u, v;

            int[,] rGraph = new int[V, V];

            logger.AddLine("создаем еще один двумерный массив.");

            for (u = 0; u < V; u++)
                for (v = 0; v < V; v++)
                    rGraph[u, v] = graph[u, v];

            logger.AddLine("копируем первый двумерный массив во второй.");

            int[] parent = new int[V];

            //logger.AddLine("создаем массив, где индекс - вершина, а значение - откуда можем попасть");

            int max_flow = 0; // There is no flow initially
            logger.AddLine("для каждого  существующего пути будем искать максимальный поток.");
            logger.AddLine("создаем переменную максимального потока для выбранного пути.");
            // Augment the flow while there is path from source
            // to sink
            while (Bfs(rGraph, s, t, parent))
            {
                // Find minimum residual capacity of the edhes
                // along the path filled by BFS. Or we can say
                // find the maximum flow through the path found.
                int path_flow = int.MaxValue;
                logger.AddLine("идем по одному из путей и для каждого ребра проверяем, является\nли его пропускная способность минимальной на нашем пути.");
                logger.AddLine("если она минимальная, то присваиваем это значение нашей переменной максимального потока для выбранного пути.");
                for (v = t; v != s; v = parent[v])
                {
                    u = parent[v];
                    path_flow
                        = Math.Min(path_flow, rGraph[u, v]);
                }
                logger.AddLine("максимальный поток на пути из вершин:");
                // update residual capacities of the edges and
                // reverse edges along the path
                for (v = t; v != s; v = parent[v])
                {
                    u = parent[v];
                    rGraph[u, v] -= path_flow;
                    //rGraph[v, u] += path_flow;
                    if (!logVert.Contains(v))
                        logVert.Add(v);
                    if (!logVert.Contains(u))
                        logVert.Add(u);
                }
                for (int i = logVert.Count - 1; i >= 0; i--)
                {
                    logger.AddLine($"{logVert[i] + 1}");
                }
                logVert.Clear();
                logger.AddLine($"равен: {path_flow}.");
                // Add path flow to overall flow
                max_flow += path_flow;
            }

            for (u = 0; u < V; u++)
                for (v = 0; v < V; v++)
                    graph[u, v] -= rGraph[u, v];


            // Return the overall flow
            logger.AddLine($"складываем наши максимальные потоки всех существующих путей из вершины {s + 1} в вершину {t + 1} и получаем: {max_flow}.\n");
            return max_flow;
        }

        public static void FordFarkensonAlgorithm(Vertice start, Vertice end, MyGraph mainGraph)
        {

            foreach(Connection conn in mainGraph.Connections)
            {
                conn.BlockText.MouseDown -= ChangeDataTool.TextBlock_MouseDown;
            }

            string[][] stringsVertices = new string[mainGraph.AllVertices.Count][];
            for (int i = 0; i < mainGraph.AllVertices.Count; i++)
            {
                string[] arrayConnection = new string[mainGraph.AllVertices.Count];
                for (int j = 0; j < mainGraph.AllVertices.Count; j++)
                {
                    Connection checkConnection = null;
                    if (i == j) arrayConnection[j] = "0";
                    else checkConnection = Connection.SearchConnection(
                        Vertice.SearchVertice(mainGraph.AllVertices[i].Id, mainGraph.AllVertices),
                        Vertice.SearchVertice(mainGraph.AllVertices[j].Id, mainGraph.AllVertices),
                        mainGraph.Connections);

                    if (checkConnection != null) arrayConnection[j] = checkConnection.Length.ToString();
                    else arrayConnection[j] = "0";
                }
                stringsVertices[i] = arrayConnection;
            }


            int[,] graph = new int[stringsVertices.Length, stringsVertices.Length];
            for (int i = 0; i < stringsVertices.Length; i++)
            {
                for (int j = 0; j < stringsVertices.Length; j++)
                {
                    graph[i, j] = Int32.Parse(stringsVertices[i][j]);
                }
            }

            Logger logger = new Logger();
            logger.AddLine("Начат алгоритм поиска максимального потока.");
            logger.AddLine("выбираем вершину стока и вершину истока.");
            logger.AddLine("копируем наш граф в двумерный массив.");

            TextBlock textBlock = DrawHelper.TextBlock;

            textBlock.Text = $"The max flow beetwen\n{start.NameTextBlock.Text} and {end.NameTextBlock.Text} is {FordFulkerson(ref graph, start.Id, end.Id)}";
            MainWindow.MainCanvas.Children.Add(textBlock);
            Canvas.SetZIndex(textBlock, 20);
            Rectangle answerRect = DrawHelper.AnswerRect;
            MainWindow.MainCanvas.Children.Add(answerRect);
            Canvas.SetZIndex(answerRect, 18);
            Canvas.SetTop(answerRect, MainWindow.MainCanvas.Height - answerRect.Height);
            Canvas.SetLeft(answerRect, MainWindow.MainCanvas.Width - answerRect.Width);
            MainWindow.MainCanvas.Children.Add(DrawHelper.BtnReturn);
            DrawHelper.BtnReturn.Click += BtnReturn_Click; ;
            Canvas.SetZIndex(DrawHelper.BtnReturn, 20);

            


            foreach (Connection conn in mainGraph.Connections)
            {

                int a = graph[conn.Vertice1.Id, conn.Vertice2.Id];
                int b = graph[conn.Vertice2.Id, conn.Vertice1.Id];

                conn.BlockText.Text = $"{Math.Max(a, b)}/{conn.BlockText.Text}";
            }
        }

        private static void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.BtnReturn_Click(sender, e);
            MainWindow.IsUserCanUseButtons = true;

            foreach (Connection connection in MainWindow.MainGraph.Connections)
            {
                connection.BlockText.Text = connection.Length.ToString();
                connection.BlockText.MouseDown += ChangeDataTool.TextBlock_MouseDown;
            }                                    
        }
    }
}
