using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{

    public class Vertice
    {
        static int Counter=0;
        public readonly int Id;

        public Vertice()
        {
            Id=Counter++;
        }

        public static bool operator == (Vertice vertice1, Vertice vertice2)
        {
            return vertice1.Id== vertice2.Id;
        }
        public static bool operator != (Vertice vertice1, Vertice vertice2)
        {
            return vertice1.Id == vertice2.Id;
        }
    }
}
