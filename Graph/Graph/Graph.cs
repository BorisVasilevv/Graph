using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    public class Graph
    {
        public List<Vertice> AllVertices;

        public List<Connection> Connections;
        public Graph() { }

        public Graph(List<Vertice> vertices, List<Connection> connections)
        {
            AllVertices = vertices;
            Connections = connections;
        }

        public Graph(Graph graph)
        {
            AllVertices=graph.AllVertices;
            Connections=graph.Connections;
        }

    }
}
