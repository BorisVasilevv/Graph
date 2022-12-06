﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;


namespace Graph
{
    public class ChoseStartEndHelper
    {

        public static Vertice Start, End;

        static TextBlock TextBlock = new TextBlock
        {
            Height = 20,
            Width = 200,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(200, 20, 0, 0)
        };

        public static Action<Vertice, Vertice> Function;

        public static void ChooseVertice(Action<Vertice, Vertice> func)
        {
            TextBlock.Text = "Выберите стартовую вершину";
            MainWindow.MainCanvas.Children.Add(TextBlock);
            Function = func;
            MainWindow.MainCanvas.MouseDown += ChooseVertice_MouseDown;

        }

        private static void ChooseVertice_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rect = AddVerticeTool.SelectedRectangle;
            if (rect != null)
            {
                if (Start == null)
                {
                    Start = Vertice.SearchVertice(rect);
                    TextBlock.Text = "Выберите конечную вершину";
                }
                else if (End == null && Start.Rect != rect)
                {
                    End = Vertice.SearchVertice(rect);
                    MainWindow.MainCanvas.MouseDown -= ChooseVertice_MouseDown;
                    MainWindow.IsProgramReady = true;
                    MainWindow.MainCanvas.Children.Remove(TextBlock);
                    Function.Invoke(Start, End);
                }
            }
        }
    }
}