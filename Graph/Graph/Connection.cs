using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Graph
{
    public class Connection
    {
        public int Length;
        public Vertice Vertice1;
        public Vertice Vertice2;
        public Polyline Polyline;

        public Connection()
        {


        }

        public Connection(int length, Vertice vertice1, Vertice vertice2)
        {
            this.Length = length;
            Vertice1 = vertice1;
            Vertice2 = vertice2;
        }

        public static bool ConnectionRepeat(List<Connection> connections, Connection checkConnection)
        {
            foreach (Connection connection in connections)
            {
                if ((connection.Vertice1 == checkConnection.Vertice1 && connection.Vertice1 == checkConnection.Vertice1)
                    || (connection.Vertice1 == checkConnection.Vertice2 && connection.Vertice2 == checkConnection.Vertice1)) return true;
            }
            return false;
        }
    }
}
