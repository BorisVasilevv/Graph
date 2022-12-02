using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Graph
{
    public static class FileWorker
    {
        public static (List<Vertice>, List<Connection>) Read(string filePath)
        {
            List<Vertice> vertice = new List<Vertice>();
            List<Connection> connections = new List<Connection>();
            List <string[]> stringsVertices = File.ReadAllLines(filePath).Select(x=>x.Split(";")).ToList();
            
            for (int i = 0; i < stringsVertices.Count; i++)
                vertice.Add(new Vertice());

            for (int i = 0; i < vertice.Count; i++)
            {
                List<Connection> connectionsForVertice = new List<Connection>();
                for (int j = 0; j < vertice.Count; j++)
                {
                    
                    if (stringsVertices[i][j] != "0")
                    {
                        
                        Connection newConn =new Connection(int.Parse(stringsVertices[i][j]), vertice[i], vertice[j]);
                        connectionsForVertice.Add(newConn);
                        if (!Connection.ConnectionRepeat(connections, newConn))
                        {
                            connections.Add(newConn);
                            
                        }
                        
                    }
                    
                }
                vertice[i].connections = new List<Connection>(connectionsForVertice);
            }
            return (vertice,connections);
        }

        public static void Write(List<Vertice> vertices, List<Connection> connections)
        {
            List<string[]> stringsVertices= new List<string[]>();
            for (int i = 0; i < vertices.Count; i++)
            {

            }
        }

    }
}
