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
    public class DrawHelper
    {
        const int LeftIndent = 100;
        const int TopIndent = 40;
        const int RectOnOneLine = 11;
        const int RectBetweenIndent = 60;
        const int TextBoxIndent = 35;
        public static void DrawGraph(Canvas canvas, MyGraph graph)
        {

            for (int i = 0; i < graph.AllVertices.Count; i++)
            {
                if(graph.AllVertices[i].Rect == null)
                {
                    Rectangle Rect = new Rectangle();
                    Rect.Width = 50;
                    Rect.Height = 50;
                    Rect.Fill = new SolidColorBrush(Colors.Brown);
                    Rect.Stroke = new SolidColorBrush(Colors.Black);
                    Rect.MouseMove += MainWindow.ToolAddVertice.RectangleMouseMove;
                    Canvas.SetZIndex(Rect, 2);
                    canvas.Children.Add(Rect);
                    Point center = new Point(LeftIndent + RectBetweenIndent * (i % RectOnOneLine), TopIndent + RectBetweenIndent * (i / RectOnOneLine));
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
                    Canvas.SetTop(textBlock, center.Y + TextBoxIndent);
                    Canvas.SetLeft(textBlock, center.X);
                    graph.AllVertices[i].NameTextBlock = textBlock;
                }
                else
                {
                    Vertice vert = graph.AllVertices[i];
                    Canvas.SetTop(vert.Rect, vert.RectCenter.Y-vert.Rect.Height/2);
                    Canvas.SetLeft(vert.Rect, vert.RectCenter.X - vert.Rect.Width / 2);
                    canvas.Children.Add(vert.Rect);
                    Canvas.SetTop(vert.NameTextBlock, vert.RectCenter.Y + TextBoxIndent - vert.Rect.Height / 2);
                    Canvas.SetLeft(vert.NameTextBlock, vert.RectCenter.X - vert.Rect.Width / 2);
                    canvas.Children.Add(vert.NameTextBlock);
                }
                

            }
            DrawConnections(canvas, graph);
        }

        public static void DrawConnections(Canvas canvas, MyGraph graph)
        {
            foreach (Connection connection in graph.Connections)
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

        public static Button BtnReturn = new Button
        {
            Content = "To original graph",
            Height = 20,
            Width = 125,
            Background = new SolidColorBrush(Colors.Gray),
            Margin = new Thickness(650, 350, 0, 0),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalContentAlignment = HorizontalAlignment.Center
        };

        public static Button BtnSearchDepth = new Button
        {
            Content = "In depth",
            Height = 20,
            Width = 100,
            Background = new SolidColorBrush(Colors.Gray),
            Margin = new Thickness(80, 170, 0, 0),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalContentAlignment = HorizontalAlignment.Center

        };

        public static Button BtnSearchWidth = new Button
        {
            Content = "In Width",
            Height = 20,
            Width = 100,
            Background = new SolidColorBrush(Colors.Gray),
            Margin = new Thickness(80, 190, 0, 0),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalContentAlignment = HorizontalAlignment.Center

        };

        public static TextBlock TextBlock = new TextBlock
        {
            Height = 40,
            Width = 200,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(655, 375, 0, 0)
        };

        public static Rectangle AnswerRect = new Rectangle
        {
            Height = 100,
            Width = 150,
            Fill = new SolidColorBrush(Colors.Brown),
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Bottom
        };

    }
}
