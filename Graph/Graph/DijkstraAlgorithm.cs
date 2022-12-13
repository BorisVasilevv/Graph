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
using System.Windows.Shapes;


namespace Graph
{
    class DijkstraAlgorithm
    {
        public static void Algorithm(Vertice start, Vertice end)
        {
            var g = new Graph();

            var connections = AddConnectionTool.Connections;

            var vertices = AddVerticeTool.AllVertices;

            //добавление вершин
            foreach ( var vertex in vertices )
            {
                g.AddVertex(vertex.Id.ToString());
            }

            //добавление ребер
            for (int i = 0; i < connections.Count; i++)
            {
                g.AddEdge(connections[i].Vertice1.Id.ToString(), connections[i].Vertice2.Id.ToString(), connections[i].Length);
            }
          
            var dijkstra = new Dijkstra(g);
            var path = dijkstra.FindShortestPath(start.Id.ToString(), end.Id.ToString());

            List<Connection> connections1 = new List<Connection>();
            for (int i = 0; i < path.Count - 1; i++)
            {
                connections1.Add(
                    Connection.SearchConnection(vertices[path[i]], vertices[path[i + 1]]));
            }

            List<Vertice> vertices1 = new List<Vertice>();
            foreach (var vertise in vertices)
            {
                if (path.Contains(vertise.Id))
                    vertices1.Add(vertise);
            }

            MainWindow.MainCanvas.Children.Clear();

            DrawGraphHelper.DrawGraph(MainWindow.MainCanvas, vertices1, connections1);

            MainWindow.IsProgramReady = true;
            MainWindow.BtnReturn.Click += MainWindow.BtnReturn_Click;

            MainWindow.MainCanvas.Children.Add(MainWindow.BtnReturn);
            Canvas.SetZIndex(MainWindow.BtnReturn, 20);
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
        public class GraphVertex : Vertice
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
        public class Graph
        {
            /// <summary>
            /// Список вершин графа
            /// </summary>
            public List<GraphVertex> Vertices { get; }

            /// <summary>
            /// Конструктор
            /// </summary>
            public Graph()
            {
                Vertices = new List<GraphVertex>();
            }

            /// <summary>
            /// Добавление вершины
            /// </summary>
            /// <param name="vertexName">Имя вершины</param>
            public void AddVertex(string vertexName)
            {
                Vertices.Add(new GraphVertex(vertexName));
            }

            /// <summary>
            /// Поиск вершины
            /// </summary>
            /// <param name="vertexName">Название вершины</param>
            /// <returns>Найденная вершина</returns>
            public GraphVertex FindVertex(string vertexName)
            {
                foreach (var v in Vertices)
                {
                    if (v.Name.Equals(vertexName))
                    {
                        return v;
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
            Graph graph;

            List<GraphVertexInfo> infos;

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="graph">Граф</param>
            public Dijkstra(Graph graph)
            {
                this.graph = graph;
            }

            /// <summary>
            /// Инициализация информации
            /// </summary>
            void InitInfo()
            {
                infos = new List<GraphVertexInfo>();
                foreach (var v in graph.Vertices)
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
            public GraphVertexInfo FindUnvisitedVertexWithMinSum()
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

                return minVertexInfo;
            }

            /// <summary>
            /// Поиск кратчайшего пути по названиям вершин
            /// </summary>
            /// <param name="startName">Название стартовой вершины</param>
            /// <param name="finishName">Название финишной вершины</param>
            /// <returns>Кратчайший путь</returns>
            public List<int> FindShortestPath(string startName, string finishName)
            {
                return FindShortestPath(graph.FindVertex(startName), graph.FindVertex(finishName))
                    .Split(';').ToList().Select(int.Parse).ToList<int>();
            }

            /// <summary>
            /// Поиск кратчайшего пути по вершинам
            /// </summary>
            /// <param name="startVertex">Стартовая вершина</param>
            /// <param name="finishVertex">Финишная вершина</param>
            /// <returns>Кратчайший путь</returns>
            public string FindShortestPath(GraphVertex startVertex, GraphVertex finishVertex)
            {
                InitInfo();
                var first = GetVertexInfo(startVertex);
                first.EdgesWeightSum = 0;
                while (true)
                {
                    var current = FindUnvisitedVertexWithMinSum();
                    if (current == null)
                    {
                        break;
                    }

                    SetSumToNextVertex(current);
                }

                return GetPath(startVertex, finishVertex);
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
            string GetPath(GraphVertex startVertex, GraphVertex endVertex)
            {
                var path = endVertex.ToString();
                while (startVertex != endVertex)
                {
                    endVertex = GetVertexInfo(endVertex).PreviousVertex;
                    path = $"{endVertex.ToString()};{path}";
                }

                return path;
            }
        }

    }
}
