using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Graph
{
    public static class FileWorker
    {

        public static string FilePath { get; set; }

        public static (List<Vertice>, List<Connection>) Read(string filePath)
        {
            List<Vertice> vertice = new List<Vertice>();
            List<Connection> connections = new List<Connection>();
            string[] a = File.ReadAllLines(filePath);
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
            return (vertice, connections);
        }

        public static void WriteToFile(List<Vertice> vertices, List<Connection> connections, string filePath)
        {
            List<string[]> stringsVertices = new List<string[]>();
            for (int i = 0; i < vertices.Count; i++)
            {
                string[] arrayConnection = new string[vertices.Count];
                for (int j = 0; j < vertices.Count; j++)
                {
                    Connection checkConnection = null;
                    if (i == j) arrayConnection[j] = "0";
                    else checkConnection = Connection.SearchConnection(Vertice.SearchVertice(vertices[i].Id), Vertice.SearchVertice(vertices[j].Id), connections);

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
