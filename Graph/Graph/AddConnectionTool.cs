using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Graph
{
    public class AddConnectionTool
    {
        //public static List<Connection> Connections { get; set; }
        public static Connection NewConnection { get; set; }
        private static Rectangle Rect1 = null;
        private static Rectangle Rect2 = null;
        static MyGraph MainGraph=MainWindow.MainGraph;

        public AddVerticeTool ToolAddVertice = MainWindow.ToolAddVertice;

        public static void addConnection(object sender, MouseEventArgs e)
        {
            
            Vertice data1, data2;
            data1 = data2 = null;
            NewConnection = new Connection();
            if (AddVerticeTool.SelectedRectangle != null)
            {
                if (Rect1 == null&& e.LeftButton == MouseButtonState.Pressed)
                {
                    Rect1 = AddVerticeTool.SelectedRectangle;
                }
                else if (Rect1!=null&&!Rect1.Equals(AddVerticeTool.SelectedRectangle) && e.LeftButton == MouseButtonState.Pressed)
                {
                    Rect2 = AddVerticeTool.SelectedRectangle;
                    NewConnection.Vertice1 = Vertice.SearchVertice(Rect1, MainGraph.AllVertices);
                    NewConnection.Vertice2 = Vertice.SearchVertice(Rect2, MainGraph.AllVertices);
                    NewConnection.Length = 1;
                    TextBlock textBlock = new TextBlock();
                    NewConnection.BlockText = textBlock;
                    NewConnection.BlockText.Text = "1";
                    NewConnection.BlockText.MouseDown += ChangeDataTool.TextBlock_MouseDown;


                    if (!Connection.ConnectionRepeat(MainGraph.Connections, NewConnection))
                    {
                        MainGraph.Connections.Add(NewConnection);
                        Rect1 = Rect2 = null;
                    }
                    DrawGraphHelper.DrawConnections(MainWindow.MainCanvas, MainGraph.Connections);
                    
                    MainWindow.MainCanvas.MouseDown -= addConnection;
                }
            }
            MainWindow.MainCanvas.MouseDown += MainWindow.ToolAddVertice.rectMouseDown;
        }

        public static bool IsConnectionNewTest(Connection connectionToTest)
        {
            foreach (Connection connection in MainGraph.Connections)
            {
                if ((connection.Vertice1.Id == connectionToTest.Vertice1.Id && connection.Vertice2.Id == connectionToTest.Vertice2.Id)
                    || (connection.Vertice1.Id == connectionToTest.Vertice2.Id && connection.Vertice2.Id == connectionToTest.Vertice1.Id))
                {
                    return false;
                }
            }
            return true;
        }

       
    }
}
