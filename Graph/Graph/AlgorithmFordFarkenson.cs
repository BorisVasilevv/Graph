using System;
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
    public class AlgorithmFordFarkenson
    {

        readonly int SummAllConnectionLength;

        public AlgorithmFordFarkenson()
        {
            foreach(Connection c in AddConnectionTool.Connections)
            {
                SummAllConnectionLength+=c.Length; 
            }
        }

        public static void Algorithm(Vertice start, Vertice end)
        {

        }


    }
}
