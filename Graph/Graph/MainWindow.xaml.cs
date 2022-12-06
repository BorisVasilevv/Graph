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



        private void btnFileName_Click(object sender, RoutedEventArgs e)
        {

            Button btn = (Button)sender;
            FileToWork += btn.Content.ToString();
            IsProgramReady = true;
            canvas1.Children.Clear();
            (AddVerticeTool.AllVertices, AddConnectionTool.Connections) = FileWorker.Read(FileToWork);
            canvas1.MouseMove += AddVerticeTool.clearSelection;
            canvas1.MouseMove += DeleteConnectionTool.clearSelection;
            canvas1.MouseMove += ChangeDataTool.TextBlockSelected;

            for (int i = 0; i < AddVerticeTool.AllVertices.Count; i++)
            {
                Rectangle Rect = new Rectangle();
                Rect.Width = 50;
                Rect.Height = 50;
                Rect.Fill = new SolidColorBrush(Colors.Brown);
                Rect.Stroke = new SolidColorBrush(Colors.Black);
                Rect.MouseMove += ToolAddVertice.RectangleMouseMove;
                Canvas.SetZIndex(Rect, 2);
                MainWindow.MainCanvas.Children.Add(Rect);
                Point center = new Point(200 + 70 * (i % 5), 50 * (i / 5 + 1));
                Canvas.SetTop(Rect, center.Y);
                Canvas.SetLeft(Rect, center.X);

                AddVerticeTool.AllVertices[i].Rect = Rect;
                AddVerticeTool.AllVertices[i].RectCenter = new Point(center.X + Rect.Width / 2, center.Y + Rect.Height / 2);
                TextBlock textBlock = new TextBlock() { Text = (AddVerticeTool.AllVertices[i].Id + 1).ToString() };

                textBlock.Height = 20;
                textBlock.Width = 50;
                textBlock.VerticalAlignment = VerticalAlignment.Top;
                textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                textBlock.TextAlignment = TextAlignment.Center;
                Canvas.SetZIndex(textBlock, 2);
                MainWindow.MainCanvas.Children.Add(textBlock);
                Canvas.SetTop(textBlock, 50 * (i / 5 + 1) + 30);
                Canvas.SetLeft(textBlock, 200 + 70 * (i % 5));
                AddVerticeTool.AllVertices[i].VerticeNameTextBlock = textBlock;

            }
            AddConnectionTool.DrawConnections();
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
            TextBox textBox = new TextBox();
            textBox.VerticalAlignment = VerticalAlignment.Top;
            textBox.HorizontalAlignment = HorizontalAlignment.Left;
            textBox.Width = 735;
            textBox.Height = 30;
            textBox.KeyDown += TextBox_KeyDown;
            Canvas.SetTop(textBox, 40);
            Canvas.SetLeft(textBox, 65);
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
                FileWorker.WriteToFile(AddVerticeTool.AllVertices,AddConnectionTool.Connections ,FileToWork);
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

        }

        private void btnMinWay_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnMaxFlow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnMinTree_Click(object sender, RoutedEventArgs e)
        {

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
