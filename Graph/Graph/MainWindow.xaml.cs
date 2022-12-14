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
            textBlock.Text = "Choose file to work";


            canvas1.Children.Add(textBlock);
            Canvas.SetLeft(textBlock, 200);
            string[] allFiles = Directory.GetFiles("..\\..\\..\\..\\");
            Button btn = new Button();
            btn.Width = 100;
            btn.Height = 30;
            btn.VerticalAlignment = VerticalAlignment.Top;
            btn.HorizontalAlignment = HorizontalAlignment.Left;
            canvas1.Children.Add(btn);
            Canvas.SetLeft(btn, 110);
            Canvas.SetTop(btn, 20);
            btn.Content = "New File";
            btn.Click += btnCreateNewFile_Click;
            int buttonCounter = 1;
            const int ButtonIndent = 10;
            foreach (string file in allFiles)
            {
                string extension = System.IO.Path.GetExtension(file);
                
                if (extension == ".csv")
                {
                    string mainNameOfFile = System.IO.Path.GetFileNameWithoutExtension(file);
                    Button button = new Button();
                    button.Width = 100;
                    button.Height = 30;
                    button.VerticalAlignment = VerticalAlignment.Top;
                    button.HorizontalAlignment = HorizontalAlignment.Left;
                    canvas1.Children.Add(button);
                    Canvas.SetLeft(button, (button.Width + ButtonIndent) * (buttonCounter / 10 + 1));
                    Canvas.SetTop(button, (button.Height + ButtonIndent) * (buttonCounter % 10 + 1)-2*ButtonIndent);
                    buttonCounter++;
                    button.Content = mainNameOfFile;
                    button.Click += btnFileName_Click;
                }
            }


        }

        public static string FullFileNamePath = null;
        public static MyGraph MainGraph;
        private static MyGraph _mainGraphCopy;
        private void btnFileName_Click(object sender, RoutedEventArgs e)
        {

            Button btn = (Button)sender;
            FileToWork += btn.Content.ToString();
            FullFileNamePath= FileToWork;
            IsProgramReady = true;
            canvas1.Children.Clear();

            MainGraph = FileWorker.Read(FileToWork);
 

            canvas1.MouseMove += AddVerticeTool.clearSelection;
            canvas1.MouseMove += DeleteConnectionTool.clearSelection;
            canvas1.MouseMove += ChangeDataTool.TextBlockSelected;

            DrawHelper.DrawGraph(MainCanvas, MainGraph);
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
            MainGraph = new MyGraph(new List<Vertice>(), new List<Connection>());
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
                FileWorker.WriteToFile(MainGraph, FileToWork);
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
                DrawHelper.BtnSearchDepth.Click += BtnSearchDetpth_Click;
                DrawHelper.BtnSearchWidth.Click += BtnSearchWidth_Click;
                if (!MainCanvas.Children.Contains(DrawHelper.BtnSearchDepth))
                {
                    MainCanvas.Children.Add(DrawHelper.BtnSearchDepth);
                    Canvas.SetZIndex(DrawHelper.BtnSearchDepth, 20);
                }

                if (!MainCanvas.Children.Contains(DrawHelper.BtnSearchWidth))
                {
                    MainCanvas.Children.Add(DrawHelper.BtnSearchWidth);
                    Canvas.SetZIndex(DrawHelper.BtnSearchWidth, 20);
                }
            }
        }

        private void BtnSearchWidth_Click(object sender, RoutedEventArgs e)
        {
            MainCanvas.Children.Remove(DrawHelper.BtnSearchDepth);
            MainCanvas.Children.Remove(DrawHelper.BtnSearchWidth);
            Traversal.BFS(MainGraph.AllVertices, MainGraph.Connections);
            
        }

        private void BtnSearchDetpth_Click(object sender, RoutedEventArgs e)
        {
            MainCanvas.Children.Remove(DrawHelper.BtnSearchDepth);
            MainCanvas.Children.Remove(DrawHelper.BtnSearchWidth);
            Traversal.DFS(MainGraph.AllVertices, MainGraph.Connections);
        }

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


        private void btnMinTree_Click(object sender, RoutedEventArgs e)
        {
            _mainGraphCopy=new MyGraph(MainGraph);
            bool IsGraphConnected = true;
            if (IsProgramReady)
            {
                if (IsGraphConnected)
                {
                    MainGraph.Connections = PrimAlghoritm.AlgorithmByPrim(MainGraph);
                    MainCanvas.Children.Clear();
                    DrawHelper.DrawGraph(MainCanvas, MainGraph);
                    DrawHelper.BtnReturn.Click += BtnReturn_Click;

                    MainCanvas.Children.Add(DrawHelper.BtnReturn);
                    Canvas.SetZIndex(DrawHelper.BtnReturn, 20);
                }

            }
        }

        private void btnMinWay_Click(object sender, RoutedEventArgs e)
        {
            _mainGraphCopy = new MyGraph(MainGraph);

            if (IsProgramReady)
            {
                IsProgramReady = false;
                ChoseStartEndHelper.ChooseVertices(DijkstraAlgorithm.Algorithm);

            }
        }

        public static void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            MainCanvas.Children.Clear();
            MainGraph = _mainGraphCopy == null? MainGraph: _mainGraphCopy;
            
            DrawHelper.DrawGraph(MainCanvas, MainGraph);
            DrawHelper.BtnReturn.Click-= BtnReturn_Click;

            _mainGraphCopy = null;
        }
     


        private void btnDeleteConnection_Click(object sender, RoutedEventArgs e)
        {
            if (IsProgramReady)
            {
                canvas1.MouseDown += DeleteConnectionTool.deleteLine;
            }
        }
    }
}
