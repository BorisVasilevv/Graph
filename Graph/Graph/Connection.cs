using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Graph
{
    public class Connection
    {
        public int Length { get; set; }
        public Vertice Vertice1 { get; set; }
        public Vertice Vertice2 { get; set; }
        public Polyline Line { get; set; }

        public TextBlock BlockText { get; set; }

        public Connection()
        {
            this.BlockText = new TextBlock();
            BlockText.MouseDown += TextBlock_MouseDown;

        }

        private void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
        }

        public Connection(int length, Vertice vertice1, Vertice vertice2)
        {
            this.Length = length;
            Vertice1 = vertice1;
            Vertice2 = vertice2;
            this.BlockText=new TextBlock();
            this.BlockText.Text = length.ToString();
            BlockText.MouseDown += TextBlock_MouseDown;
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

        public static Connection SearchConnection(Polyline line)
        {
            foreach (Connection connect in AddConnectionTool.Connections)
            {
                if (connect.Line == line) return connect;
            }
            return null;
        }

        public static Connection SearchConnection(Vertice vertice1, Vertice vertice2, List<Connection> connections)
        {
            foreach (Connection connection in connections)
            {
                if ((connection.Vertice1.Id == vertice1.Id && connection.Vertice2.Id == vertice2.Id)
                    || (connection.Vertice2.Id == vertice1.Id && connection.Vertice1.Id == vertice2.Id))
                {
                    return connection;
                }
            }
            return null;
        }
    }
}
