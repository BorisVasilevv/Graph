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
        readonly List<Vertice> Vertices;
        private Canvas _canvas = MainWindow.MainCanvas;
        private int _counter = 0;

        readonly Dictionary<Shape, string> MessagesToEachShape;

        public AnimaztionPainter(List<Vertice> vertices)
        {
            Vertices = vertices;
        }
        public AnimaztionPainter(Dictionary<Shape, string> messages)
        {
            MessagesToEachShape = messages;
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

        public void ShowAnimation()
        {
            if (Vertices != null)
            {
                for (int i = 0; i < Vertices.Count; i++)
                {
                    if (i == 0) 
                    {
                        _shapes.Add(Vertices[i].Rect);
                    }
                    else 
                    {
                        _shapes.Add(Connection.SearchConnection(Vertices[i - 1], Vertices[i]).Line);
                        _shapes.Add(Vertices[i].Rect);
                    }
                }
            }

            BtnNext.Click += BtnNext_Click;
            _canvas.MouseMove -= AddVerticeTool.clearSelection;
            _canvas.MouseMove -= DeleteConnectionTool.clearSelection;
            _canvas.MouseMove -= ChangeDataTool.TextBlockSelected;
            _canvas.MouseMove -= MainWindow.ToolAddVertice.newRectMouseMove;
            _canvas.MouseDown -= MainWindow.ToolAddVertice.rectMouseDown;

            MainWindow.IsProgramReady = false;
            _canvas.Children.Add(BtnNext);

        }

        private List<Shape> _shapes = new List<Shape>();

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {

            if (_counter == 0)
            {
                _shapes[_counter].Effect = new DropShadowEffect() { Color = Colors.AliceBlue };
            }
            else if (_counter == _shapes.Count-1)
            {
                _shapes[_counter].Effect = null;
                _canvas.MouseMove += AddVerticeTool.clearSelection;
                _canvas.MouseMove += DeleteConnectionTool.clearSelection;
                _canvas.MouseMove += ChangeDataTool.TextBlockSelected;
                _canvas.MouseMove += MainWindow.ToolAddVertice.newRectMouseMove;
                _canvas.MouseDown += MainWindow.ToolAddVertice.rectMouseDown;
                BtnNext.Click -= BtnNext_Click;
                MainWindow.IsProgramReady = true;
                _canvas.Children.Remove(BtnNext);
            }
            else
            {
                _shapes[_counter].Effect = null;
                _shapes[++_counter].Effect = new DropShadowEffect() { Color = Colors.AliceBlue };
            }
        }
    }
}
