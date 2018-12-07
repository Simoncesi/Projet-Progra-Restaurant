using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class Objet: Entity
    {
        private Entity conteneur;

        public Objet(Entity conteneur, Loader loader) : base(loader)
        {
            this.conteneur = conteneur;
        }

        public Entity GetConteneur()
        {
            return conteneur;
        }

        public void SetConteneur(Entity conteneur)
        {
            this.conteneur = conteneur;
        }
    }

    public class Divers: Objet
    {
        public string type;
        public int quantity;

        public Divers(string type, Entity conteneur, Loader loader, int quantity = 1) : base(conteneur, loader)
        {
            this.type = type;
            this.quantity = quantity;
        }
    }

    public class Nourriture: Objet
    {
        public string nom;
        private string stockage;

        public Nourriture(string nom, string stockage, Entity conteneur, Loader loader) : base(conteneur, loader)
        {
            this.nom = nom;
            this.stockage = stockage;
        }
    }
}
