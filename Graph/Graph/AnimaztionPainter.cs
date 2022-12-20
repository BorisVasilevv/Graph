using System;
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
        public List<Shape> Shapes { get; private set; }
        public List<(Connection,string)> ConnectionDescriptions { get; private set; }

        AlgorithmType _type;

        public AnimaztionPainter(AlgorithmType type)
        {
            _type = type;
            if(type == AlgorithmType.Traversal|| type == AlgorithmType.Prim) 
                Shapes = new List<Shape>();
            if (type==AlgorithmType.FordFarkenson)
                ConnectionDescriptions = new List<(Connection, string)>();
        }


        Button BtnNext = null;


        Button BtnReturn = null;

        public void ShowAnimation()
        {
            if (BtnReturn == null) BtnReturn = DrawHelper.BtnReturn;
            if (BtnNext == null) BtnNext = DrawHelper.BtnNext;
            _shapes = Shapes;
            

            BtnReturn.Click += BtnReturn_Click;
            if (_type == AlgorithmType.Traversal)
            {
                BtnNext.Click += BtnNextTraversal_Click;
                
            }
            else if (_type == AlgorithmType.Prim)
            {
                BtnNext.Click += BtnNextPrim_Click;
                
            }
            else if (_type == AlgorithmType.FordFarkenson)
            {
                BtnNext.Click += BtnNextFordFarkenson_Click;
            }
            else if (_type == AlgorithmType.Dijkstra)
            {
                BtnNext.Click += BtnNextDijkstra_Click;
            }
            _canvas.Children.Add(BtnReturn);
            Canvas.SetZIndex(BtnReturn, 20);

            _canvas.MouseMove -= AddVerticeTool.clearSelection;
            _canvas.MouseMove -= DeleteConnectionTool.clearSelection;
            _canvas.MouseMove -= ChangeDataTool.TextBlockSelected;
            _canvas.MouseMove -= MainWindow.ToolAddVertice.newRectMouseMove;
            _canvas.MouseDown -= MainWindow.ToolAddVertice.rectMouseDown;
            foreach (var v in MainGraph.AllVertices)
                v.Rect.MouseMove -= MainWindow.ToolAddVertice.RectangleMouseMove;


            MainWindow.IsUserCanUseButtons = false;
            _canvas.Children.Add(BtnNext);
            Canvas.SetZIndex(BtnNext, 20);
            
        }

        private void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            BtnReturn.Click -= BtnReturn_Click;
            if (_canvas.Children.Contains(BtnNext)) _canvas.Children.Remove(BtnNext);
            _canvas.Children.Remove(BtnReturn);
            _canvas.MouseMove += AddVerticeTool.clearSelection;
            _canvas.MouseMove += DeleteConnectionTool.clearSelection;
            _canvas.MouseMove += ChangeDataTool.TextBlockSelected;
            _canvas.MouseDown += MainWindow.ToolAddVertice.rectMouseDown;
            BtnNext.Click -= BtnNextTraversal_Click;
            if(_shapes!=null)
                foreach (Shape shape in _shapes)
                    shape.Effect = null;

            if (ConnectionDescriptions != null)
            {
                foreach (Connection connection in MainWindow.MainGraph.Connections)
                {
                    connection.BlockText.Text = connection.Length.ToString();
                    connection.BlockText.MouseDown += ChangeDataTool.TextBlock_MouseDown;
                }
            }

            foreach (var v in MainGraph.AllVertices)
                v.Rect.MouseMove += MainWindow.ToolAddVertice.RectangleMouseMove;

            MainWindow.IsUserCanUseButtons = true;

            while (_canvas.Children.Contains(BtnNext)) _canvas.Children.Remove(BtnNext);
            while (_canvas.Children.Contains(BtnReturn)) _canvas.Children.Remove(BtnReturn);

        }

        private List<Shape> _shapes = new List<Shape>();


        int _counter = 0;
        private void BtnNextTraversal_Click(object sender, RoutedEventArgs e)
        {

            if (_counter < _shapes.Count)
            {
                _shapes[_counter].Effect = new DropShadowEffect() { Color = Colors.Black };
                
            }
            _counter++;
            if (_counter >= _shapes.Count) _canvas.Children.Remove(BtnNext);

        }


        private void BtnNextPrim_Click(object sender, RoutedEventArgs e)
        {

            if (_counter < _shapes.Count)
            {
                _shapes[_counter].Effect = new DropShadowEffect() { Color = Colors.Black };               
            }
            if (_counter == _shapes.Count)
            {
                _canvas.Children.Clear();
                DrawHelper.DrawGraph(_canvas, MainGraph);
                DrawHelper.BtnReturn.Click += MainWindow.BtnReturn_Click;
                _canvas.Children.Remove(BtnReturn);

                _canvas.MouseMove += AddVerticeTool.clearSelection;
                _canvas.MouseMove += DeleteConnectionTool.clearSelection;
                _canvas.MouseMove += ChangeDataTool.TextBlockSelected;
                _canvas.MouseDown += MainWindow.ToolAddVertice.rectMouseDown;
                BtnNext.Click -= BtnNextTraversal_Click;
                foreach (Shape shape in _shapes)
                    shape.Effect = null;

                foreach (var v in MainGraph.AllVertices)
                    v.Rect.MouseMove += MainWindow.ToolAddVertice.RectangleMouseMove;

                MainWindow.IsUserCanUseButtons = true;

                _canvas.Children.Add(DrawHelper.BtnReturn);
                Canvas.SetZIndex(DrawHelper.BtnReturn, 20);
            }
            _counter++;
        }

        public static void ReturnFunctionality()
        {

        }


        private void BtnNextFordFarkenson_Click(object sender, RoutedEventArgs e)
        {
            if (_counter < ConnectionDescriptions.Count)
            {
                ConnectionDescriptions[_counter].Item1.BlockText.Text = ConnectionDescriptions[_counter].Item2;
            }
            _counter++;
            if (_counter >= ConnectionDescriptions.Count)
            {
                _canvas.Children.Remove(BtnNext);

                foreach(Connection conn in MainGraph.Connections)
                {
                    conn.BlockText.Text = $"{AlgorithmFordFarkenson.ResultLength[conn]}/{conn.Length}";
                }
            }

           
        }


        private void BtnNextDijkstra_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
