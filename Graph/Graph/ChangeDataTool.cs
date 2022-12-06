using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media.Effects;

namespace Graph
{
    public class ChangeDataTool
    {
        static Button BtnReadyToAddData = new Button();

        static TextBlock TextBlock = new TextBlock();

        static TextBox TextBox = new TextBox();

        private static TextBlock SelectedTextBlock;

        private static TextBlock TextBlockToChange;
        private static Rectangle RectToAddData;

        static TextBlock TextBlockTellUser;

        public static void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MainWindow.IsProgramReady = false;
            TextBlockToChange = SelectedTextBlock;

            if (RectToAddData == null)
            {
                RectToAddData = new Rectangle();
                RectToAddData.Height = 450;
                RectToAddData.Width = 180;
                RectToAddData.HorizontalAlignment = HorizontalAlignment.Left;
                RectToAddData.VerticalAlignment = VerticalAlignment.Top;
                RectToAddData.Fill = new SolidColorBrush(Colors.Brown);
            }

            if (SelectedTextBlock != null && e.LeftButton == MouseButtonState.Pressed)
            {
                MainWindow.MainCanvas.Children.Add(RectToAddData);
                Canvas.SetZIndex(RectToAddData, 8);
                Canvas.SetLeft(RectToAddData, MainWindow.MainCanvas.Width - RectToAddData.Width - 15);


                TextBox.Height = 20;
                TextBox.Width = 150;
                TextBox.TextAlignment = TextAlignment.Left;
                TextBox.VerticalAlignment = VerticalAlignment.Top;
                TextBox.HorizontalAlignment = HorizontalAlignment.Center;
                Canvas.SetZIndex(TextBox, 10);
                Canvas.SetTop(TextBox, 60);
                Canvas.SetLeft(TextBox, MainWindow.MainCanvas.Width - RectToAddData.Width);
                MainWindow.MainCanvas.Children.Add(TextBox);


                TextBlock.Height = 40;
                TextBlock.Width = 150;
                TextBlock.Text = "Введите навую длинну или\n пропускную способность";
                Canvas.SetTop(TextBlock, 20);
                Canvas.SetLeft(TextBlock, MainWindow.MainCanvas.Width - RectToAddData.Width);
                Canvas.SetZIndex(TextBlock, 10);
                MainWindow.MainCanvas.Children.Add(TextBlock);



                BtnReadyToAddData.Height = 20;
                BtnReadyToAddData.Width = 150;
                Canvas.SetTop(BtnReadyToAddData, 100);
                Canvas.SetLeft(BtnReadyToAddData, MainWindow.MainCanvas.Width - RectToAddData.Width);
                BtnReadyToAddData.Content = "Готово";
                BtnReadyToAddData.HorizontalContentAlignment = HorizontalAlignment.Center;
                BtnReadyToAddData.VerticalContentAlignment = VerticalAlignment.Center;
                Canvas.SetZIndex(BtnReadyToAddData, 10);
                MainWindow.MainCanvas.Children.Add(BtnReadyToAddData);
                BtnReadyToAddData.Click += btnReadyToAddData_Click;
            }
            MainWindow.MainCanvas.MouseDown += MainWindow.ToolAddVertice.RectangleMouseMove;
            MainWindow.MainCanvas.MouseDown -= TextBlock_MouseDown;
        }


        public static void btnReadyToAddData_Click(object sender, RoutedEventArgs e)
        {
            while(true)
            {
                string inputStr = TextBox.Text;
                int number;
                if (Int32.TryParse(inputStr, out number))
                {
                    if (number <= 0)
                    {
                        ShowUserErrorMesage();
                    }
                    else
                    {
                        Connection connectionToChange= Connection.SearchConnection(TextBlockToChange);
                        connectionToChange.Length = number;
                        connectionToChange.BlockText.Text = inputStr;
                        break;
                    }
                }
                else
                {
                    ShowUserErrorMesage();
                }
            }
            MainWindow.IsProgramReady = true;
            MainWindow.MainCanvas.Children.Remove(TextBlock);
            MainWindow.MainCanvas.Children.Remove(BtnReadyToAddData);
            MainWindow.MainCanvas.Children.Remove(RectToAddData);
            MainWindow.MainCanvas.Children.Remove(TextBox);
            if (MainWindow.MainCanvas.Children.Contains(TextBlockTellUser)) MainWindow.MainCanvas.Children.Remove(TextBlockTellUser);

        }


        public static void ShowUserErrorMesage()
        {
            TextBlockTellUser = new TextBlock();
            TextBlockTellUser.Height = 40;
            TextBlockTellUser.Width = 150;
            Canvas.SetTop(TextBlockTellUser, 120);
            Canvas.SetLeft(TextBlockTellUser, MainWindow.MainCanvas.Width - RectToAddData.Width);
            Canvas.SetZIndex(TextBlockTellUser, 10);
            TextBlockTellUser.Text = "Введённые данные некорректны";
            MainWindow.MainCanvas.Children.Add(TextBlockTellUser);
        }

        public static void TextBlockSelected(object sender, MouseEventArgs e)
        {
            bool isSelect = false;
            foreach (Connection connection in AddConnectionTool.Connections)
            {
                if (connection.BlockText.IsMouseOver)
                {
                    connection.BlockText.Effect = new DropShadowEffect() { Color = Colors.Black };
                    SelectedTextBlock = connection.BlockText;
                    isSelect = true;
                }
                else
                {
                    connection.BlockText.Effect = null;
                }
            }

            if (!isSelect)
            {
                SelectedTextBlock = null;
            }
        }

    }
}
