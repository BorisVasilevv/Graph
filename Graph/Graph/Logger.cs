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
    class Logger
    {
        static Rectangle LoggerRect = new Rectangle() { Fill = new SolidColorBrush(Colors.White) };
        static TextBlock LoggerTextBlock = new TextBlock();

        static StringBuilder sb = new StringBuilder();
        static Button BtnReturn = new Button { Width = 70, Height = 30, Content = "RETURN", Margin = new Thickness(710, 45, 0, 0) };
        static private double _canvasHeight;
        private static int _count = 0;
        static Canvas _canvas;
        public void AddText(string text)
        {
            _count++;
            sb.Append(text);
            //sb.Append("\n");
        }


        public static void ShowAllLogToUser(Canvas canvas)
        {
            MainWindow.IsUserCanUSeButton = false;
            LoggerTextBlock.MouseWheel += LoggerTextBlock_MouseWheel;
            BtnReturn.Click += BtnReturn_Click;
            _canvas = canvas;
            LoggerRect.Width = canvas.Width;
            LoggerRect.Height = canvas.Height;

            LoggerTextBlock.Height = 20*_count;
            LoggerTextBlock.Width = canvas.Width;
            LoggerTextBlock.Margin = new Thickness(85, 5, 0, 0);

            canvas.Children.Add(LoggerRect);
            canvas.Children.Add(LoggerTextBlock);
            canvas.Children.Add(BtnReturn);

            Canvas.SetZIndex(LoggerRect, 25);
            Canvas.SetZIndex(LoggerTextBlock, 26);
            Canvas.SetZIndex(BtnReturn, 26);

            _canvasHeight = canvas.Height;
        }

        private static void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.IsUserCanUSeButton = true;
            _canvas.Children.Remove(LoggerRect);
            _canvas.Children.Remove(LoggerTextBlock);
            _canvas.Children.Remove(BtnReturn);
        }


        private static int _diff;
        private static void LoggerTextBlock_MouseWheel(object sender, MouseWheelEventArgs e)
        {


            if (LoggerTextBlock.Height > _canvasHeight)
            {
                Thickness thickness = LoggerTextBlock.Margin;
                int delta = e.Delta;
                

                if (delta > 0 &&  _diff< 0)
                {
                    // The user scrolled up.
                    thickness.Top += delta;
                    _diff += delta;

                }
                else if (delta < 0 && LoggerTextBlock.Height - _canvasHeight - Math.Abs(_diff) > 0)
                {
                    // The user scrolled down.
                    thickness.Top += delta;
                    _diff += delta;
                }
                
                LoggerTextBlock.Margin = thickness;

            }

        }
    }
}
