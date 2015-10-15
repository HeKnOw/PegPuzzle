using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pegPuzzle
{
    public partial class nodeComparer : IEqualityComparer<List<List<Peg>>>
    {
        public bool Equals(List<List<Peg>> x, List<List<Peg>> y)
        {
            if (x.Count != y.Count)
                return false;

            for (int i = 0; i < x.Count; i++ )
            {
                if(x[i].Count != y[i].Count)
                {
                    return false;
                }
                for(int j = 0; j < x[i].Count; j++)
                {
                    if(x[i][j].Status != y[i][j].Status)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public int GetHashCode(List<List<Peg>> obj)
        {
            int total = 1823;

            for (int j = 0; j < obj.Count; j++ )
            {
                for(int i = 0; i < obj[j].Count; i ++ )
                {
                    if(obj[j][i].Status)
                    {
                        total = total ^ (j ^ i).GetHashCode();
                    }
                    else
                    {
                        total = total ^ ((j ^ i)^(j ^ i)).GetHashCode();
                    }
                }
            }

            return total.GetHashCode();
            
        }
    }
}
