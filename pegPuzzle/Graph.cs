using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pegPuzzle
{
    public class EdgeInfo
    {
        public List<List<Peg>> goesTo;
    }

    public class Graph
    {
        public Dictionary<List<List<Peg>>, GraphNode> nodes;

    }

    public class GraphNode
    {
        public List<EdgeInfo> adjacent;
    }
}
