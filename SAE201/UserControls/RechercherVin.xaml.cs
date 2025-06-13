using Npgsql;
using SAE201.Model;
using SAE201.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SAE201.UserControls
{
    public class VinDemande
    {
        public string NomVin { get; set; }
        public DateTime Date { get; set; }
        public int Quantite { get; set; }
        public int NumClient { get; set; }
        public int NumVin { get; set; }

        public VinDemande(string nomVin, DateTime date, int quantite)
        {
            NomVin = nomVin;
            Date = date;
            Quantite = quantite;
        }
    }
    public partial class RechercherVin : UserControl
    {
        
        
        //------------------------------------CLASSES MODELE ...------------------------------------
        
        private readonly Employe employeConnecte;
        public ObservableCollection<Vin> Vins { get; set; }
        public ICollectionView VinsView { get; set; }

        public ObservableCollection<VinDemande> VinsDemande { get; set; } = new ObservableCollection<VinDemande>();
        public ObservableCollection<Demande> LesDemandes { get; set; }
        private readonly Action logout;

        public RechercherVin(Employe employe, Action logoutAction)  
        {
            InitializeComponent();
            this.employeConnecte = employe;
            this.logout = logoutAction;
            Vins = new ObservableCollection<Vin>();
            VinsView = CollectionViewSource.GetDefaultView(Vins);
            VinsView.Filter = RechercheMotClefVin;
            ChargerVins();
            DataContext = this;
            ChargeData();
        }

        //----------------------------------------GESTION DONNEES-----------------------------------------
        private void ChargerVins()
        {
            try
            {
                List<Vin> vinsFromDb = Vin.FindAll();
                if (vinsFromDb != null)
                {
                    foreach (Vin vin in vinsFromDb)
                    {
                        Vins.Add(vin);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des vins : {ex.Message}");
            }
        }


        public void ChargeData()
        {
            try
            {
                List<Demande> demandes = Demande.FindAll();
                LesDemandes = new ObservableCollection<Demande>(demandes);
                this.DataContext = this;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problème lors de récupération des données" + ex.Message);
                LogError.Log(ex, "Erreur SQL");
            }
        }

        //-------------------------------------------------RECHERCHE VIN-------------------------------------------

        private bool RechercheMotClefVin(object obj)
        {
            Vin unVin = obj as Vin;
            if (unVin == null)
                return false;

            if (textRechercheVin != null && !String.IsNullOrEmpty(textRechercheVin.Text))
            {
                if (String.IsNullOrEmpty(unVin.NomVin) ||
                    !unVin.NomVin.ToLower().Contains(textRechercheVin.Text.ToLower()))
                {
                    return false;
                }
            }

            if (comboTypeVin != null && comboTypeVin.SelectedItem is ComboBoxItem typeItem &&
                typeItem.Content.ToString() != "Tous les types")
            {
                string typeSelectionne = typeItem.Content.ToString();

                string typeVin = "";
                switch (unVin.NumType)
                {
                    case 1:
                        typeVin = "Rouge";
                        break;
                    case 2:
                        typeVin = "Blanc";
                        break;
                    case 3:
                        typeVin = "Rosé";
                        break;
                    default:
                        typeVin = "";
                        break;
                }

                if (!typeVin.Equals(typeSelectionne, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            if (comboAppellation != null && comboAppellation.SelectedItem is ComboBoxItem appellationItem &&
                appellationItem.Content.ToString() != "Toutes appellations")
            {
                string appellationSelectionnee = appellationItem.Content.ToString();

                string appellationVin = "";
                if (unVin.NumType2 != null)
                {
                    switch (unVin.NumType2.NumType2)
                    {
                        case 1:
                            appellationVin = "AOP";
                            break;
                        case 2:
                            appellationVin = "AOC";
                            break;
                        case 3:
                            appellationVin = "IGP";
                            break;
                        default:
                            appellationVin = "";
                            break;
                    }
                }

                if (!appellationVin.Equals(appellationSelectionnee, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            if (textAnnee != null && !String.IsNullOrEmpty(textAnnee.Text))
            {
                if (int.TryParse(textAnnee.Text, out int anneeRecherchee))
                {
                    if (unVin.Millesime != anneeRecherchee)
                    {
                        return false;
                    }
                }
            }


            if (textPrix != null && !String.IsNullOrEmpty(textPrix.Text) &&
                double.TryParse(textPrix.Text, out double prixMax))
            {
                if (unVin.PrixVin > prixMax)
                {
                    return false;
                }
            }

            return true;
        }

        private void RefreshRecherche(object sender, TextChangedEventArgs e)
        {
            VinsView?.Refresh();
        }

        private void textRechercheVin_GotFocus(object sender, RoutedEventArgs e)
        {
            if (labRechercheVin != null)
                labRechercheVin.Content = "";
        }

        private void ComboTypeVin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VinsView?.Refresh();
        }

        private void ComboAppellation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VinsView?.Refresh();
        }

        private void TextAnnee_TextChanged(object sender, TextChangedEventArgs e)
        {
            VinsView?.Refresh();
        }

        private void TextPrix_TextChanged(object sender, TextChangedEventArgs e)
        {
            VinsView?.Refresh();
        }

        private void BtnReinitialiser_Click(object sender, RoutedEventArgs e)
        {
            if (textRechercheVin != null)
            {
                textRechercheVin.Text = "";
                if (labRechercheVin != null)
                    labRechercheVin.Content = "Nom du vin";
            }

            if (comboTypeVin != null)
                comboTypeVin.SelectedIndex = 0;

            if (comboAppellation != null)
                comboAppellation.SelectedIndex = 0;

            if (textAnnee != null)
                textAnnee.Text = "";

            if (textPrix != null)
                textPrix.Text = "";

            VinsView?.Refresh();
        }



        //--------------------------------------AJOUT VIN AUX DEMANDES-------------------------------------

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if (btn != null && btn.Tag is Vin vinSelectionne)
            {
                if (vinSelectionne.NumFournisseur == null || vinSelectionne.NumType2 == null)
                {
                    MessageBox.Show("Le vin sélectionné est incomplet.");
                    return;
                }

                VinsDemande.Add(new VinDemande(vinSelectionne.NomVin, DateTime.Now, 1));
                MessageBox.Show($"Vin '{vinSelectionne.NomVin}' ajouté ! Total: {VinsDemande.Count} vins");
            }
            else
            {
                MessageBox.Show("Erreur : Aucun vin sélectionné.");
            }
        }
        private void butAjouterClient_Click(object sender, RoutedEventArgs e)
        {
            if (VinsDemande.Count == 0)
            {
                MessageBox.Show("Aucune demande à valider.");
                return;
            }

            RechercherClient fenetreRecherche = new RechercherClient();

            if (fenetreRecherche.ShowDialog() == true)
            {
                int numClientSelectionne = fenetreRecherche.NumClientSelectionne.Value;
                foreach (VinDemande demande in VinsDemande)
                {
                    demande.NumClient = numClientSelectionne;
                }
                dgDemande.Items.Refresh();
            }
        }

        private void butValiderDemande_Click(object sender, RoutedEventArgs e)
        {
            if (VinsDemande.Count == 0)
            {
                MessageBox.Show("Aucune demande à valider.");
                return;
            }

            if (VinsDemande.Any(v => v.NumClient == 0))
            {
                MessageBox.Show("Veuillez sélectionner un client pour chaque demande.",
                                "Client manquant", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                foreach (VinDemande vin in VinsDemande)
                {
                    Vin? vinBdd = Vins.FirstOrDefault(v => v.NomVin == vin.NomVin);
                    if (vinBdd == null)
                    {
                        MessageBox.Show($"Le vin {vin.NomVin} n'existe pas dans la base.");
                        continue;
                    }
                    using NpgsqlCommand cmd = new NpgsqlCommand(@"INSERT INTO sae201_nicolas.demande
                            (numvin, numemploye, numcommande, numclient, datedemande, quantitedemande, accepter)
                            VALUES (@numvin, @numemploye, @numcommande, @numclient, @datedemande, @quantite, 'En Attente')
                            RETURNING numdemande");

                    cmd.Parameters.AddWithValue("numvin", vinBdd.NumVin);
                    cmd.Parameters.AddWithValue("numemploye", this.employeConnecte.NumEmploye); 
                    cmd.Parameters.AddWithValue("numcommande", DBNull.Value);
                    cmd.Parameters.AddWithValue("numclient", vin.NumClient);
                    cmd.Parameters.AddWithValue("datedemande", vin.Date);
                    cmd.Parameters.AddWithValue("quantite", vin.Quantite);

                    int idDemande = DataAccess.Instance.ExecuteInsert(cmd);

                    Demande demande = new Demande
                    {
                        NumDemande = idDemande,
                        DateDemande = vin.Date,
                        QuantiteDemande = vin.Quantite,
                        Accepter = "En Attente",
                        NumVin = vinBdd,
                        NumEmploye = new Employe { NumEmploye = this.employeConnecte.NumEmploye },
                        NumClient = new Client { NumClient = vin.NumClient }
                    };

                    LesDemandes.Add(demande);
                }

                MessageBox.Show("La demande a été validée", "Validation demande",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'enregistrement des demandes : " + ex.Message);
                LogError.Log(ex, "Erreur SQL");
            }

            VinsDemande.Clear();
            dgEtatDemande.Items.Refresh();
            ChargeData();
        }


        private void buttSupprimer_Click(object sender, RoutedEventArgs e)
        {
            if (dgDemande.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner une demande à supprimer", "Attention", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            VinDemande demandeAsupprimer = (VinDemande)dgDemande.SelectedItem;

            MessageBoxResult result = MessageBox.Show("Voulez-vous vraiment supprimer ce demande ?", "Con firmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    VinsDemande.Remove(demandeAsupprimer);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Le chien n'a pas pu être supprimé.\n" + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        //-----------------------------------------------SE DECONNECTER---------------------------------------

        private void buttDeconnexion_Click(object sender, RoutedEventArgs e)
        {
            logout?.Invoke();
        }

    }
}