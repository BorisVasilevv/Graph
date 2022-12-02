using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{

    public class Vertice
    {
        static int Counter=0;
        public readonly int Id;
        public string Name;
        public List<Vertice> Connection;

        Vertice()
        {
            Id=Counter++;
        }
    }
}
