﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace Graph
{
    public class AnimaztionPainter
    {
        public enum AlgorithmType
        {
            Traversal,
            FordFarkenson,
            Prim,
            Dijkstra
        }
        MyGraph MainGraph = MainWindow.MainGraph;
        private Canvas _canvas = MainWindow.MainCanvas;
        public List<Shape> Shapes;
        AlgorithmType _type;
        public AnimaztionPainter(AlgorithmType type)
        {
            _type = type;
        }


        Button BtnNext = new Button
        {
            Content = "Next",
            Height = 20,
            Width = 125,
            Background = new SolidColorBrush(Colors.Gray),
            Margin = new Thickness(650, 320, 0, 0),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalContentAlignment = HorizontalAlignment.Center

        };


        Button BtnReturn = new Button
        {
            Content = "To original graph",
            Height = 20,
            Width = 125,
            Background = new SolidColorBrush(Colors.Gray),
            Margin = new Thickness(650, 350, 0, 0),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalContentAlignment = HorizontalAlignment.Center

        };

        public void ShowAnimation()
        {
            _shapes = Shapes;
            //for (int i = 0; i < Vertices.Count; i++)
            //{
            //    if (i == 0)
            //    {
            //        _shapes.Add(Vertices[i].Rect);
            //    }
            //    else
            //    {
            //        _shapes.Add(Connection.SearchConnection(Vertices[i - 1], Vertices[i]).Line);
            //        _shapes.Add(Vertices[i].Rect);
            //    }
            //}

            BtnReturn.Click += BtnReturn_Click;
            if (_type == AlgorithmType.Traversal) BtnNext.Click += BtnNextTraversal_Click;
            else if (_type == AlgorithmType.Prim) BtnNext.Click += BtnNextPrim_Click;
            else if (_type == AlgorithmType.FordFarkenson) BtnNext.Click += BtnNextFordFarkenson_Click;
            else if (_type == AlgorithmType.Dijkstra) BtnNext.Click += BtnNextDijkstra_Click;

            _canvas.MouseMove -= AddVerticeTool.clearSelection;
            _canvas.MouseMove -= DeleteConnectionTool.clearSelection;
            _canvas.MouseMove -= ChangeDataTool.TextBlockSelected;
            _canvas.MouseMove -= MainWindow.ToolAddVertice.newRectMouseMove;
            _canvas.MouseDown -= MainWindow.ToolAddVertice.rectMouseDown;
            foreach (var v in MainGraph.AllVertices)
                v.Rect.MouseMove -= MainWindow.ToolAddVertice.RectangleMouseMove;


            MainWindow.IsProgramReady = false;
            _canvas.Children.Add(BtnNext);
            Canvas.SetZIndex(BtnNext, 20);
            _canvas.Children.Add(BtnReturn);
            Canvas.SetZIndex(BtnReturn, 20);
        }

        private void BtnReturn_Click(object sender, RoutedEventArgs e)
        {

            _canvas.MouseMove += AddVerticeTool.clearSelection;
            _canvas.MouseMove += DeleteConnectionTool.clearSelection;
            _canvas.MouseMove += ChangeDataTool.TextBlockSelected;
            _canvas.MouseDown += MainWindow.ToolAddVertice.rectMouseDown;
            BtnNext.Click -= BtnNextTraversal_Click;
            foreach (Shape shape in _shapes)
                shape.Effect = null;

            foreach (var v in MainGraph.AllVertices)
                v.Rect.MouseMove += MainWindow.ToolAddVertice.RectangleMouseMove;

            MainWindow.IsProgramReady = true;
            if (_canvas.Children.Contains(BtnNext)) _canvas.Children.Remove(BtnNext);
            _canvas.Children.Remove(BtnReturn);

        }

        private List<Shape> _shapes = new List<Shape>();


        int _counter = 0;
        private void BtnNextTraversal_Click(object sender, RoutedEventArgs e)
        {

            if (_counter < _shapes.Count)
            {
                _shapes[_counter].Effect = new DropShadowEffect() { Color = Colors.Black };
                _counter++;
                if (_counter == _shapes.Count) _canvas.Children.Remove(BtnNext);
            }

        }


        private void BtnNextPrim_Click(object sender, RoutedEventArgs e)
        {

            if (_counter < _shapes.Count)
            {
                _shapes[_counter].Effect = new DropShadowEffect() { Color = Colors.Black };
                _counter++;
                if (_counter == _shapes.Count) _canvas.Children.Remove(BtnNext);
            }

        }


        private void BtnNextFordFarkenson_Click(object sender, RoutedEventArgs e)
        {

        }


        private void BtnNextDijkstra_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
