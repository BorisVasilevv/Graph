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
    class DijkstraAlgorithm
    {
        static int result;

        static Rectangle AnswerRect = new Rectangle
        {
            Height = 100,
            Width = 150,
            Fill = new SolidColorBrush(Colors.Brown),
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Bottom
        };

        static TextBlock AnswerBlock = new TextBlock
        {
            Height = 40,
            Width = 150,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Bottom,
            Margin = new Thickness(655, 375, 0, 0)
            //Text = result.ToString()
        };

        static MyGraph GraphToWork;

        public static void Algorithm(Vertice start, Vertice end, MyGraph graph)
        {
            Logger logger = new Logger();
            logger.AddLine("Начат алгоритм поиска минимального пути");

            var g = new GraphD();
            GraphToWork = graph;

            //добавление вершин
            foreach ( var vertex in graph.AllVertices)
            {
                g.AddVertex(vertex.Id.ToString());
            }

            //добавление ребер
            for (int i = 0; i < graph.Connections.Count; i++)
            {
                g.AddEdge(graph.Connections[i].Vertice1.Id.ToString(), graph.Connections[i].Vertice2.Id.ToString(), graph.Connections[i].Length);
            }
          
            var dijkstra = new Dijkstra(g);
            var path = dijkstra.FindShortestPath(start.Id.ToString(), end.Id.ToString(), logger);

            List<Connection> connections1 = new List<Connection>();
            for (int i = 0; i < path.Count - 1; i++)
            {
                connections1.Add(
                    Connection.SearchConnection(graph.AllVertices[path[i]], graph.AllVertices[path[i + 1]], graph.Connections));
            }

            List<Vertice> vertices1 = new List<Vertice>();
            foreach (var vertise in graph.AllVertices)
            {
                if (path.Contains(vertise.Id))
                    vertices1.Add(vertise);
            }
            MainWindow.MainGraph = new MyGraph( vertices1, connections1);
            

            MainWindow.MainCanvas.Children.Clear();

            DrawHelper.DrawGraph(MainWindow.MainCanvas, MainWindow.MainGraph);

            MainWindow.IsUserCanUseButtons = true;
            DrawHelper.BtnReturn.Click += BtnReturn_Click;

            foreach (Connection connection in connections1)
            {
                result += connection.Length;
            }

            AnswerBlock.Text = $"Min way from {start.NameTextBlock.Text} to {end.NameTextBlock.Text} \nResult: {result}" ;

            MainWindow.MainCanvas.Children.Add(DrawHelper.BtnReturn);

            Canvas.SetZIndex(DrawHelper.BtnReturn, 20);
            MainWindow.MainCanvas.Children.Add(AnswerRect);
            Canvas.SetZIndex(AnswerRect, 18);
            Canvas.SetTop(AnswerRect, MainWindow.MainCanvas.Height - AnswerRect.Height);
            Canvas.SetLeft(AnswerRect, MainWindow.MainCanvas.Width - AnswerRect.Width);

            MainWindow.MainCanvas.Children.Add(AnswerBlock);
            Canvas.SetZIndex(AnswerBlock, 19);
        }



        public static void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.MainCanvas.Children.Clear();
            MainWindow.MainGraph = GraphToWork;
            DrawHelper.DrawGraph(MainWindow.MainCanvas, GraphToWork);
        }

        /// <summary>
        /// Ребро графа
        /// </summary>
        public class GraphEdge
        {
            /// <summary>
            /// Связанная вершина
            /// </summary>
            public GraphVertex ConnectedVertex { get; }

            /// <summary>
            /// Вес ребра
            /// </summary>
            public int EdgeWeight { get; }

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="connectedVertex">Связанная вершина</param>
            /// <param name="weight">Вес ребра</param>
            public GraphEdge(GraphVertex connectedVertex, int weight)
            {
                ConnectedVertex = connectedVertex;
                EdgeWeight = weight;
            }
        }

        /// <summary>
        /// Вершина графа
        /// </summary>
        public class GraphVertex
        {
            /// <summary>
            /// Название вершины
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// Список ребер
            /// </summary>
            public List<GraphEdge> Edges { get; }

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="vertexName">Название вершины</param>
            public GraphVertex(string vertexName)
            {
                Name = vertexName;
                Edges = new List<GraphEdge>();
            }

            /// <summary>
            /// Добавить ребро
            /// </summary>
            /// <param name="newEdge">Ребро</param>
            public void AddEdge(GraphEdge newEdge)
            {
                Edges.Add(newEdge);
            }

            /// <summary>
            /// Добавить ребро
            /// </summary>
            /// <param name="vertex">Вершина</param>
            /// <param name="edgeWeight">Вес</param>
            public void AddEdge(GraphVertex vertex, int edgeWeight)
            {
                AddEdge(new GraphEdge(vertex, edgeWeight));
            }

            /// <summary>
            /// Преобразование в строку
            /// </summary>
            /// <returns>Имя вершины</returns>
            public override string ToString() => Name;
        }

        /// <summary>
        /// Граф
        /// </summary>
        public class GraphD
        {
            /// <summary>
            /// Список вершин графа
            /// </summary>
            public List<GraphVertex> Verticess { get; }

            /// <summary>
            /// Конструктор
            /// </summary>
            public GraphD()
            {
                Verticess = new List<GraphVertex>();
            }

            /// <summary>
            /// Добавление вершины
            /// </summary>
            /// <param name="vertexName">Имя вершины</param>
            public void AddVertex(string vertexName)
            {
                Verticess.Add(new GraphVertex(vertexName));
            }

            /// <summary>
            /// Поиск вершины
            /// </summary>
            /// <param name="vertexName">Название вершины</param>
            /// <returns>Найденная вершина</returns>
            public GraphVertex FindVertex(string vertexName)
            {
                foreach (var v in Verticess)
                {
                    if (v.Name.Equals(vertexName))
                    {
                        return v;
                        //Logger.add($"Выбрали вершину: {v.Name}")
                    }
                }

                return null;
            }

            /// <summary>
            /// Добавление ребра
            /// </summary>
            /// <param name="firstName">Имя первой вершины</param>
            /// <param name="secondName">Имя второй вершины</param>
            /// <param name="weight">Вес ребра соединяющего вершины</param>
            public void AddEdge(string firstName, string secondName, int weight)
            {
                var v1 = FindVertex(firstName);
                var v2 = FindVertex(secondName);
                if (v2 != null && v1 != null)
                {
                    v1.AddEdge(v2, weight);
                    v2.AddEdge(v1, weight);
                }
            }
        }
        /// <summary>
        /// Информация о вершине
        /// </summary>
        public class GraphVertexInfo
        {
            /// <summary>
            /// Вершина
            /// </summary>
            public GraphVertex Vertex { get; set; }

            /// <summary>
            /// Не посещенная вершина
            /// </summary>
            public bool IsUnvisited { get; set; }

            /// <summary>
            /// Сумма весов ребер
            /// </summary>
            public int EdgesWeightSum { get; set; }

            /// <summary>
            /// Предыдущая вершина
            /// </summary>
            public GraphVertex PreviousVertex { get; set; }

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="vertex">Вершина</param>
            public GraphVertexInfo(GraphVertex vertex)
            {
                Vertex = vertex;
                IsUnvisited = true;
                EdgesWeightSum = int.MaxValue;
                PreviousVertex = null;
            }
        }

        /// <summary>
        /// Алгоритм Дейкстры
        /// </summary>
        public class Dijkstra
        {
            GraphD graph;

            List<GraphVertexInfo> infos;

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="graph">Граф</param>
            public Dijkstra(GraphD graph)
            {
                this.graph = graph;
            }

            /// <summary>
            /// Инициализация информации
            /// </summary>
            void InitInfo()
            {
                infos = new List<GraphVertexInfo>();
                foreach (var v in graph.Verticess)
                {
                    infos.Add(new GraphVertexInfo(v));
                }
            }

            /// <summary>
            /// Получение информации о вершине графа
            /// </summary>
            /// <param name="v">Вершина</param>
            /// <returns>Информация о вершине</returns>
            GraphVertexInfo GetVertexInfo(GraphVertex v)
            {
                foreach (var i in infos)
                {
                    if (i.Vertex.Equals(v))
                    {
                        return i;
                    }
                }

                return null;
            }

            /// <summary>
            /// Поиск непосещенной вершины с минимальным значением суммы
            /// </summary>
            /// <returns>Информация о вершине</returns>
            public GraphVertexInfo FindUnvisitedVertexWithMinSum(Logger logger)
            {
                var minValue = int.MaxValue;
                GraphVertexInfo minVertexInfo = null;
                foreach (var i in infos)
                {
                    if (i.IsUnvisited && i.EdgesWeightSum < minValue)
                    {
                        minVertexInfo = i;
                        minValue = i.EdgesWeightSum;
                    }
                }
                if(minVertexInfo != null)
                    logger.AddLine($"Нашли вершину: {int.Parse(minVertexInfo.Vertex.Name) + 1}, до которой путь будет минимальным");
                return minVertexInfo;
            }

            /// <summary>
            /// Поиск кратчайшего пути по названиям вершин
            /// </summary>
            /// <param name="startName">Название стартовой вершины</param>
            /// <param name="finishName">Название финишной вершины</param>
            /// <returns>Кратчайший путь</returns>
            public List<int> FindShortestPath(string startName, string finishName, Logger logger)
            {
                return FindShortestPath(graph.FindVertex(startName), graph.FindVertex(finishName), logger)
                    .Split(';').ToList().Select(int.Parse).ToList<int>();
            }

            /// <summary>
            /// Поиск кратчайшего пути по вершинам
            /// </summary>
            /// <param name="startVertex">Стартовая вершина</param>
            /// <param name="finishVertex">Финишная вершина</param>
            /// <returns>Кратчайший путь</returns>
            public string FindShortestPath(GraphVertex startVertex, GraphVertex finishVertex, Logger logger)
            {
                InitInfo();
                var first = GetVertexInfo(startVertex);
                first.EdgesWeightSum = 0;
                while (true)
                {
                    var current = FindUnvisitedVertexWithMinSum(logger);
                    if (current == null)
                    {
                        break;
                    }

                    SetSumToNextVertex(current);
                    logger.AddLine($"Нашли минимальный путь от вершины {int.Parse(current.Vertex.Name)+1} к следующей вершине, найденной выше, равный: {current.EdgesWeightSum}");
                }

                return GetPath(startVertex, finishVertex, logger);
            }

            /// <summary>
            /// Вычисление суммы весов ребер для следующей вершины
            /// </summary>
            /// <param name="info">Информация о текущей вершине</param>
            void SetSumToNextVertex(GraphVertexInfo info)
            {
                info.IsUnvisited = false;
                foreach (var e in info.Vertex.Edges)
                {
                    var nextInfo = GetVertexInfo(e.ConnectedVertex);
                    var sum = info.EdgesWeightSum + e.EdgeWeight;
                    if (sum < nextInfo.EdgesWeightSum)
                    {
                        nextInfo.EdgesWeightSum = sum;
                        nextInfo.PreviousVertex = info.Vertex;
                    }
                }
            }

            /// <summary>
            /// Формирование пути
            /// </summary>
            /// <param name="startVertex">Начальная вершина</param>
            /// <param name="endVertex">Конечная вершина</param>
            /// <returns>Путь</returns>
            string GetPath(GraphVertex startVertex, GraphVertex endVertex, Logger logger)
            {
                var path = endVertex.ToString();
                logger.AddLine($"Восстанавливаем путь, для заданных начальной и конечной вершин:");
                while (startVertex != endVertex)
                {
                    endVertex = GetVertexInfo(endVertex).PreviousVertex;
                    if (path.Length > 1)
                    {
                        List<int> path1 = path.Split(';').ToList().Select(x => int.Parse(x)).ToList<int>();
                        logger.AddLine($"Взяли путь от вершины {int.Parse(endVertex.Name) + 1} до вершины {path1.Last()+1}");
                    }
                    else
                    {
                        logger.AddLine($"Взяли путь от вершины {int.Parse(endVertex.Name) + 1} до вершины {int.Parse(path)+1}");
                    }
                    path = $"{endVertex.ToString()};{path}";
                }

                return path;
            }
        }

    }
}
