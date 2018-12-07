using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public abstract class Entity
    {
        public int id;

        public Entity(Loader loader)
        {
            this.id = loader.GiveID();
        }
    }

    public abstract class PhysicalEntity: Entity
    {
        protected int[] position;

        public PhysicalEntity(Loader loader, int[] position): base(loader)
        {
            this.position = position;
        }
    }
}
