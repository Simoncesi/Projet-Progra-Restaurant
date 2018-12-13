using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public abstract class Objet: Entity
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

        public override List<string[]> ReturnInformations()
        {
            List<string[]> infosToReturn = new List<string[]>();

            infosToReturn.Add(new string[] { "ID", id.ToString() });
            infosToReturn.Add(new string[] { "Type", type });
            infosToReturn.Add(new string[] { "Quantité", quantity.ToString() });

            return infosToReturn;
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

        public override List<string[]> ReturnInformations()
        {
            List<string[]> infosToReturn = new List<string[]>();

            infosToReturn.Add(new string[] { "ID", id.ToString() });
            infosToReturn.Add(new string[] { "Nom", nom });
            infosToReturn.Add(new string[] { "Stockage", stockage });

            return infosToReturn;
        }
    }

    public class Materiel: Objet
    {
        private bool propre;

        public Materiel(Entity conteneur, Loader loader, bool propre = true) : base(conteneur, loader)
        {
            this.propre = propre;
        }

        public override List<string[]> ReturnInformations()
        {
            List<string[]> infosToReturn = new List<string[]>();

            infosToReturn.Add(new string[] { "ID", id.ToString() });
            infosToReturn.Add(new string[] { "Propre", propre.ToString() });

            return infosToReturn;
        }

        public void Salir()
        {
            propre = false;
        }

        public void Nettoyer()
        {
            propre = true;
        }

        public bool GetProprete()
        {
            return propre;
        }
    }

    public class Ustensile: Materiel
    {
        public string ustensileType;
        public Ustensile(Entity conteneur, Loader loader, string ustensileType, bool propre = true) : base(conteneur, loader, propre)
        {
            this.ustensileType = ustensileType;
        }
    }

    public class ObjetConteneur: Materiel
    {
        //If it can contain solids or liquids
        public string contenuType;
        public string type;
        private List<Nourriture> contenuNourriture;

        public ObjetConteneur(Entity conteneur, Loader loader, string type, bool propre = true, string contenuType = "Solid") : base(conteneur, loader, propre)
        {
            this.type = type;
            this.contenuType = contenuType;
            contenuNourriture = new List<Nourriture>();
        }

        public void AjouterContenu(Nourriture nourriture)
        {
            contenuNourriture.Add(nourriture);
        }

        public bool RetirerContenu(Nourriture nourriture)
        {
            return contenuNourriture.Remove(nourriture);
        }

        public List<Nourriture> GetContenu()
        {
            return contenuNourriture;
        }
    }

    public class Plat: Objet
    {
        private ObjetConteneur platConteneur;
        public int platId;
        public string nomPlat;

        public Plat(string nomPlat, int platId, ObjetConteneur platConteneur, Entity conteneur, Loader loader) : base(conteneur, loader)
        {
            this.nomPlat = nomPlat;
            this.platId = platId;
            this.platConteneur = platConteneur;
        }

        public override List<string[]> ReturnInformations()
        {
            List<string[]> infosToReturn = new List<string[]>();

            infosToReturn.Add(new string[] { "ID", id.ToString() });
            infosToReturn.Add(new string[] { "Plat", nomPlat });

            return infosToReturn;
        }
    }

    public class Carte: Objet
    {
        private List<List<String>> menus;

        public Carte(Entity conteneur, Loader loader, List<List<String>> menus) : base(conteneur, loader)
        {
            this.menus = menus;
        }

        public override List<string[]> ReturnInformations()
        {
            List<string[]> infosToReturn = new List<string[]>();

            infosToReturn.Add(new string[] { "ID", id.ToString() });

            int i = 0;

            foreach(List<String> menu in menus)
            {
                i++;
                infosToReturn.Add(new string[] { "Menu "+i, menu[0]+" "+menu[1]+" "+menu[2] });
            }

            return infosToReturn;
        }

        public List<List<String>> GetMenus()
        {
            return menus;
        }
    }

}
