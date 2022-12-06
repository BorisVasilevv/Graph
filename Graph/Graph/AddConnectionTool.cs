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
                    NewConnection.Vertice1 = Vertice.SearchVertice(Rect1);
                    NewConnection.Vertice2 = Vertice.SearchVertice(Rect2);
                    NewConnection.Length = 1;
                    TextBlock textBlock = new TextBlock();
                    NewConnection.BlockText = textBlock;
                    NewConnection.BlockText.Text = "1";


                    if (!Connection.ConnectionRepeat(Connections, NewConnection))
                    {
                        Connections.Add(NewConnection);
                        Rect1 = Rect2 = null;
                    }
                    DrawConnections();
                    
                    MainWindow.MainCanvas.MouseDown -= addConnection;
                }
            }
            MainWindow.MainCanvas.MouseDown += MainWindow.ToolAddVertice.rectMouseDown;
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
                if (connection.Line != null) MainWindow.MainCanvas.Children.Remove(connection.Line); //удаление линии при перетаскивании прямоугольника
                Polyline line = new Polyline();
                PointCollection points = new PointCollection();
                Point point1 = connection.Vertice1.RectCenter;
                Point point2 = connection.Vertice2.RectCenter;
                double minX = point1.X < point2.X ? point1.X : point2.X;
                double minY = point1.Y < point2.Y ? point1.Y : point2.Y;
                Point textBlockCenter= new Point(minX+Math.Abs(point1.X-point2.X)/2, minY+Math.Abs(point1.Y-point2.Y)/2);
                if (connection.BlockText != null) MainWindow.MainCanvas.Children.Remove(connection.BlockText);
                MainWindow.MainCanvas.Children.Add(connection.BlockText);
                Canvas.SetZIndex(connection.BlockText, 0);
                Canvas.SetLeft(connection.BlockText, textBlockCenter.X);
                Canvas.SetTop(connection.BlockText, textBlockCenter.Y-15);
                //connection.BlockText.VerticalAlignment =VerticalAlignment.Center;
                //connection.BlockText.HorizontalAlignment =HorizontalAlignment.Center;
                //connection.BlockText.Margin = new Thickness(textBlockCenter.X, textBlockCenter.Y, 800- textBlockCenter.X, 450- textBlockCenter.Y);

                


                points.Add(point1);
                points.Add(point2);
                line.Fill = Brushes.Blue;
                line.Stroke = Brushes.Blue;


                line.Points = points;
                Canvas.SetZIndex(line, 1);
                connection.Line = line;
                MainWindow.MainCanvas.Children.Add(line);
            }
        }
    }
}
