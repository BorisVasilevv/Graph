using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Graph.DijkstraAlgorithm;

namespace Graph
{
    public static class FileWorker
    {

        public static string FilePath { get; set; }

        public static MyGraph Read(string filePath)
        {
            List<Vertice> vertice = new List<Vertice>();
            List<Connection> connections = new List<Connection>();

            List<string[]> stringsVertices = File.ReadAllLines(filePath).Select(x => x.Split(";")).ToList();

            for (int i = 0; i < stringsVertices.Count; i++)
                vertice.Add(new Vertice());

            for (int i = 0; i < vertice.Count; i++)
            {
                List<int> connectionsForVertice = new List<int>();
                for (int j = 0; j < vertice.Count; j++)
                {
                    int lengthConnection = int.Parse(stringsVertices[i][j]);
                    if (lengthConnection != 0)
                    {
                        Connection newConn = new Connection(lengthConnection, vertice[i], vertice[j]);
                        vertice[i].ConnectionIds.Add(vertice[j].Id);
                        if (!Connection.ConnectionRepeat(connections, newConn))
                        {
                            connections.Add(newConn);
                        }

                    }

                }
                vertice[i].ConnectionIds = new List<int>(connectionsForVertice);
            }
            return new MyGraph(vertice, connections);
        }

        public static void WriteToFile(MyGraph graph, string filePath)
        {
            List<string[]> stringsVertices = new List<string[]>();
            for (int i = 0; i < graph.AllVertices.Count; i++)
            {
                string[] arrayConnection = new string[graph.AllVertices.Count];
                for (int j = 0; j < graph.AllVertices.Count; j++)
                {
                    Connection checkConnection = null;
                    if (i == j) arrayConnection[j] = "0";
                    else checkConnection = Connection.SearchConnection(
                        Vertice.SearchVertice(graph.AllVertices[i].Id, graph.AllVertices), 
                        Vertice.SearchVertice(graph.AllVertices[j].Id, graph.AllVertices),
                        graph.Connections);

                    if (checkConnection != null) arrayConnection[j] = checkConnection.Length.ToString();
                    else arrayConnection[j] = "0";
                }
                stringsVertices.Add(arrayConnection);
            }
            string[] str=stringsVertices.Select(x => string.Join(';',x)).ToArray();
            File.WriteAllLines(filePath,str);
        }

    }
}
