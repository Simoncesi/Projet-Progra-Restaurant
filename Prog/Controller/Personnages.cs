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
                loader.GetWorld().GetTableEntity(this.position).RemoveEntity(this);
                this.position = position;
                loader.GetWorld().GetTableEntity(this.position).AddEntity(this);
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
        private Carte carte;
        private List<String> menuChoisi;
        private string repasEtape;
        private int attenteCountDown;

        public Client(int argent, int attente, string nom, string prenom, int age, int[] position, Loader loader) :base(nom, prenom, age, position, loader)
        {
            this.inventaire.Add(new Divers("argent", this, loader));
            this.attente = attente;
            etat = "idle";
            repasEtape = "null";
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

                    if(carte != null)
                    {
                        etat = "choisisMenu";
                    }
                    break;

                case "choisisMenu":

                    if(menuChoisi == null)
                    {
                        Random rnd = new Random();
                        menuChoisi = carte.GetMenus()[rnd.Next(0, carte.GetMenus().Count())];
                        attenteCountDown = rnd.Next(1, 3) * attente;
                        Console.WriteLine("Le client " + id + " a choisis le menus " + menuChoisi[0] + " " + menuChoisi[1] + " " + menuChoisi[2]);
                    }
                    else
                    {
                        if(attenteCountDown > 0)
                        {
                            attenteCountDown -= 1;
                        }
                        else
                        {
                            etat = "menuChoisi";
                        }
                    }
                
                    break;

                case "menuChoisi":
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

                    Console.WriteLine("Client " + id + " s'asseois à table " + table.GetNumTable());

                    return true;
                }
                
            }
            return false;
        }

        public void RecieveCarte(Carte carte)
        {
            this.carte = carte;
        }

        public void GiveBackCarte()
        {
            carte = null;
        }

        public string GetRepasEtape()
        {
            return repasEtape;
        }

        public string GetEtat()
        {
            return etat;
        }

        public List<String> DonnerMenu()
        {
            etat = "attendsRepas";
            repasEtape = "Entree";
            return menuChoisi;
        }

        public string GetPlatAttente()
        {
            switch (repasEtape)
            {
                case "Entree":
                    return menuChoisi[0];

                case "Plat":
                    return menuChoisi[1];

                case "Dessert":
                    return menuChoisi[2];
            }
            return null;
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
        private List<ChefDeRang> chefsDeRang;

        public MaitreHotel(string nom, string prenom, int age, int[] position, Loader loader) : base(nom, prenom, age, position, loader)
        {
            rnd = new Random();
            randomCountDown = 0;
            this.hall = loader.GetWorld().getHall();
            this.restaurant = loader.GetWorld().getRestaurant();
            this.clientsAttente = new List<List<Client>>();
            this.chefsDeRang = new List<ChefDeRang>();
        }

        public override void UpdatePersonnage(int delta)
        {
            //Console.WriteLine(randomCountDown);

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
                    clientsAttente.RemoveAt(0);
                }
            }
        }

        public void AddChefDeRang(ChefDeRang chefDeRang)
        {
            chefsDeRang.Add(chefDeRang);
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
                        //client.RecevoirTable(designatedTable);
                    }
                    AlertChefDeRang(clients, designatedTable);
                    return true;
                }
            }
            return false;
        }

        private bool AlertChefDeRang(List<Client> clients, Table table)
        {
            foreach(ChefDeRang chef in chefsDeRang)
            {
                if(chef.GetEtat() == "idle")
                {
                    chef.AddClientsToRecieve(clients, table);
                    Console.WriteLine("MH "+id+" alerte CR "+chef.id+" pour reçevoir clients pour table "+ table.GetNumTable());
                    return true;
                }
            }
            chefsDeRang[0].AddClientsToRecieve(clients, table);
            Console.WriteLine("MH " + id + " alerte CR " + chefsDeRang[0].id + " pour reçevoir clients pour table " + table.GetNumTable());
            return false;
        }
    }

    public class Serveur : PersonnelSalle
    {
        private string etat;
        private Comptoir comptoir;
        private Table targetTable;

        public Serveur(string nom, string prenom, int age, int[] position, Loader loader) : base(nom, prenom, age, position, loader)
        {
            etat = "idle";
            comptoir = loader.GetWorld().GetComptoir();
        }

        public override void UpdatePersonnage(int delta)
        {
            switch (etat)
            {
                case "idle":

                    List<Table> tablesCorrespondent = CheckTablesClientsEtat("attendsRepas");

                    if (tablesCorrespondent.Count() > 0)
                    {
                        foreach(Table table in tablesCorrespondent)
                        {
                            bool platsPrets = true;
                            foreach (Client client in table.GetClients())
                            {
                                if(comptoir.HasPlat(client.GetPlatAttente()) == false)
                                {
                                    platsPrets = false;
                                }
                            }

                            if(platsPrets)
                            {
                                targetTable = table;
                                etat = "apportePlat";
                                break;
                            }
                        }
                    }

                    break;

                case "apportePain":
                    break;

                case "apportePlat":
                    break;

                case "debarasseRepas":
                    break;
            }
        }

        private List<Table> CheckTablesClientsEtat(string etatToCheck)
        {
            List<Table> tablesRemplies = loader.GetWorld().getRestaurant().GetFilledTables();
            List<Table> tablesCorrespondent = new List<Table>();

            foreach (Table table in tablesRemplies)
            {
                bool clientsCorrespondent = true;

                foreach (Client client in table.GetClients())
                {
                    if (client.GetEtat() != etatToCheck)
                    {
                        clientsCorrespondent = false;
                        break;
                    }
                }
                if (clientsCorrespondent == true)
                {
                    tablesCorrespondent.Add(table);
                }
            }
            return tablesCorrespondent;
        }

    }

    public class ChefDeRang : PersonnelSalle
    {
        private string etat;
        private List<Object[]> clientsToRecieve;
        private List<Object[]> clientsChoosingMenus;
        private List<Object[]> clientsWaitingFood;
        private int attenteClients;
        private int[] tablePosition;
        private Carte carte;
        private MaitreHotel maitre;
        private Table tableCiblee;

        public ChefDeRang(string nom, string prenom, int age, int[] position, Loader loader, MaitreHotel maitreHotel) : base(nom, prenom, age, position, loader)
        {
            etat = "idle";
            clientsToRecieve = new List<Object[]>();
            clientsChoosingMenus = new List<Object[]>();
            clientsWaitingFood = new List<Object[]>();
            tablePosition = new int[2];
            carte = null;
            this.maitre = maitreHotel;
            maitre.AddChefDeRang(this);
        }

        public override void UpdatePersonnage(int delta)
        {
            switch (etat)
            {
                case "idle":

                    if(clientsToRecieve.Count() > 0)
                    {
                        RecieveClients(clientsToRecieve[0]);
                    }
                    else
                    {
                        List<Table> tablesCorrespondent = CheckTablesClientsEtat("menuChoisi");

                        if(tablesCorrespondent.Count() > 0)
                        {
                            etat = "prendCommande";
                            tableCiblee = tablesCorrespondent[0];
                            MoveTo(tableCiblee.GetPosition());
                            break;
                        }

                        tablesCorrespondent = CheckTablesClientsEtat("attendsRepas");

                        if(tablesCorrespondent.Count() > 0)
                        {

                        }
                    }

                    break;

                case "attendsClientsTable":

                    WaitingClients();
                    break;

                case "apporteCartes":

                    GettingMenus();
                    break;

                case "prendCommande":
                    GettingCommande(tableCiblee);
                    break;

                case "apporteCouverts":

                    break;
            }
        }

        private void WaitingClients()
        {
            if (attenteClients > 0)
            {
                if (clientsToRecieve.Count() > 0)
                {
                    Table table = (Table)clientsToRecieve[0][1];
                    bool allClientsPresents = true;

                    foreach (Client client in (List<Client>)clientsToRecieve[0][0])
                    {
                        if (loader.GetWorld().GetTableEntity(GetPosition()).HasEntity(client.id) == false)
                        {
                            allClientsPresents = false;
                        }
                    }

                    if (allClientsPresents == true)
                    {
                        etat = "apporteCartes";
                        Console.WriteLine("CR " + id + " apporte les cartes pour la table " + table.GetNumTable());
                    }
                    else
                    {
                        attenteClients--;
                    }
                }
                else
                {
                    attenteClients = 0;
                }
            }
            else
            {
                Console.WriteLine("CR " + id + " a trop attendus !");
                etat = "idle";
            }
        }

        private void GettingMenus()
        {
            if (carte == null)
            {
                if (GetPosition() == ((Table)clientsToRecieve[0][1]).GetPosition())
                {
                    tablePosition = GetPosition();
                    MoveTo(loader.GetWorld().GetComptoir().GetPointAccesRestaurant());
                }
                else
                {
                    carte = loader.GetWorld().GetComptoir().GetCarte();
                    Console.WriteLine("CR " + id + " a récupéré cartes");
                }
            }
            else
            {
                if (GetPosition() == ((Table)clientsToRecieve[0][1]).GetPosition())
                {
                    foreach (Client client in (List<Client>)clientsToRecieve[0][0])
                    {
                        client.RecieveCarte(carte);
                    }

                    Console.WriteLine("CR " + id + " a donné les cartes aux clients de la table " + ((Table)clientsToRecieve[0][1]).GetNumTable());

                    clientsChoosingMenus.Add(clientsToRecieve[0]);
                    clientsToRecieve.RemoveAt(0);

                    carte = null;

                    etat = "idle";
                }
                else
                {
                    MoveTo(tablePosition);
                }
            }
        }

        public void InformMaitreHotel(MaitreHotel maitreHotel)
        {
            maitreHotel.AddChefDeRang(this);
        }

        public void AddClientsToRecieve(List<Client> clients, Table table)
        {
            clientsToRecieve.Add(new Object[] { clients, table });
        }

        private void RecieveClients(Object[] clientsTable)
        {
            Table table = (Table)clientsTable[1];
            MoveTo(table.GetPosition());
            foreach(Client client in (List<Client>)clientsTable[0])
            {
                client.RecevoirTable(table);
            }
            attenteClients = 5;
            etat = "attendsClientsTable";
            Console.WriteLine("CR " + id + " viens de recevoir des clients pour la table " + table.GetNumTable());
        }

        public string GetEtat()
        {
            return etat;
        }

        private void GettingCommande(Table table)
        {
            List<List<String>> commandes = new List<List<string>>();

            foreach(Client client in table.GetClients())
            {
                commandes.Add(client.DonnerMenu());
            }
        }

        private List<Table> CheckTablesClientsEtat(string etatToCheck)
        {
            List<Table> tablesRemplies = loader.GetWorld().getRestaurant().GetFilledTables();
            List<Table> tablesCorrespondent = new List<Table>();

            foreach (Table table in tablesRemplies)
            {
                bool clientsCorrespondent = true;

                foreach (Client client in table.GetClients())
                {
                    if (client.GetEtat() != etatToCheck)
                    {
                        clientsCorrespondent = false;
                        break;
                    }
                }
                if(clientsCorrespondent == true)
                {
                    tablesCorrespondent.Add(table);
                }
            }
            return tablesCorrespondent;
        }
    }
}
