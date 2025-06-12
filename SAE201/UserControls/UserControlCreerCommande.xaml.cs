using SAE201.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    /// <summary>
    /// Logique d'interaction pour UserControlCreerCommande.xaml
    /// </summary>
    public partial class UserControlCreerCommande : UserControl
    {
        public ObservableCollection<Demande> LesDemandes { get; set; }
        public ObservableCollection<GroupeFournisseur> LesCommandesParFournisseur { get; set; }


        public UserControlCreerCommande()
        {
            InitializeComponent();
            ChargeData();
        }

        public void ChargeData()
        {
            try
            {
                List<Demande> demandes = Demande.FindAll();
                LesDemandes = new ObservableCollection<Demande>(demandes);
                RegrouperDemandesParFournisseur();
                this.DataContext = this;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problème lors de récupération des données" + ex.Message);
                LogError.Log(ex, "Erreur SQL");
            }
        }


        private void RegrouperDemandesParFournisseur()
        {
            // Filtrer les demandes acceptées
            List<Demande> demandesAcceptees = LesDemandes.Where(d => d.Accepter == "Accepter").ToList();

            // Regrouper par fournisseur
            Dictionary<int, List<Demande>> demandesParFournisseur = new Dictionary<int, List<Demande>>();

            foreach (Demande demande in demandesAcceptees)
            {
                int numFournisseur = demande.NumVin.NumFournisseur.NumFournisseur;

                if (!demandesParFournisseur.ContainsKey(numFournisseur))
                {
                    demandesParFournisseur[numFournisseur] = new List<Demande>();
                }

                demandesParFournisseur[numFournisseur].Add(demande);
            }

            // Créer la collection pour l'affichage
            LesCommandesParFournisseur = new ObservableCollection<GroupeFournisseur>();

            foreach (KeyValuePair<int, List<Demande>> kvp in demandesParFournisseur)
            {
                GroupeFournisseur groupeFournisseur = new GroupeFournisseur();
                groupeFournisseur.NomFournisseur = kvp.Value[0].NumVin.NumFournisseur.NomFournisseur;
                groupeFournisseur.NumFournisseur = kvp.Key; 
                groupeFournisseur.DemandesVins = new ObservableCollection<Demande>();

                foreach (Demande demande in kvp.Value)
                {
                    groupeFournisseur.DemandesVins.Add(demande);
                }

                groupeFournisseur.PrixTotal = CalculerPrixTotal(kvp.Value);
                LesCommandesParFournisseur.Add(groupeFournisseur);
            }
        }

        private double CalculerPrixTotal(List<Demande> demandes)
        {
            double total = 0;
            foreach (Demande demande in demandes)
            {
                total += demande.NumVin.PrixVin * demande.QuantiteDemande;
            }
            return total;
        }


        private void buttEditerDemande_Click(object sender, RoutedEventArgs e)
        {
            if (dgCommandes.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner une demande", "Attention", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Demande demandeSelectionnee = (Demande)dgCommandes.SelectedItem;
                UserControlModifierDemande ucModifierDemande = new UserControlModifierDemande(demandeSelectionnee);
                Window dialogWindow = new Window()
                {
                    Title = "Modifier le statut de la demande",
                    Content = ucModifierDemande,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    ResizeMode = ResizeMode.NoResize
                };

                bool? result = dialogWindow.ShowDialog();
                if (result == true)
                {
                    try
                    {
                        demandeSelectionnee.Update();  
                        dgCommandes.Items.Refresh();   
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("La demande n'a pas pu être modifiée.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        private void buttValiderCommande_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            GroupeFournisseur groupeFournisseur = button.DataContext as GroupeFournisseur;

            if (groupeFournisseur != null)
            {
                try
                {
                    // Créer une nouvelle commande
                    Commande nouvelleCommande = new Commande();
                    nouvelleCommande.NumEmploye = 1; // À adapter selon votre logique d'employé connecté
                    nouvelleCommande.DateCommande = DateTime.Now;
                    nouvelleCommande.Valider = true;
                    nouvelleCommande.PrixTotal = groupeFournisseur.PrixTotal;

                    // Insérer la commande en base
                    int idCommande = nouvelleCommande.Create();

                    // Mettre à jour les demandes avec l'ID de la commande créée
                    foreach (Demande demande in groupeFournisseur.DemandesVins)
                    {
                        demande.NumCommande = new Commande();
                        demande.NumCommande.NumCommande = idCommande;
                        demande.Update();
                    }

                    MessageBox.Show("Commande validée avec succès pour " + groupeFournisseur.NomFournisseur + "!",
                                  "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Recharger les données
                    ChargeData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la validation de la commande : " + ex.Message,
                                  "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    // Classe pour regrouper les demandes par fournisseur
    public class GroupeFournisseur
    {
        public string NomFournisseur { get; set; }
        public int NumFournisseur { get; set; } // Utiliser directement NumFournisseur au lieu d'IdFournisseur
        public ObservableCollection<Demande> DemandesVins { get; set; }
        public double PrixTotal { get; set; }

        public GroupeFournisseur()
        {
            DemandesVins = new ObservableCollection<Demande>();
        }
    }
}


