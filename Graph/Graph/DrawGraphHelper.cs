using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Graph
{
    public class DrawGraphHelper
    {
        const int LeftIndent = 100;
        const int TopIndent = 40;
        const int RectOnOneLine = 11;
        const int RectBetweenIndent = 60;
        public static void DrawGraph(Canvas canvas, Graph graph)
        {

            for (int i = 0; i < graph.AllVertices.Count; i++)
            {
                Rectangle Rect = new Rectangle();
                Rect.Width = 50;
                Rect.Height = 50;
                Rect.Fill = new SolidColorBrush(Colors.Brown);
                Rect.Stroke = new SolidColorBrush(Colors.Black);
                Rect.MouseMove += MainWindow.ToolAddVertice.RectangleMouseMove;
                Canvas.SetZIndex(Rect, 2);
                canvas.Children.Add(Rect);
                Point center = new Point(LeftIndent + RectBetweenIndent * (i % RectOnOneLine), TopIndent+ RectBetweenIndent * (i / RectOnOneLine));
                Canvas.SetTop(Rect, center.Y);
                Canvas.SetLeft(Rect, center.X);

                graph.AllVertices[i].Rect = Rect;
                graph.AllVertices[i].RectCenter = new Point(center.X + Rect.Width / 2, center.Y + Rect.Height / 2);
                TextBlock textBlock = new TextBlock() { Text = (graph.AllVertices[i].Id + 1).ToString() };

                textBlock.Height = 20;
                textBlock.Width = 50;
                textBlock.VerticalAlignment = VerticalAlignment.Top;
                textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                textBlock.TextAlignment = TextAlignment.Center;
                Canvas.SetZIndex(textBlock, 2);
                canvas.Children.Add(textBlock);
                Canvas.SetTop(textBlock, TopIndent+ RectBetweenIndent * (i / RectOnOneLine) + 30);
                Canvas.SetLeft(textBlock, LeftIndent + RectBetweenIndent * (i % RectOnOneLine));
                graph.AllVertices[i].VerticeNameTextBlock = textBlock;

            }
            DrawConnections(canvas, graph.Connections);
        }



        public static void DrawConnections(Canvas canvas, List<Connection> connections)
        {
            foreach (Connection connection in connections)
            {
                if (connection.Line != null) canvas.Children.Remove(connection.Line); //удаление линии при перетаскивании прямоугольника
                Polyline line = new Polyline();
                PointCollection points = new PointCollection();
                Point point1 = connection.Vertice1.RectCenter;
                Point point2 = connection.Vertice2.RectCenter;
                double minX = point1.X < point2.X ? point1.X : point2.X;
                double minY = point1.Y < point2.Y ? point1.Y : point2.Y;
                Point textBlockCenter = new Point(minX + Math.Abs(point1.X - point2.X) / 2, minY + Math.Abs(point1.Y - point2.Y) / 2);
                if (connection.BlockText != null) canvas.Children.Remove(connection.BlockText);
                canvas.Children.Add(connection.BlockText);
                Canvas.SetZIndex(connection.BlockText, 0);
                Canvas.SetLeft(connection.BlockText, textBlockCenter.X);
                Canvas.SetTop(connection.BlockText, textBlockCenter.Y - 15);
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
                canvas.Children.Add(line);
            }
        }
    }
}
