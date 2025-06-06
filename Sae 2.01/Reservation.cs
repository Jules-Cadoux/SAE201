using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sae_2._01
{
    public class Reservation
    {
        private int num_reservation;
        private Assurance une_assurance;
        private Client un_client;
        private DateTime date_reservation;
        private DateTime date_debut_reservation;
        private DateTime date_fin_reservation;
        private double montant_reservation;
        private string forfait_km;
        private List<Vehicule> vehicules;
       
        

        public Reservation(int num_reservation, DateTime date_reservation, DateTime date_debut_reservation, DateTime date_fin_reservation, double montant_reservation, string forfait_km,int num_assurance, string description_assurance, int prix_assurance, int num_client, string nom_client, string telephone_client, string mail_client, List<Vehicule> vehicules )
        {
            Num_reservation = num_reservation;
            Date_reservation = date_reservation;
            Date_debut_reservation = date_debut_reservation;
            Date_fin_reservation = date_fin_reservation;
            Montant_reservation = montant_reservation;
            Forfait_km = forfait_km;
            Une_assurance = new Assurance(num_assurance,description_assurance,prix_assurance);
            Un_client = new Client(num_client,nom_client,telephone_client,mail_client);
            Vehicules = vehicules;
        }



        public int Num_reservation
        {
            get => num_reservation;
            set 
            {
                
                num_reservation = value; 
            }
        }
        public DateTime Date_reservation { get => date_reservation; set => date_reservation = value; }
        public DateTime Date_debut_reservation { get => date_debut_reservation; set => date_debut_reservation = value; }
        public DateTime Date_fin_reservation { get => date_fin_reservation; set => date_fin_reservation = value; }
        public double Montant_reservation
        {
            get => montant_reservation;
            set
            {
                
                
                montant_reservation = value;
            }
        }
        public string Forfait_km
        {
            get => forfait_km;
            set
            {
                if (value.Length > 10)
                    throw new ArgumentException("Le forfait kilométrique ne doit pas faire plus de 10 caractères");
                forfait_km = value;
            }
        }

        public Assurance Une_assurance { get => une_assurance; set => une_assurance = value; }
        public List<Vehicule> Vehicules { get => vehicules; set => vehicules = value; }
        public Client Un_client { get => un_client; set => un_client = value; }

        public Assurance Assurance
        {
            get => default;
            set
            {
            }
        }

        public Vehicule Vehicule
        {
            get => default;
            set
            {
            }
        }

        public Client Client
        {
            get => default;
            set
            {
            }
        }
    }
}
