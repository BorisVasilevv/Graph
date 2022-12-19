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

        static Canvas _canvas;

        public void AddText(string text)
        {                  
            sb.Append(text);
        }

        public void AddLine(string text)
        {
            sb.Append(text);
            sb.Append("\n");
            
        }


        public static void ShowAllLogToUser(Canvas canvas)
        {
            MainWindow.IsUserCanUseButtons = false;
            LoggerTextBlock.MouseWheel += LoggerTextBlock_MouseWheel;
            BtnReturn.Click += BtnReturn_Click;
            _canvas = canvas;
            LoggerRect.Width = canvas.Width;
            LoggerRect.Height = canvas.Height;

            int amountOfStrings = 0;
            foreach(char c in sb.ToString())
                if (c == '\n') amountOfStrings++;
            


            LoggerTextBlock.Height = 16.1* amountOfStrings;
            LoggerTextBlock.Width = canvas.Width;
            LoggerTextBlock.Margin = new Thickness(85, 5, 0, 0);
            LoggerTextBlock.Text=sb.ToString();

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
            MainWindow.IsUserCanUseButtons = true;
            _canvas.Children.Remove(LoggerRect);
            _canvas.Children.Remove(LoggerTextBlock);
            _canvas.Children.Remove(BtnReturn);
            _diff = 0;
            LoggerTextBlock.Margin = new Thickness(85, 5, 0, 0);
        }


        private static int _diff;
        const int ChangeLength = 40; 
        private static void LoggerTextBlock_MouseWheel(object sender, MouseWheelEventArgs e)
        {


            if (LoggerTextBlock.Height > _canvasHeight)
            {
                Thickness thickness = LoggerTextBlock.Margin;
                int delta = e.Delta;
                

                if (delta > 0 &&  _diff< 0)
                {
                    //delta = 120;
                    // The user scrolled up.
                    thickness.Top += ChangeLength;
                    _diff += ChangeLength;

                }
                else if (delta < 0 && LoggerTextBlock.Height - _canvasHeight - Math.Abs(_diff) > 0)
                {
                    //delta = -120;
                    // The user scrolled down.
                    thickness.Top -= ChangeLength;
                    _diff -= ChangeLength;
                }
                
                LoggerTextBlock.Margin = thickness;

            }

        }
    }
}
