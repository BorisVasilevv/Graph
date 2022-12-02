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
        Canvas MainCanvas;
        public static string FileToWork= "..\\..\\..\\..\\";
        public bool IsProgramReady = false;
        public List<Vertice> Vertices;
        public List<Connection> Connections;

        public MainWindow()
        {
            InitializeComponent();
            MainCanvas = canvas1;
            TextBlock textBlock = new TextBlock();
            textBlock.VerticalAlignment = VerticalAlignment.Top;
            textBlock.HorizontalAlignment = HorizontalAlignment.Left;
            textBlock.Width = 735;
            textBlock.Height = 30;
            textBlock.Text = "Выберите Файл для работы";
            canvas1.Children.Add(textBlock);
            Canvas.SetLeft(textBlock, 65);
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
            (Vertices,Connections) = FileWorker.Read(FileToWork);
            int p = 0;
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
            Canvas.SetLeft(textBlock, 150);
            TextBox textBox = new TextBox();
            textBox.VerticalAlignment = VerticalAlignment.Top;
            textBox.HorizontalAlignment = HorizontalAlignment.Left;
            textBox.Width = 735;
            textBox.Height = 30;
            textBox.KeyDown += TextBox_KeyDown;
            Canvas.SetTop(textBox, 40);
            Canvas.SetLeft(textBox, 65);
            canvas1.Children.Add(textBox);
        }

        private void btnCreateVertice_Click(object sender, RoutedEventArgs e)
        {
            if (IsProgramReady)
            {
                
            }
        }

        private void btnAddConnection_Click(object sender, RoutedEventArgs e)
        {
            if (IsProgramReady)
            {
                
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
            
        }

        private void btnExit_click(object sender, RoutedEventArgs e)
        {
            //Сохранить или нет
            Environment.Exit(0);
        }

        private void btnDeleteVertice_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
