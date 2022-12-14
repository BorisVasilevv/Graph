using System;
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
        static int FordFulkerson(int[,] graph, int s, int t)
        {
            int V = (int)Math.Sqrt(graph.Length);

            int u, v;

            int[,] rGraph = new int[V, V];

            for (u = 0; u < V; u++)
                for (v = 0; v < V; v++)
                    rGraph[u, v] = graph[u, v];

            int[] parent = new int[V];

            int max_flow = 0; // There is no flow initially

            // Augment the flow while there is path from source
            // to sink
            while (Bfs(rGraph, s, t, parent))
            {
                // Find minimum residual capacity of the edhes
                // along the path filled by BFS. Or we can say
                // find the maximum flow through the path found.
                int path_flow = int.MaxValue;
                for (v = t; v != s; v = parent[v])
                {
                    u = parent[v];
                    path_flow
                        = Math.Min(path_flow, rGraph[u, v]);
                }

                // update residual capacities of the edges and
                // reverse edges along the path
                for (v = t; v != s; v = parent[v])
                {
                    u = parent[v];
                    rGraph[u, v] -= path_flow;
                    rGraph[v, u] += path_flow;
                }

                // Add path flow to overall flow
                max_flow += path_flow;
            }

            // Return the overall flow
            return max_flow;
        }

        public static void Algorithm(Vertice start, Vertice end, MyGraph mainGraph)
        {
            string path = MainWindow.FullFileNamePath;
            string[][] graph2 = File.ReadAllLines(path).Select(x => x.Split(";")).ToArray();

            int[,] graph = new int[graph2.Length, graph2.Length];
            for (int i = 0; i < graph2.Length; i++)
            {
                for (int j = 0; j < graph2.Length; j++)
                {
                    graph[i, j] = Int32.Parse(graph2[i][j]);
                }
            }

            TextBlock textBlock = DrawHelper.TextBlock;
            textBlock.Text = $"The maximum possible\nflow is {FordFulkerson(graph, start.Id, end.Id)}";
            MainWindow.MainCanvas.Children.Add(textBlock);
            Canvas.SetZIndex(textBlock, 20);
            Rectangle answerRect=DrawHelper.AnswerRect;
            MainWindow.MainCanvas.Children.Add(answerRect);
            Canvas.SetZIndex(answerRect, 18);
            Canvas.SetTop(answerRect, MainWindow.MainCanvas.Height - answerRect.Height);
            Canvas.SetLeft(answerRect, MainWindow.MainCanvas.Width - answerRect.Width);
            MainWindow.MainCanvas.Children.Add(DrawHelper.BtnReturn);
            DrawHelper.BtnReturn.Click += MainWindow.BtnReturn_Click;
            Canvas.SetZIndex(DrawHelper.BtnReturn, 20);
        }


    }
}
