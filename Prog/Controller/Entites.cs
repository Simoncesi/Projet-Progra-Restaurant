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

        public abstract List<string[]> ReturnInformations();
    }

    public abstract class PhysicalEntity : Entity
    {
        protected int[] position;

        public PhysicalEntity(Loader loader, int[] position) : base(loader)
        {
            this.position = position;
        }

        public int[] GetPosition()
        {
            return position;
        }
    }

    public abstract class Conteneur : PhysicalEntity
    {
        protected List<Object> contenu;

        public Conteneur(Loader loader, int[] position) : base(loader, position)
        {
            contenu = new List<Object>();
        }

        public void AddContent(Object objet)
        {
            contenu.Add(objet);
        }

        public void DelContent(Object objet)
        {
            contenu.Remove(objet);
        }

        public List<Object> GetContent()
        {
            return contenu;
        }

        public Object FindContent(Predicate<Object> condition)
        {
            return contenu.Find(condition);
        }

        public Object TakeContent(Object objet)
        {
            if(contenu.FindIndex(e => e == objet) > -1)
            {
                DelContent(objet);
                return objet;
            }
            else
            {
                return null;
            }
        }
    }

    public class Table : Conteneur
    {
        private int numTable;
        private int places;
        private List<Client> clientsPresents;
        private bool libre;
        public bool commisReservee;
        public bool serveurReservee;


        public Table(Loader loader, int[] position, int numTable, int places) : base(loader, position)
        {
            this.numTable = numTable;
            libre = true;
            clientsPresents = new List<Client>();
            this.places = places;
        }

        public override List<string[]> ReturnInformations()
        {
            List<string[]> infosToReturn = new List<string[]>();

            infosToReturn.Add(new string[] { "ID", id.ToString() });
            infosToReturn.Add(new string[] { "Numéro table", numTable.ToString() });
            infosToReturn.Add(new string[] { "Places", places.ToString() });
            infosToReturn.Add(new string[] { "Places libres", GetPlacesLibres().ToString() });
            infosToReturn.Add(new string[] { "Places occupées", clientsPresents.Count().ToString() });

            return infosToReturn;
        }

        public int GetNombrePlaces()
        {
            return places;
        }

        public int GetPlacesLibres()
        {
            return places - clientsPresents.Count();
        }

        public bool EstLibre()
        {
            return libre;
        }

        public void AjouterClient(Client client)
        {
            Console.WriteLine("Un client ajouté à la table " + numTable);
            clientsPresents.Add(client);
        }

        public void RetirerClient(Client client)
        {
            clientsPresents.Remove(client);
        }

        public bool Reserver()
        {
            if (libre == false)
            {
                return false;
            }
            else
            {
                Console.WriteLine("Table numéro " + numTable + " réservée !");
                libre = false;
                return true;
            }
        }

        public bool Liberer()
        {
            if (libre == true)
            {
                return false;
            }
            else
            {
                libre = true;
                return true;
            }
        }

        public int GetNumTable()
        {
            return numTable;
        }

        public List<Client> GetClients()
        {
            return clientsPresents;
        }
    }

    public class Stock: Conteneur
    {
        public Stock(Loader loader, int[] position) : base(loader, position)
        {
            
        }

        public override List<string[]> ReturnInformations()
        {
            List<string[]> infosToReturn = new List<string[]>();

            infosToReturn.Add(new string[] { "ID", id.ToString() });
            infosToReturn.Add(new string[] { "Items", contenu.Count().ToString() });

            return infosToReturn;
        }
    }
}
