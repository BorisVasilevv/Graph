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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Canvas MainCanvas;
        public static string FileToWork = "..\\..\\..\\..\\";
        public static bool IsProgramReady = false;
        public static AddVerticeTool ToolAddVertice;
        public AddConnectionTool ToolAddConnedtion;


        public MainWindow()
        {
            InitializeComponent();
            ToolAddVertice = new AddVerticeTool();
            MainCanvas = canvas1;
            TextBlock textBlock = new TextBlock();
            textBlock.VerticalAlignment = VerticalAlignment.Top;
            textBlock.HorizontalAlignment = HorizontalAlignment.Left;
            textBlock.Width = 735;
            textBlock.Height = 30;
            textBlock.Text = "Выберите Файл для работы";


            canvas1.Children.Add(textBlock);
            Canvas.SetLeft(textBlock, 200);
            string[] allFiles = Directory.GetFiles("..\\..\\..\\..\\");
            Button btn = new Button();
            btn.Width = 100;
            btn.Height = 30;
            btn.VerticalAlignment = VerticalAlignment.Top;
            btn.HorizontalAlignment = HorizontalAlignment.Left;
            canvas1.Children.Add(btn);
            Canvas.SetLeft(btn, 100);
            Canvas.SetTop(btn, 40);
            btn.Content = "Новый файл";
            btn.Click += btnCreateNewFile_Click;
            int buttonCounter = 1;
            foreach (string file in allFiles)
            {
                string extension = System.IO.Path.GetExtension(file);
                string mainNameOfFile = System.IO.Path.GetFileName(file);
                if (extension == ".csv")
                {
                    Button button = new Button();
                    button.Width = 100;
                    button.Height = 30;
                    button.VerticalAlignment = VerticalAlignment.Top;
                    button.HorizontalAlignment = HorizontalAlignment.Left;
                    canvas1.Children.Add(button);
                    Canvas.SetLeft(button, 100 * (buttonCounter / 10 + 1));
                    Canvas.SetTop(button, 40 * (buttonCounter % 10 + 1));
                    buttonCounter++;
                    button.Content = mainNameOfFile;
                    button.Click += btnFileName_Click;
                }
            }


        }

        public static string FullFileNamePath = null;

        private void btnFileName_Click(object sender, RoutedEventArgs e)
        {

            Button btn = (Button)sender;
            FileToWork += btn.Content.ToString();
            FullFileNamePath= FileToWork;
            IsProgramReady = true;
            canvas1.Children.Clear();
            (AddVerticeTool.AllVertices, AddConnectionTool.Connections) = FileWorker.Read(FileToWork);
            canvas1.MouseMove += AddVerticeTool.clearSelection;
            canvas1.MouseMove += DeleteConnectionTool.clearSelection;
            canvas1.MouseMove += ChangeDataTool.TextBlockSelected;

            DrawGraphHelper.DrawGraph(MainCanvas, AddVerticeTool.AllVertices, AddConnectionTool.Connections);
        }


        

        private void btnCreateNewFile_Click(object sender, RoutedEventArgs e)
        {
            canvas1.Children.Clear();
            TextBlock textBlock = new TextBlock();



            textBlock.VerticalAlignment = VerticalAlignment.Top;
            textBlock.HorizontalAlignment = HorizontalAlignment.Left;
            textBlock.Width = 735;
            textBlock.Height = 30;
            textBlock.Text = "Введите имя файла ниже и нажмите Enter";
            canvas1.Children.Add(textBlock);
            Canvas.SetLeft(textBlock, 200);
            Canvas.SetTop(textBlock, 10);
            TextBox textBox = new TextBox();
            textBox.VerticalAlignment = VerticalAlignment.Top;
            textBox.HorizontalAlignment = HorizontalAlignment.Left;
            textBox.Width = 400;
            textBox.Height = 20;
            textBox.KeyDown += TextBox_KeyDown;
            Canvas.SetTop(textBox, 30);
            Canvas.SetLeft(textBox, 200);
            canvas1.Children.Add(textBox);
            AddVerticeTool.AllVertices = new List<Vertice>();
            AddConnectionTool.Connections = new List<Connection>();
            canvas1.MouseMove += AddVerticeTool.clearSelection;
            canvas1.MouseMove += DeleteConnectionTool.clearSelection;
            canvas1.MouseMove += ChangeDataTool.TextBlockSelected;

        }

        private void btnCreateVertice_Click(object sender, RoutedEventArgs e)
        {
            if (IsProgramReady)
            {
                canvas1.MouseMove += ToolAddVertice.newRectMouseMove;
                canvas1.MouseDown += ToolAddVertice.rectMouseDown;
            }
        }

        private void btnAddConnection_Click(object sender, RoutedEventArgs e)
        {
            if (IsProgramReady)
            {
                canvas1.MouseDown += AddConnectionTool.addConnection;
            }
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (e.Key == Key.Enter)
            {
                string nameToNewFile = textBox.Text;
                FileToWork = "..\\..\\..\\..\\" + nameToNewFile + ".csv";
                IsProgramReady = true;
                canvas1.Children.Clear();
            }
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (IsProgramReady)
            {
                FileWorker.WriteToFile(AddVerticeTool.AllVertices, FileToWork);
            }
        }

        private void btnExit_click(object sender, RoutedEventArgs e)
        {
            //Сохранить или нет
            Environment.Exit(0);
        }

        private void btnDeleteVertice_Click(object sender, RoutedEventArgs e)
        {
            if (IsProgramReady)
            {
                canvas1.MouseDown += DeleteVerticeTool.DeleteVertice;
            }
        }

        private void btnTraversal_Click(object sender, RoutedEventArgs e)
        {
            if (IsProgramReady)
            {
                BtnSearchDepth.Click += BtnSearchDetpth_Click;
                BtnSearchWidth.Click += BtnSearchWidth_Click;
                if (!MainCanvas.Children.Contains(BtnSearchDepth))
                {
                    MainCanvas.Children.Add(BtnSearchDepth);
                    Canvas.SetZIndex(BtnSearchDepth, 20);
                }

                if (!MainCanvas.Children.Contains(BtnSearchWidth))
                {
                    MainCanvas.Children.Add(BtnSearchWidth);
                    Canvas.SetZIndex(BtnSearchWidth, 20);
                }
            }
        }

        private void BtnSearchWidth_Click(object sender, RoutedEventArgs e)
        {
            MainCanvas.Children.Remove(BtnSearchDepth);
            MainCanvas.Children.Remove(BtnSearchWidth);
            Traversal.BFS(AddVerticeTool.AllVertices, AddConnectionTool.Connections);
            
        }

        private void BtnSearchDetpth_Click(object sender, RoutedEventArgs e)
        {
            MainCanvas.Children.Remove(BtnSearchDepth);
            MainCanvas.Children.Remove(BtnSearchWidth);
            Traversal.DFS(AddVerticeTool.AllVertices, AddConnectionTool.Connections);
        }

        Button BtnSearchDepth = new Button
        {
            Content = "In depth",
            Height = 20,
            Width = 100,
            Background = new SolidColorBrush(Colors.Gray),
            Margin = new Thickness(80, 170, 0, 0),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalContentAlignment = HorizontalAlignment.Center

        };

        Button BtnSearchWidth = new Button
        {
            Content = "In Width",
            Height = 20,
            Width = 100,
            Background = new SolidColorBrush(Colors.Gray),
            Margin = new Thickness(80, 190, 0, 0),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalContentAlignment = HorizontalAlignment.Center

        };



        private void btnMaxFlow_Click(object sender, RoutedEventArgs e)
        {
            if (IsProgramReady)
            {
                IsProgramReady = false;
                ChoseStartEndHelper.ChooseVertices(AlgorithmFordFarkenson.Algorithm);
            }
        }

        Canvas dopCanvas = new Canvas
        {
            Height = 450,
            Width = 800,
            Background = new SolidColorBrush(Colors.White)
        };

        static List<Connection> _copyConnection;
        static List<Vertice> _copyVertice;

        private void btnMinTree_Click(object sender, RoutedEventArgs e)
        {
            _copyConnection = new List<Connection>(AddConnectionTool.Connections);
            _copyVertice = new List<Vertice>(AddVerticeTool.AllVertices);
            bool IsGraphConnected = true;
            if (IsProgramReady)
            {
                if (IsGraphConnected)
                {
                    List<Connection> connections = PrimAlghoritm.AlgorithmByPrim(_copyConnection);
                    MainCanvas.Children.Clear();
                    DrawGraphHelper.DrawGraph(MainCanvas, AddVerticeTool.AllVertices, connections);
                    BtnReturn.Click += BtnReturn_Click;

                    MainCanvas.Children.Add(BtnReturn);
                    Canvas.SetZIndex(BtnReturn, 20);
                }

            }
        }

        private void btnMinWay_Click(object sender, RoutedEventArgs e)
        {
            _copyConnection = new List<Connection>(AddConnectionTool.Connections);
            _copyVertice = new List<Vertice>(AddVerticeTool.AllVertices);
            if (IsProgramReady)
            {
                IsProgramReady = false;
                ChoseStartEndHelper.ChooseVertices(DijkstraAlgorithm.Algorithm);

            }
        }

        public static void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            MainCanvas.Children.Clear();
            AddConnectionTool.Connections = _copyConnection==null? AddConnectionTool.Connections: _copyConnection;
            AddVerticeTool.AllVertices = _copyVertice == null ? AddVerticeTool.AllVertices : _copyVertice;
            DrawGraphHelper.DrawGraph(MainCanvas, AddVerticeTool.AllVertices, AddConnectionTool.Connections);
            BtnReturn.Click-=BtnReturn_Click;
            _copyVertice = null;
            _copyConnection = null;
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






        private void btnDeleteConnection_Click(object sender, RoutedEventArgs e)
        {
            if (IsProgramReady)
            {
                canvas1.MouseDown += DeleteConnectionTool.deleteLine;
            }
        }
    }
}
