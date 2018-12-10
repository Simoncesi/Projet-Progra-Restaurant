using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public abstract class Personnage : PhysicalEntity
    {
        public string nom;
        public string prenom;
        public int age;
        protected List<Objet> inventaire;
        protected Loader loader;

        public abstract void UpdatePersonnage(int delta);

        public Personnage(string nom, string prenom, int age, int[] position, Loader loader):base(loader, position)
        {
            this.loader = loader;
            loader.GetCore().AddPersonnage(this);

            this.nom = nom;
            this.prenom = prenom;
            this.age = age;

            this.inventaire = new List<Objet>();
        }

        public bool MoveTo(int[] position)
        {
            if(loader.GetWorld().GetTableEntity(position) != null)
            {
                this.position = position;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class Client: Personnage
    {
        public int attente;
        public Table table;
        public bool tableDonnee;
        public string etat;
        private MaitreHotel maitre;

        public Client(int argent, int attente, string nom, string prenom, int age, int[] position, Loader loader) :base(nom, prenom, age, position, loader)
        {
            this.inventaire.Add(new Divers("argent", this, loader));
            this.attente = attente;
            etat = "idle";
        }

        public override void UpdatePersonnage(int delta)
        {
            switch(etat)
            {
                case "idle":
                    if(tableDonnee)
                    {
                        bool valid = false;
                        if(MoveTo(table.GetPosition()))
                        {
                            if(Sasseoir())
                            {
                                valid = true;
                            }
                        }

                        if(valid == false)
                        {
                            MoveTo(maitre.GetPosition());
                        }
                    }
                    break;

                case "assis":
                    break;

                case "choisisMenu":
                    break;

                case "attendsRepas":
                    break;

                case "mange":
                    break;

                case "part":
                    break;
            }
        }

        public void RecevoirTable(Table table)
        {
            this.table = table;
            tableDonnee = true;
        }

        public void RecevoirMaitre(MaitreHotel maitre)
        {
            this.maitre = maitre;
        }

        public bool Sasseoir()
        {
            TableEntity tablentity = loader.GetWorld().GetTableEntity(position);
            
            if(tablentity.FindEntityByType("Controller.Table").Count() > 0)
            {
                Table tableCase = (Table)tablentity.FindEntityByType("Controller.Table")[0];

                if (tableCase == table && tableCase.GetPlacesLibres() > 0)
                {
                    table.AjouterClient(this);
                    etat = "assis";
                    return true;
                }
                
            }
            return false;
        }
    }

    public abstract class PersonnelSalle: Personnage
    {
        public PersonnelSalle(string nom, string prenom, int age, int[] position, Loader loader) : base(nom, prenom, age, position, loader)
        {

        }
    }

    public class MaitreHotel: PersonnelSalle
    {
        private int randomCountDown;
        private Random rnd;
        private List<List<Client>> clientsAttente;
        private Hall hall;
        private Restaurant restaurant;

        public MaitreHotel(string nom, string prenom, int age, int[] position, Loader loader) : base(nom, prenom, age, position, loader)
        {
            rnd = new Random();
            randomCountDown = 0;
            this.hall = loader.GetWorld().getHall();
            this.restaurant = loader.GetWorld().getRestaurant();
            this.clientsAttente = new List<List<Client>>();
        }

        public override void UpdatePersonnage(int delta)
        {
            Console.WriteLine(randomCountDown);

            if (randomCountDown <= 0)
            {
                clientsAttente.Add(hall.GenerateClients(loader));
                randomCountDown = rnd.Next(10, 20);
            }
            else
            {
                randomCountDown -= 1;
            }

            if(clientsAttente.Count > 0)
            {
                if(RecieveClients(clientsAttente[0]))
                {
                    Console.WriteLine("Clients reçus !");
                    clientsAttente.RemoveAt(0);
                }
            }
        }

        private bool RecieveClients(List<Client> clients)
        {
            int quantity = clients.Count();
            List<Table> tables = restaurant.GetEmptyTables();
            if (tables.Count() > 0)
            {
                Table designatedTable = tables[0];
                if(designatedTable.Reserver())
                {
                    foreach(Client client in clients)
                    {
                        client.RecevoirMaitre(this);
                        client.RecevoirTable(designatedTable);
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
