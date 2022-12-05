using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace Graph
{
    public class AddVerticeTool
    {
        public static List<Vertice> AllVertices = new List<Vertice>();
        private Rectangle NewVerticeRect;
        public static Rectangle SelectedRectangle;


        public void RectMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.MainCanvas.MouseMove -= NewRectMouseMove;
            MainWindow.MainCanvas.MouseDown -= RectMouseDown;
            NewVerticeRect = null;
            SelectedRectangle = null;
        }

        public void NewRectMouseMove(object sender, MouseEventArgs e)
        {
            Point point = new Point();
            double mouseX = e.GetPosition(MainWindow.MainCanvas).X;
            double mouseY = e.GetPosition(MainWindow.MainCanvas).Y;
            if (NewVerticeRect == null)
            {
                Vertice vertice = new Vertice();
                NewVerticeRect = new Rectangle();
                NewVerticeRect.Width = 50;
                NewVerticeRect.Height = 50;
                NewVerticeRect.Fill = new SolidColorBrush(Colors.Brown);
                NewVerticeRect.Stroke = new SolidColorBrush(Colors.Black);
                NewVerticeRect.MouseMove += RectangleMouseMove;
                Canvas.SetZIndex(NewVerticeRect, 2);
                MainWindow.MainCanvas.Children.Add(NewVerticeRect);
                point.X = mouseX;
                point.Y = mouseY;
                vertice.Rect = NewVerticeRect;
                AllVertices.Add(vertice);
                vertice.RectCenter = point;

                TextBlock textBlock = new TextBlock() { Text = (vertice.Id+1).ToString() };

                textBlock.Height = 20;
                textBlock.Width = 50;
                textBlock.VerticalAlignment = VerticalAlignment.Top;
                textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                textBlock.TextAlignment = TextAlignment.Center;
                Canvas.SetZIndex(textBlock, 2);
                MainWindow.MainCanvas.Children.Add(textBlock);
                Canvas.SetTop(textBlock, mouseY  + 20);
                Canvas.SetLeft(textBlock, mouseX - NewVerticeRect.Height / 2);
                vertice.VerticeNameTextBlock = textBlock;
            }

            



            Canvas.SetLeft(NewVerticeRect, mouseX - NewVerticeRect.Width / 2);
            Canvas.SetTop(NewVerticeRect, mouseY - NewVerticeRect.Height / 2);
        }

        public void RectangleMouseMove(object sender, MouseEventArgs e)
        {
            double mouseX = e.GetPosition(MainWindow.MainCanvas).X;
            double mouseY = e.GetPosition(MainWindow.MainCanvas).Y;
            selectedShapes(MainWindow.MainCanvas);
            DrawSelection();
            if (SelectedRectangle != null && e.LeftButton == MouseButtonState.Pressed)
            {
                Point point = new Point();
                point.X = mouseX;
                point.Y = mouseY + SelectedRectangle.Height / 4;
                Canvas.SetLeft(SelectedRectangle, mouseX - SelectedRectangle.Width / 2);
                Canvas.SetTop(SelectedRectangle, mouseY - SelectedRectangle.Height / 4);
                //RectangleData rectangleData = AddRectangleTool.RectangleDatas[SelectedVertice];


                Vertice vertice = Vertice.SearchVertice(SelectedRectangle);
                vertice.RectCenter = point;

                Canvas.SetTop(vertice.VerticeNameTextBlock, mouseY +20);
                Canvas.SetLeft(vertice.VerticeNameTextBlock, mouseX - SelectedRectangle.Height / 2);
                AddConnectionTool.DrawConnections();


                //AddConnectionTool.DrawConnections(AddConnectionTool.connections);
            }
        }

        public static void clearSelection(object sender, MouseEventArgs e)
        {
            foreach (Vertice vertice in AllVertices)
                if (!vertice.Rect.IsMouseOver)
                {
                    vertice.Rect.Effect = null;
                    if (vertice.Rect == SelectedRectangle) SelectedRectangle = null;
                }
        }

        private void selectedShapes(Canvas canvas)
        {
            foreach (var elem in canvas.Children)
            {
                if (elem is Rectangle rect)
                {
                    if (rect.IsMouseOver)
                    {
                        SelectedRectangle = rect;
                    }
                }
            }
        }

        private void DrawSelection()
        {
            if (SelectedRectangle != null)
                SelectedRectangle.Effect = new DropShadowEffect() { Color = Colors.Green };
        }
    }
}
