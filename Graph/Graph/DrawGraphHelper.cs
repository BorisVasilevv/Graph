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
        const int TopIndent = 10;
        const int RectOnOneLine = 11;
        const int RectBetweenIndent = 60;
        public static void DrawGraph(Canvas canvas, List<Vertice> vertices, List<Connection> connections)
        {
            AddConnectionTool.Connections=connections;
            AddVerticeTool.AllVertices=vertices;

            for (int i = 0; i < vertices.Count; i++)
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

                vertices[i].Rect = Rect;
                vertices[i].RectCenter = new Point(center.X + Rect.Width / 2, center.Y + Rect.Height / 2);
                TextBlock textBlock = new TextBlock() { Text = (vertices[i].Id + 1).ToString() };

                textBlock.Height = 20;
                textBlock.Width = 50;
                textBlock.VerticalAlignment = VerticalAlignment.Top;
                textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                textBlock.TextAlignment = TextAlignment.Center;
                Canvas.SetZIndex(textBlock, 2);
                canvas.Children.Add(textBlock);
                Canvas.SetTop(textBlock, TopIndent+ RectBetweenIndent * (i / RectOnOneLine) + 30);
                Canvas.SetLeft(textBlock, LeftIndent + RectBetweenIndent * (i % RectOnOneLine));
                vertices[i].VerticeNameTextBlock = textBlock;

            }
            AddConnectionTool.DrawConnections(canvas, connections);
        }

    }
}
