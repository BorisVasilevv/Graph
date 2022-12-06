using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows;


namespace Graph
{

    public class Vertice
    {
        static int _counter=0;
        public readonly int Id;

        public Vertice()
        {
            Id=_counter++;
        }

        public TextBlock VerticeNameTextBlock { get; set; }

        public Rectangle Rect { get; set; }

        public Point RectCenter { get; set; }

        public List<int> ConnectionIds = new List<int>();






        public static Vertice SearchVertice(int id)
        {
            foreach (Vertice vertice in AddVerticeTool.AllVertices)
            {
                if (vertice.Id == id)
                    return vertice;
            }
            return null;
        }
        public static Vertice SearchVertice(Rectangle rect)
        {
            foreach (Vertice vertice in AddVerticeTool.AllVertices)
            {
                if (vertice.Rect == rect)
                    return vertice;
            }
            return null;
        }

        

    }
}
