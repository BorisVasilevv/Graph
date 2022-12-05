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
        public static List<Connection> Connections { get; set; }
        public static Connection NewConnection { get; set; }
        private static Rectangle Rect1 = null;
        private static Rectangle Rect2 = null;


        public AddVerticeTool ToolAddVertice = MainWindow.ToolAddVertice;

        public static void AddConnection(object sender, MouseEventArgs e)
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
                    NewConnection.Vertice1 = Vertice.SearchVertice(Rect1);
                    NewConnection.Vertice2 = Vertice.SearchVertice(Rect2);
                    if (!Connection.ConnectionRepeat(Connections, NewConnection))
                    {

                        Connections.Add(NewConnection);
                        Rect1 = Rect2 = null;
                    }
                    DrawConnections();
                    
                    MainWindow.MainCanvas.MouseDown -= AddConnection;
                }
            }
            MainWindow.MainCanvas.MouseDown += MainWindow.ToolAddVertice.RectMouseDown;
        }

        public static bool IsConnectionNewTest(Connection connectionToTest)
        {
            foreach (Connection connection in Connections)
            {
                if ((connection.Vertice1.Id == connectionToTest.Vertice1.Id && connection.Vertice2.Id == connectionToTest.Vertice2.Id)
                    || (connection.Vertice1.Id == connectionToTest.Vertice2.Id && connection.Vertice2.Id == connectionToTest.Vertice1.Id))
                {
                    return false;
                }
            }
            return true;
        }

        public static void DrawConnections()
        {
            foreach (Connection connection in Connections)
            {
                if (connection.Polyline != null) MainWindow.MainCanvas.Children.Remove(connection.Polyline); //удаление линии при перетаскивании прямоугольника
                Polyline line = new Polyline();
                PointCollection points = new PointCollection();
                Point point1 = connection.Vertice1.RectCenter;
                Point point2 = connection.Vertice2.RectCenter;


                points.Add(point1);
                points.Add(point2);
                line.Fill = Brushes.Blue;
                line.Stroke = Brushes.Blue;


                line.Points = points;
                Canvas.SetZIndex(line, 1);
                connection.Polyline = line;
                MainWindow.MainCanvas.Children.Add(line);
            }
        }
    }
}
