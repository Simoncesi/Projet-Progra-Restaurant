using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class Loader
    {
        private int IDCount;

        public Loader()
        {
            IDCount = 0;
        }

        public int GiveID()
        {
            return IDCount++;
        }
    }
}
