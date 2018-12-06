using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class World
    {
        protected int width;
        protected int height;
        protected TableEntity[,] table;

        public World(int width, int height)
        {
            this.width = width;
            this.height = height;

            this.table = new TableEntity[width,height];
        }

        public Cuisine InstantiateCuisine(int width, int height, int[] position)
        {
            SetSalle(width, height, position, "Cuisine");
            return new Cuisine(width, height, position, this);
        }

        public Restaurant InstantiateRestaurant(int width, int height, int[] position)
        {
            SetSalle(width, height, position, "Restaurant");
            return new Restaurant(width, height, position, this);
        }

        public Comptoir InstantiateComptoir(int width, int height, int[] position)
        {
            SetSalle(width, height, position, "Comptoir");
            return new Comptoir(width, height, position, this);
        }

        public Hall InstantiateHall(int width, int height, int[] position)
        {
            SetSalle(width, height, position, "Hall");
            return new Hall(width, height, position, this);
        }

        private void SetSalle(int width, int height, int[] position, string typeSalle)
        {
            for(int x = position[0]; x < width; x++)
            {
                for (int y = position[1]; y < height; y++)
                {
                    table[x, y].typeSalle = typeSalle;
                }
            }
        }
    }


    public abstract class Salle
    {
        public int width;
        public int height;
        public int[] position;
        public World world;

        public Salle()
        {
            this.width = 10;
            this.height = 10;
            this.position = new int[2] { 0, 0 };
        }

        public Salle(int width, int height, int[] position, World world)
        {
            this.width = width;
            this.height = height;

            this.position = position;
            this.world = world;

            Console.WriteLine(position);
        }
    }

    public class Cuisine : Salle
    {

        public Cuisine():base() { }

        public Cuisine(int width, int height, int[] position, World world):base(width, height, position, world)
        {

        }

        public void test()
        {
            Console.WriteLine("erererer");
        }
    }

    public class Restaurant : Salle
    {

        public Restaurant() { }

        public Restaurant(int width, int height, int[] position, World world) : base(width, height, position, world)
        {

        }
    }

    public class Comptoir : Salle
    {

        public Comptoir() { }

        public Comptoir(int width, int height, int[] position, World world) : base(width, height, position, world)
        {

        }
    }

    public class Hall : Salle
    {

        public Hall() { }

        public Hall(int width, int height, int[] position, World world) : base(width, height, position, world)
        {

        }
    }

    public class TableEntity
    {
        public string typeSalle;
        private List<Entity> entities;

        public TableEntity()
        {
            entities = new List<Entity>();
        }

        public List<Entity> GetAllEntities()
        {
            return entities;
        }

        public void AddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        public bool RemoveEntity(Entity entity)
        {
            return entities.Remove(entity);
        }

        public List<Entity> FindEntityByType(string type)
        {
            return entities.FindAll(e => e.GetType().ToString() == type);
        }

        public bool HasEntity(int entityID)
        {
            int result = entities.FindIndex(e => e.id == entityID);

            if(result > -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
