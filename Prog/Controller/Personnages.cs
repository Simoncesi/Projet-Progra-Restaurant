using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public abstract class Personnage: PhysicalEntity
    {
        public string nom;
        public string prenom;
        public int age;
        protected List<Objet> inventaire;

        public Personnage(string nom, string prenom, int age, int[] position, Loader loader):base(loader, position)
        {
            this.nom = nom;
            this.prenom = prenom;
            this.age = age;

            this.inventaire = new List<Objet>();
        }
    }

    public class Client: Personnage
    {
        public int attente;

        public Client(int argent, int attente, string nom, string prenom, int age, int[] position, Loader loader) :base(nom, prenom, age, position, loader)
        {
            this.inventaire.Add(new Divers(loader, "argent"));
            this.attente = attente;
        }
    }
}
