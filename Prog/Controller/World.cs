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

        private Core core;
        private Loader loader;

        private Cuisine cuisine;
        private Restaurant restaurant;
        private Hall hall;
        private Comptoir comptoir;

        public World(int width, int height)
        {
            this.width = width;
            this.height = height;

            this.core = new Core();
            this.loader = new Loader(core, this);

            this.table = new TableEntity[width,height];

            FillTable(table);
        }

        public Loader GetLoader()
        {
            return loader;
        }

        public TableEntity GetTableEntity(int[] position)
        {
            if(position[0] < width && position[1] < height)
            {
                return table[position[0], position[1]];
            }
            else
            {
                return null;
            }
            
        }

        public Cuisine InstantiateCuisine(int width, int height, int[] position)
        {
            SetSalle(width, height, position, "Cuisine");
            cuisine = new Cuisine(width, height, position, this);
            return cuisine;
        }

        public Restaurant InstantiateRestaurant(int width, int height, int[] position)
        {
            SetSalle(width, height, position, "Restaurant");
            restaurant = new Restaurant(width, height, position, this);
            return restaurant;
        }

        public Comptoir InstantiateComptoir(int width, int height, int[] position, int[] pointAccesRestaurant, int[] pointAccesCuisine)
        {
            SetSalle(width, height, position, "Comptoir");
            comptoir = new Comptoir(width, height, position, this, pointAccesRestaurant, pointAccesCuisine);
            return comptoir;
        }

        public Hall InstantiateHall(int width, int height, int[] position)
        {
            SetSalle(width, height, position, "Hall");
            hall = new Hall(width, height, position, this);
            return hall;
        }

        public Cuisine getCuisine()
        {
            return cuisine;
        }

        public Restaurant getRestaurant()
        {
            return restaurant;
        }

        public Hall getHall()
        {
            return hall;
        }

        public Comptoir GetComptoir()
        {
            return comptoir;
        }

        private void SetSalle(int width, int height, int[] position, string typeSalle)
        {
            for(int x = position[0]; x < (width + position[0]); x++)
            {
                for (int y = position[1]; y < (height + position[1]); y++)
                {
                    table[x, y].typeSalle = typeSalle;
                }
            }
        }

        private void FillTable(TableEntity[,] table)
        {
            for (int x = 0; x < table.GetLength(0); x++)
            {
                for (int y = 0; y < table.GetLength(1); y++)
                {
                    table[x, y] = new TableEntity();
                }
            }
        }

        public string[,] GenerateGridView()
        {
            string[,] stringTable = new string[this.table.GetLength(0), this.table.GetLength(1)];

            for (int x = 0; x < this.table.GetLength(0); x++)
            {
                for (int y = 0; y < this.table.GetLength(1); y++)
                {
                    stringTable[x, y] = GetTableEntity(new int[] { x, y }).typeSalle;
                }
            }

            return stringTable;
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
        private int tableIdCount;
        private List<Table> tables;

        public Restaurant() { }

        public Restaurant(int width, int height, int[] position, World world) : base(width, height, position, world)
        {
            tableIdCount = 0;
            tables = new List<Table>();
        }

        public void GenerateTables(int[,] positions)
        {
            for(int i = 0; i < position.GetLength(0); i++)
            {
                if(positions[i,0] < width && positions[i,1] < height)
                {
                    Table table = new Table(world.GetLoader(), new int[] { position[0] + positions[i, 0], position[1] + positions[i, 1] }, tableIdCount, 10);
                    world.GetTableEntity(new int[] { positions[i, 0], positions[i, 1] }).AddEntity(table);
                    tables.Add(table);
                    tableIdCount++;

                    Console.WriteLine("Table générée à: "+ position[0] + positions[i, 0]+" , "+ position[1] + positions[i, 1]);
                }
            }
        }

        public List<Table> GetAllTables()
        {
            return tables;
        }

        public List<Table>GetEmptyTables()
        {
            List<Table> emptyTables = new List<Table>();

            foreach(Table table in tables)
            {
                if(table.EstLibre())
                {
                    emptyTables.Add(table);
                }
            }
            return emptyTables;
        }

        public List<Table>GetFilledTables()
        {
            List<Table> filledTables = new List<Table>();

            foreach (Table table in tables)
            {
                if (table.GetPlacesLibres() < table.GetNombrePlaces())
                {
                    filledTables.Add(table);
                }
            }
            return filledTables;
        }

        public List<Table>GetReservedTables()
        {
            List<Table> reservedTables = new List<Table>();

            foreach (Table table in tables)
            {
                if (table.EstLibre() == false)
                {
                    reservedTables.Add(table);
                }
            }
            return reservedTables;
        }
    }

    public class Comptoir : Salle
    {
        private int[] pointAccesRestaurant;
        private int[] pointAccesCuisine;
        private Carte carte;
        public Stock stockCartes;
        public Stock stockPlats;

        public Comptoir() { }

        public Comptoir(int width, int height, int[] position, World world, int[] pointAccesRestaurant, int[] pointAccesCuisine) : base(width, height, position, world)
        {
            stockCartes = new Stock(world.GetLoader(), position);
            stockPlats = new Stock(world.GetLoader(), position);

            if(pointAccesRestaurant[0] < width && pointAccesRestaurant[1] < height)
            {
                this.pointAccesRestaurant = pointAccesRestaurant;
            }
            else
            {
                this.pointAccesRestaurant = new int[] { 0, 0 };
            }

            if (pointAccesCuisine[0] < width && pointAccesCuisine[1] < height)
            {
                this.pointAccesCuisine = pointAccesCuisine;
            }
            else
            {
                this.pointAccesCuisine = new int[] { 0, 0 };
            }
        }

        public int[] GetPointAccesRestaurant()
        {
            return pointAccesRestaurant;
        }

        public int[] GetPointAccesCuisine()
        {
            return pointAccesCuisine;
        }

        public void LoadCarte(Carte carte)
        {
            this.carte = carte;
        }

        public Carte GetCarte()
        {
            return carte;
        }

        public Plat GetPlat(string plat)
        {
            List<Object> platsEnStock = stockPlats.GetContent();

            foreach (Object objet in platsEnStock)
            {
                if (objet.GetType().ToString() == "Controller.Plat")
                {
                    if (((Plat)objet).nomPlat == plat)
                    {
                        return (Plat)objet;
                    }
                }
            }
            return null;
        }

        public bool HasPlat(string plat)
        {
            List<Object> platsEnStock = stockPlats.GetContent();

            foreach(Object objet in platsEnStock)
            {
                if(objet.GetType().ToString() == "Controller.Plat")
                {
                    if(((Plat)objet).nomPlat == plat)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    public class Hall : Salle
    {
        public Hall() { }

        public Hall(int width, int height, int[] position, World world) : base(width, height, position, world)
        {

        }

        public List<Client> GenerateClients(Loader loader)
        {
            Console.WriteLine("Génération de clients !");
            Random rnd = new Random();
            List<Client> clients = new List<Client>();

            for(int i = 0; i < rnd.Next(1,4); i++)
            {
                clients.Add(new Client(rnd.Next(30, 100), rnd.Next(0, 10), "Jean", "Albert", rnd.Next(20, 80), this.position, loader));
            }

            return clients;
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
