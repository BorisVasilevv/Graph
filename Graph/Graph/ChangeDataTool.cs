﻿using System;
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

        static Button BtnExit = new Button();

        static TextBlock TextBlock = new TextBlock();

        static TextBox TextBox = new TextBox();

        static MyGraph MainGraph = MainWindow.MainGraph;
        private static TextBlock SelectedTextBlock;

        private static TextBlock TextBlockToChange;
        private static Rectangle RectToAddData;

        static TextBlock TextBlockTellUser;
        static double Length = MainWindow.MainCanvas.Width;
        static bool IsElemsAdd = false;
        public static void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TextBox.Text = "";
            MainWindow.IsProgramReady = false;

            Connection connectionToChange ;
            if (TextBlockToChange != null && TextBlockToChange != SelectedTextBlock)
            {
                
                TextBlockToChange = SelectedTextBlock;
                connectionToChange = Connection.SearchConnection(TextBlockToChange,MainGraph.Connections);
                TextBlock.Text = $"Введите навую длинну или\nпропускную способность " +
                    $"\nмежду {connectionToChange.Vertice1.VerticeNameTextBlock.Text} и " +
                    $"{connectionToChange.Vertice2.VerticeNameTextBlock.Text}";
            }
            else if(TextBlockToChange == null)
            {
                TextBlockToChange = SelectedTextBlock;
                connectionToChange = Connection.SearchConnection(TextBlockToChange, MainGraph.Connections);
            }
            else
            {
                connectionToChange = Connection.SearchConnection(TextBlockToChange, MainGraph.Connections);
            }


            if (RectToAddData == null)
            {
                RectToAddData = new Rectangle();
                RectToAddData.Height = 450;
                RectToAddData.Width = 180;
                RectToAddData.HorizontalAlignment = HorizontalAlignment.Left;
                RectToAddData.VerticalAlignment = VerticalAlignment.Top;
                RectToAddData.Fill = new SolidColorBrush(Colors.Brown);
            }
            
            if (SelectedTextBlock != null && e.LeftButton == MouseButtonState.Pressed &&!IsElemsAdd)
            {
                IsElemsAdd = true;
                MainWindow.MainCanvas.Children.Add(RectToAddData);
                Canvas.SetZIndex(RectToAddData, 8);
                Canvas.SetLeft(RectToAddData, Length - RectToAddData.Width - 15);


                TextBox.Height = 20;
                TextBox.Width = 150;
                TextBox.TextAlignment = TextAlignment.Left;
                TextBox.VerticalAlignment = VerticalAlignment.Top;
                TextBox.HorizontalAlignment = HorizontalAlignment.Center;
                Canvas.SetZIndex(TextBox, 10);
                Canvas.SetTop(TextBox, 80);
                Canvas.SetLeft(TextBox, Length - RectToAddData.Width);
                MainWindow.MainCanvas.Children.Add(TextBox);


                TextBlock.Height = 60;
                TextBlock.Width = 150;
                TextBlock.Text =  $"Введите навую длинну или\nпропускную способность " +
                    $"\nмежду {connectionToChange.Vertice1.VerticeNameTextBlock.Text} и " +
                    $"{connectionToChange.Vertice2.VerticeNameTextBlock.Text}";
                Canvas.SetTop(TextBlock, 20);
                Canvas.SetLeft(TextBlock, Length - RectToAddData.Width);
                Canvas.SetZIndex(TextBlock, 10);
                MainWindow.MainCanvas.Children.Add(TextBlock);

                BtnExit.Height = 20;
                BtnExit.Width = 150;
                Canvas.SetTop(BtnExit, 150);
                Canvas.SetLeft(BtnExit, Length - RectToAddData.Width);
                BtnExit.Content = "Отмена";
                BtnExit.HorizontalContentAlignment = HorizontalAlignment.Center;
                BtnExit.VerticalContentAlignment = VerticalAlignment.Center;
                Canvas.SetZIndex(BtnExit, 10);
                MainWindow.MainCanvas.Children.Add(BtnExit);
                BtnExit.Click += btnExit_Click;


                BtnReadyToAddData.Height = 20;
                BtnReadyToAddData.Width = 150;
                Canvas.SetTop(BtnReadyToAddData, 120);
                Canvas.SetLeft(BtnReadyToAddData, Length - RectToAddData.Width);
                BtnReadyToAddData.Content = "Готово";
                BtnReadyToAddData.HorizontalContentAlignment = HorizontalAlignment.Center;
                BtnReadyToAddData.VerticalContentAlignment = VerticalAlignment.Center;
                Canvas.SetZIndex(BtnReadyToAddData, 10);
                MainWindow.MainCanvas.Children.Add(BtnReadyToAddData);
                BtnReadyToAddData.Click += btnReadyToAddData_Click;
            }
            MainWindow.MainCanvas.MouseDown += MainWindow.ToolAddVertice.RectangleMouseMove;
            
        }


        public static void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.IsProgramReady = true;
            MainWindow.MainCanvas.Children.Remove(TextBlock);
            MainWindow.MainCanvas.Children.Remove(BtnReadyToAddData);
            MainWindow.MainCanvas.Children.Remove(BtnExit);
            MainWindow.MainCanvas.Children.Remove(RectToAddData);
            MainWindow.MainCanvas.Children.Remove(TextBox);
            if (MainWindow.MainCanvas.Children.Contains(TextBlockTellUser)) MainWindow.MainCanvas.Children.Remove(TextBlockTellUser);
            IsElemsAdd=false;

        }


        static bool FlagMessage = false;

        public static void btnReadyToAddData_Click(object sender, RoutedEventArgs e)
        {
            
            string inputStr = TextBox.Text;
            int number;
            if (Int32.TryParse(inputStr, out number))
            {
                if (number <= 0)
                {
                    if (!FlagMessage)
                    {
                        ShowUserErrorMesage();
                        FlagMessage = true;
                    }
                }
                else
                {
                    Connection connectionToChange = Connection.SearchConnection(TextBlockToChange, MainGraph.Connections);
                    connectionToChange.Length = number;
                    connectionToChange.BlockText.Text = inputStr;
                    btnExit_Click(sender, e);
                }
            }
            else
            {
                if (!FlagMessage)
                {
                    ShowUserErrorMesage();
                    FlagMessage = true;
                }
            }
        }


        public static void ShowUserErrorMesage()
        {
            TextBlockTellUser = new TextBlock();
            TextBlockTellUser.Height = 40;
            TextBlockTellUser.Width = 150;
            Canvas.SetTop(TextBlockTellUser, 180);
            Canvas.SetLeft(TextBlockTellUser, Length - RectToAddData.Width);
            Canvas.SetZIndex(TextBlockTellUser, 10);
            TextBlockTellUser.Text = "Введённые данные \nнекорректны";
            MainWindow.MainCanvas.Children.Add(TextBlockTellUser);
        }

        public static void TextBlockSelected(object sender, MouseEventArgs e)
        {
            bool isSelect = false;
            foreach (Connection connection in MainGraph.Connections)
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
