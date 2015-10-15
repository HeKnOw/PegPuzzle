using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pegPuzzle
{
    public class Peg
    {
        //on or off
        private bool status;

        public Peg(bool status)
        {
            this.status = status;
        }

        public bool Status
        {
            get
            {
                return status;
            }
            set
            {
                this.status = value;
            }
        }

        //rowLocation
        //columnLocation
    }
}
