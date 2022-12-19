using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using static Graph.DijkstraAlgorithm;


namespace Graph
{
    public class DeleteVerticeTool
    {
        static MyGraph MainGraph= MainWindow.MainGraph;

        public static void DeleteVertice(object sender, MouseEventArgs e)
        {
            Rectangle selectedRectangle = AddVerticeTool.SelectedRectangle;
            if (selectedRectangle != null)
            { 
                Vertice vert = Vertice.SearchVertice(selectedRectangle, MainGraph.AllVertices);
                List<Connection> connectToDel= MainGraph.Connections.Where(x => x.Vertice1!=null && x.Vertice2!=null && x.Vertice1.Id == vert.Id || x.Vertice2.Id == vert.Id).ToList();
                MainGraph.Connections = MainGraph.Connections.Where(x => x.Vertice1 != null && x.Vertice2 != null && x.Vertice1.Id!=vert.Id&&x.Vertice2.Id!=vert.Id).ToList();
                MainGraph.AllVertices.Remove(vert);

                foreach (Connection conn in connectToDel)
                {
                    MainWindow.MainCanvas.Children.Remove(conn.Line);
                    MainWindow.MainCanvas.Children.Remove(conn.BlockText);
                }
                MainWindow.MainCanvas.Children.Remove(selectedRectangle);
                MainWindow.MainCanvas.Children.Remove(vert.NameTextBlock);

            }
            MainWindow.MainCanvas.MouseDown -= DeleteVertice;


            
        }
    }
}
