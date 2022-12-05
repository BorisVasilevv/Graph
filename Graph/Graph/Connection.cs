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
        public int Length { get; set; }
        public Vertice Vertice1 { get; set; }
        public Vertice Vertice2 { get; set; }
        public Polyline Polyline { get; set; }

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
                if ((connection.Vertice1.Id == checkConnection.Vertice1.Id && connection.Vertice2.Id == checkConnection.Vertice1.Id)
                    || (connection.Vertice1.Id == checkConnection.Vertice2.Id && connection.Vertice2.Id == checkConnection.Vertice1.Id)) return true;
            }
            return false;
        }

        public static Connection SearchConnection(Vertice vertice1, Vertice vertice2, List<Connection> connections)
        {
            foreach (Connection connection in connections)
            {
                if((connection.Vertice1.Id == vertice1.Id && connection.Vertice2.Id == vertice2.Id)
                    || (connection.Vertice2.Id == vertice1.Id && connection.Vertice1.Id == vertice2.Id))
                {
                    return connection;
                }
            }
            return null;
        }
    }
}
