using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class Entity
    {
        public int id;
        public int tt;

        public Entity(Loader loader)
        {
            id = loader.GiveID();
        }
    }


}
