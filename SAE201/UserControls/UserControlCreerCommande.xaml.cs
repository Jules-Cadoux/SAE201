using Npgsql;
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
        //------------------------CLASSE MODELE ...----------------------------------------------------------
        public class GroupeFournisseur
        {
            public string NomFournisseur { get; set; }
            public int NumFournisseur { get; set; }
            public ObservableCollection<Demande> DemandesVins { get; set; }
            public double PrixTotal { get; set; }

            public GroupeFournisseur()
            {
                DemandesVins = new ObservableCollection<Demande>();
            }
        }

        private readonly Employe employeConnecte;
        private readonly Action logout; 
        public ObservableCollection<Demande> LesDemandes { get; set; }
        public ObservableCollection<Commande> LesCommandes { get; set; }
        public ObservableCollection<GroupeFournisseur> LesCommandesParFournisseur { get; set; } = new ObservableCollection<GroupeFournisseur>();


        public UserControlCreerCommande(Employe employe, Action logoutAction)
        {
            InitializeComponent();
            this.employeConnecte = employe;
            this.logout = logoutAction; 

            ChargeData();
        }

        //----------------------------------------GESTION DES DONNES-----------------------------------------------//

        public void ChargeData()
        {
            try
            {
                List<Demande> demandes = Demande.FindAll();

                LesDemandes = new ObservableCollection<Demande>(
                    demandes.Where(d => d.Accepter == "Accepter" && d.NumCommande == null || d.Accepter == "En Attente" || d.Accepter == "Refuser").ToList()
                );

                List<Commande> commandes = Commande.FindAll();
                LesCommandes = new ObservableCollection<Commande>(commandes);
                RegrouperDemandesParFournisseur();  
                this.DataContext = this;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problème lors de récupération des données: " + ex.Message);
                LogError.Log(ex, "Erreur SQL");
            }
        }


        //---------------------------------------------GESTION DEMANDES-------------------------------------------------------------

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
                        ChargeData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("La demande n'a pas pu être modifiée.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        LogError.Log(ex, "Erreur lors de la modification de demande");
                    }
                }
            }
        }



        //---------------------------------GESTION COMMANDES----------------------------------------


        private void RegrouperDemandesParFournisseur()
        {
            List<Demande> demandesAcceptees = LesDemandes.Where(d => d.Accepter == "Accepter").ToList();
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

            LesCommandesParFournisseur.Clear();

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

        

        private void buttValiderCommande_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button bouton = sender as Button;
                if (bouton == null)
                {
                    MessageBox.Show("Erreur: impossible de récupérer le bouton", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                GroupeFournisseur groupeFournisseur = bouton.DataContext as GroupeFournisseur;
                if (groupeFournisseur == null)
                {
                    MessageBox.Show("Erreur: impossible de récupérer les informations du fournisseur", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBoxResult result = MessageBox.Show(
                    $"Êtes-vous sûr de vouloir valider la commande pour {groupeFournisseur.NomFournisseur} ?\n" +
                    $"Nombre d'articles : {groupeFournisseur.DemandesVins.Count}\n" +
                    $"Prix total : {groupeFournisseur.PrixTotal:C2}",
                    "Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using NpgsqlCommand cmdCommande = new NpgsqlCommand(@"INSERT INTO sae201_nicolas.commande
                (numemploye, datecommande, valider, prixtotal)
                VALUES (@numemploye, @datecommande, @valider, @prixtotal)
                RETURNING numcommande");

                        cmdCommande.Parameters.AddWithValue("numemploye", this.employeConnecte.NumEmploye); 
                        cmdCommande.Parameters.AddWithValue("datecommande", DateTime.Now);
                        cmdCommande.Parameters.AddWithValue("valider", true);
                        cmdCommande.Parameters.AddWithValue("prixtotal", groupeFournisseur.PrixTotal);

                        int numeroCommande = DataAccess.Instance.ExecuteInsert(cmdCommande);

                        Commande commande = new Commande
                        {
                            NumCommande = numeroCommande,
                            DateCommande = DateTime.Now,
                            NumEmploye = this.employeConnecte.NumEmploye, 
                            Valider = true,
                            PrixTotal = groupeFournisseur.PrixTotal
                        };

                        if (numeroCommande > 0)
                        {
                            MessageBox.Show(
                                $"Commande #{numeroCommande} créée avec succès pour {groupeFournisseur.NomFournisseur}!\n" +
                                $"Prix total : {groupeFournisseur.PrixTotal:C2}",
                                "Succès",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information
                            );

                            foreach (Demande demande in groupeFournisseur.DemandesVins)
                            {
                                demande.NumCommande = new Commande { NumCommande = numeroCommande };
                                try
                                {
                                    demande.UpdateCommande();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Erreur lors de la mise à jour d'une demande : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                                    LogError.Log(ex, "Erreur lors de la mise à jour de demande");
                                }
                            }

                            LesCommandesParFournisseur.Remove(groupeFournisseur);
                            dgCommandes.Items.Refresh();  
                            ChargeData();
                        }
                        else
                        {
                            MessageBox.Show("Erreur: La commande n'a pas pu être créée (numéro invalide)", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur lors de l'insertion de la commande : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        LogError.Log(ex, "Erreur SQL lors de l'insertion de commande");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la création de la commande : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                LogError.Log(ex, "Erreur lors de la création de commande");
            }
        }



        //-----------------------------------------------SE DECONNECTER---------------------------------------

        private void buttDeconnexion_Click(object sender, RoutedEventArgs e)
        {
            logout?.Invoke();
        }
    }
}