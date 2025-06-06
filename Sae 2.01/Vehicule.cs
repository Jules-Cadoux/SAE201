using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sae_2._01
{
    public class Vehicule
    {
        private string immatriculation;
        private string type_boite;    
        private string nom_vehicule;
        private string description_vehicule;
        private int nombre_places;
        private double prix_location;
        private bool climatisation;
        private string line_photo_url;
        private Magasin un_magasin;
        private Categorie_Vehicule une_categorie;
        private List<Caracteristique> caracteristiques;

        public Vehicule(string immatriculation, string type_boite, string nom_vehicule, string description_vehicule, int nombre_places, double prix_location, bool climatisation, string line_photo_url, int num_magasin, string nom_magasin, string adresse_rue_magasin, string adresse_cp_magasin, string adresse_ville_magasin, string horaire_magasin, string nom_categorie)
        {
            Immatriculation = immatriculation;
            Type_boite = type_boite;
            Nom_vehicule = nom_vehicule;
            Description_vehicule = description_vehicule;
            Nombre_places = nombre_places;
            Prix_location = prix_location;
            Climatisation = climatisation;
            Line_photo_url = line_photo_url;
            Un_magasin = new Magasin(num_magasin,nom_magasin,adresse_rue_magasin,adresse_cp_magasin,adresse_ville_magasin,horaire_magasin);
            Une_categorie = new Categorie_Vehicule(nom_categorie);
            
        }

        public Vehicule(string immatriculation, string type_boite, string nom_vehicule, string description_vehicule, int nombre_places, double prix_location, bool climatisation, string line_photo_url, int num_magasin, string nom_magasin, string adresse_rue_magasin, string adresse_cp_magasin, string adresse_ville_magasin, string horaire_magasin, string nom_categorie, List<Caracteristique> caracteristiques)
        {
            Immatriculation = immatriculation;
            Type_boite = type_boite;
            Nom_vehicule = nom_vehicule;
            Description_vehicule = description_vehicule;
            Nombre_places = nombre_places;
            Prix_location = prix_location;
            Climatisation = climatisation;
            Line_photo_url = line_photo_url;
            Un_magasin = new Magasin(num_magasin, nom_magasin, adresse_rue_magasin, adresse_cp_magasin, adresse_ville_magasin, horaire_magasin);
            Une_categorie = new Categorie_Vehicule(nom_categorie);
            Caracteristiques = caracteristiques;
        }




        public string Immatriculation { get => immatriculation;
            set
            {
                if (value == null || value.Length != 7)
                    throw new ArgumentException("Une plaque d'immatriculation doit être composé de 7 caractères");
                immatriculation = value;
            }
        }
        public string Type_boite
        {
            get => type_boite;
            set
            {
                
                if (value != "manuelle" && value != "automatique")
                    throw new ArgumentException("La boite de vitesse doit être manuelle ou automatique");
                type_boite = value;
            }
        }
        public string Nom_vehicule
        {
            get => nom_vehicule;
            set
            {
                if (value == null || value.Length > 50)
                    throw new ArgumentException("Le nom du véhicule doit faire moins de 50 caractères");

                nom_vehicule = value;
            }
        }
        public string Description_vehicule
        {
            get => description_vehicule;
            set
            {
                if (value.Length > 300)
                    throw new ArgumentException("La description doit faire moins de 300 cractères");
                description_vehicule = value;
            }
        }
        public int Nombre_places
        {
            get => nombre_places;
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Le prix de location ne doit pas être nul");

                nombre_places = value;
            }
        }
        public double Prix_location
        {
            get => prix_location;
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Le prix de location ne doit pas être nul");
                prix_location = value;
            }
        }
        public bool Climatisation
        {
            get => climatisation;
            set
            {
                if (value == null)
                    throw new ArgumentNullException("La valeur de la climatisation doit être true ou false");
                climatisation = value;
            }
        }
        public string Line_photo_url
        {
            get => line_photo_url;
            set
            {
                if (value.Length > 100)
                    throw new ArgumentException("L'url de la photo ne doit pas faire plus de 100 caractères");
                line_photo_url = value;
            }
        }

        public Magasin Un_magasin { get => un_magasin; set => un_magasin = value; }
        public Categorie_Vehicule Une_categorie { get => une_categorie; set => une_categorie = value; }
        public List<Caracteristique> Caracteristiques { get => caracteristiques; set => caracteristiques = value; }

        public Caracteristique Caracteristique
        {
            get => default;
            set
            {
            }
        }

        public Magasin Magasin
        {
            get => default;
            set
            {
            }
        }

        public Categorie_Vehicule Categorie_Vehicule
        {
            get => default;
            set
            {
            }
        }
    }
}
