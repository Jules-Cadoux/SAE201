using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sae_2._01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Vehicule> vehicules = new List<Vehicule>();
        private DateTime dtDebut;
        private DateTime dtFin;
        private int nbjours;
        double multiplicateurAssurance;
        double montanttotal;
        string forfait_km;
        Assurance assurance;
        public MainWindow()
        {
            InitializeComponent();
            Connection connection = new Connection();
            connection.ShowDialog();


        }



        private void btCréerClient_Click(object sender, RoutedEventArgs e)
        {
            Client nouveauClient = new Client();
            AddClient addclient = new AddClient();
            addclient.mainpanelClient.DataContext = nouveauClient;
            addclient.ShowDialog();
            if (addclient.DialogResult == true)
            {
                dataReservation.LesClients.Add(nouveauClient);
                dgClients.SelectedItem = nouveauClient;
                dataReservation.Createclient(nouveauClient);
            }
        }
        private bool ContientMotClefVehiculeCategorie(object obj)
        {
            Vehicule unVehicule = obj as Vehicule;
            if (String.IsNullOrEmpty(rechercheCategorie.Text))
                return true;
            else
                return (unVehicule.Une_categorie.Nom_categorie.StartsWith(rechercheCategorie.Text, StringComparison.OrdinalIgnoreCase));
                
        }

        private void rechercheCategorie_TextChanged(object sender, TextChangedEventArgs e)
        {
            dgSelectResa.Items.Filter = ContientMotClefVehiculeCategorie;

            CollectionViewSource.GetDefaultView(dgSelectResa.ItemsSource).Refresh();
        }

        private bool ContientMotClefVehiculeMagasin(object obj)
        {
            Vehicule unVehicule = obj as Vehicule;
            if (String.IsNullOrEmpty(rechercheMagasin.Text))
                return true;
            else
                return (unVehicule.Un_magasin.Nom_magasin.StartsWith(rechercheMagasin.Text, StringComparison.OrdinalIgnoreCase));

        }

       
        private bool ContientMotClefVehiculeManuelle(object obj)
        {
            Vehicule unVehicule = obj as Vehicule;
            if (String.IsNullOrEmpty("manuelle"))
                return true;
            else
                return (unVehicule.Type_boite.StartsWith("manuelle", StringComparison.OrdinalIgnoreCase));


        }

        private void rbManuelle_Checked(object sender, RoutedEventArgs e)
        {
            dgSelectResa.Items.Filter = ContientMotClefVehiculeManuelle;

            CollectionViewSource.GetDefaultView(dgSelectResa.ItemsSource).Refresh();
        }

        private bool ContientMotClefVehiculeAutomatique(object obj)
        {
            Vehicule unVehicule = obj as Vehicule;
            if (String.IsNullOrEmpty("automatique"))
                return true;
            else
                return (unVehicule.Type_boite.StartsWith("automatique", StringComparison.OrdinalIgnoreCase));
        }

        private void rbAutomatique_Checked(object sender, RoutedEventArgs e)
        {
            dgSelectResa.Items.Filter = ContientMotClefVehiculeAutomatique;

            CollectionViewSource.GetDefaultView(dgSelectResa.ItemsSource).Refresh();
        }

        private bool ContientMotClefClientNom(object obj)
        {
            Client unClient = obj as Client;
            if (String.IsNullOrEmpty(tbRecherche_ClientNom.Text))
                return true;
            else
                return (unClient.Nom_client.StartsWith(tbRecherche_ClientNom.Text, StringComparison.OrdinalIgnoreCase));
        }

        private void tbRecherche_ClientNom_TextChanged(object sender, TextChangedEventArgs e)
        {
            dgClients.Items.Filter = ContientMotClefClientNom;

            CollectionViewSource.GetDefaultView(dgClients.ItemsSource).Refresh();
        }

        private bool ContientMotClefClientTelephone(object obj)
        {
            Client unClient = obj as Client;
            if (String.IsNullOrEmpty(tbRechercheClientTelephone.Text))
                return true;
            else
                return (unClient.Telephone_client.StartsWith(tbRechercheClientTelephone.Text, StringComparison.OrdinalIgnoreCase));
        }

        private void tbRechercheClientTelephone_TextChanged(object sender, TextChangedEventArgs e)
        {
            dgClients.Items.Filter = ContientMotClefClientTelephone;

            CollectionViewSource.GetDefaultView(dgClients.ItemsSource).Refresh();
        }

        private bool ContientMotClefReservationClient(object obj)
        {
            Reservation UneReservation = obj as Reservation;
            if (String.IsNullOrEmpty(tbResaClient.Text))
                return true;
            else
                return (UneReservation.Un_client.Nom_client.StartsWith(tbResaClient.Text, StringComparison.OrdinalIgnoreCase));
        }

        private void tbResaClient_TextChanged(object sender, TextChangedEventArgs e)
        {
            dgResa.Items.Filter = ContientMotClefReservationClient;

            CollectionViewSource.GetDefaultView(dgResa.ItemsSource).Refresh();

        }

        private void btAjouter_Click(object sender, RoutedEventArgs e)
        {
            Vehicule vehicule = (Vehicule)dgSelectResa.SelectedItem;
            Console.WriteLine(vehicule.Immatriculation);
            if (vehicule != null)
            {
                Console.WriteLine(vehicule.Immatriculation);
                vehicules.Add(vehicule);
                dataReservation.LesVehiculesAjoutes.Add(vehicule);
                dgAjouter.SelectedItem = vehicule;
                CollectionViewSource.GetDefaultView(dgAjouter.ItemsSource).Refresh();
                Calculer_Montant();
            }


        }

        private void dpFin_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpFin.SelectedDate != null)
            {
               dtFin = (DateTime)dpFin.SelectedDate;
                Calculer_Montant();
            }
        }

        private void dpDebut_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpDebut.SelectedDate != null)
            {
                dtDebut = (DateTime)dpDebut.SelectedDate;
                Calculer_Montant();
            }
        }

        private void Calculer_Montant()
        {
            int nbjoursfin = dtFin.Year *365 +dtFin.Month *30 +dtFin.Day;
            int nbjoursdebut = dtDebut.Year*365 +dtDebut.Month *30 +dtDebut.Day;
            nbjours = nbjoursfin-nbjoursdebut +1;
            double montantvehicule = 0;
            foreach (Vehicule v in vehicules)
            {
                montantvehicule = montantvehicule + v.Prix_location * nbjours;

            }
            montantVehicule.Content = "Montant réservation avec véhicule : " + montantvehicule;
            montantAssurance.Content = "Pourcentage avec  l'assurance : " + Math.Round((multiplicateurAssurance * 100),2) + "%";
            montanttotal = montantvehicule + multiplicateurAssurance * montantvehicule;
            montantTotal.Content = "Montant total : " + montanttotal;

        }

        private void rbPasAssurance_Checked(object sender, RoutedEventArgs e)
        {
            multiplicateurAssurance = 0;
            assurance = dataReservation.LesAssurances[0];
        }

        private void rbDommageCorporel_Checked(object sender, RoutedEventArgs e)
        {
            multiplicateurAssurance = 0.07;
            Calculer_Montant();
            assurance = dataReservation.LesAssurances[2];

        }

        private void rbDommageVol_Checked(object sender, RoutedEventArgs e)
        {
            multiplicateurAssurance = 0.05;
            Calculer_Montant();
            assurance = dataReservation.LesAssurances[1];
        }

        private void rbDommageVolEtCorporel_Checked(object sender, RoutedEventArgs e)
        {

            multiplicateurAssurance = 0.1;
            Calculer_Montant();
            assurance = dataReservation.LesAssurances[3];
        }

        private void btenregistrerResa_Click(object sender, RoutedEventArgs e)
        {
            int num_reservation = dataReservation.LesReservations.Count+1;
            DateTime date_reservation = DateTime.Today;
            Client client = (Client)dgClients.SelectedItem;
            Assurance assu = assurance;
            Reservation resa = new Reservation(num_reservation,date_reservation,dtDebut,dtFin,montanttotal,forfait_km,assu.Num_assurance,assu.Description_assurance,assu.Prix_assurance,client.Num_client,client.Nom_client,client.Telephone_client,client.Mail_client,vehicules);
            int res = dataReservation.Createreservation(resa);
            if (res>0)
            {
                MessageBox.Show("Réservation enregistrer, fermeture de l'application");
                Environment.Exit(-1);
            }

        }

        private void rbMoins100_Checked(object sender, RoutedEventArgs e)
        {
            forfait_km = "-100km";
        }

        private void rb100A250_Checked(object sender, RoutedEventArgs e)
        {
            forfait_km = "100à250";
        }

        private void rb250Plus_Checked(object sender, RoutedEventArgs e)
        {
            forfait_km = "+250km";

        }

        private void rechercheMagasin_TextChanged(object sender, TextChangedEventArgs e)
        {
            dgSelectResa.Items.Filter = ContientMotClefVehiculeMagasin;

            CollectionViewSource.GetDefaultView(dgSelectResa.ItemsSource).Refresh();
        }

        private void txPlaque_TextChanged(object sender, TextChangedEventArgs e)
        {
            dgResa.Items.Filter = ContientMotClefVehiculeMagasin;

            CollectionViewSource.GetDefaultView(dgResa.ItemsSource).Refresh();


        }


        

        private void btAfficherPlaque_Click(object sender, RoutedEventArgs e)
        {
            Reservation res = (Reservation)dgResa.SelectedItem;
            string rep = "";
            int compteur = 0;
            foreach (Vehicule vec in res.Vehicules)
            {
                compteur += 1;
                rep += "Immatriculation " + compteur + " : " + vec.Immatriculation +"\n";
            }
            MessageBox.Show(rep);

        }
    }
}
