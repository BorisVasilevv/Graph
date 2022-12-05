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


namespace Graph
{
    public class DeleteVerticeTool
    {

        public static void DeleteVertice(object sender, MouseEventArgs e)
        {
            Rectangle selectedRectangle = AddVerticeTool.SelectedRectangle;
            if (selectedRectangle != null)
            { 
                Vertice vert = Vertice.SearchVertice(selectedRectangle);
                List<Connection> connectToDel= AddConnectionTool.Connections.Where(x => x.Vertice1!=null && x.Vertice2!=null && x.Vertice1.Id == vert.Id || x.Vertice2.Id == vert.Id).ToList();
                AddConnectionTool.Connections = AddConnectionTool.Connections.Where(x => x.Vertice1 != null && x.Vertice2 != null && x.Vertice1.Id!=vert.Id&&x.Vertice2.Id!=vert.Id).ToList();
                AddVerticeTool.AllVertices.Remove(vert);

                foreach (Connection conn in connectToDel)
                {
                    MainWindow.MainCanvas.Children.Remove(conn.Polyline);
                }
                MainWindow.MainCanvas.Children.Remove(selectedRectangle);
                MainWindow.MainCanvas.Children.Remove(vert.VerticeNameTextBlock);

            }
            MainWindow.MainCanvas.MouseDown -= DeleteVertice;
        }
    }
}
