using SAE201.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SAE201.UserControls
{
    public partial class UserControlCreerCommande : UserControl
    {
        private readonly Employe employeConnecte;
        public ObservableCollection<Demande> LesDemandes { get; set; }
        public ObservableCollection<Commande> LesCommandes { get; set; }
        public ObservableCollection<GroupeFournisseur> LesCommandesParFournisseur { get; set; }
        public event Action? CommandeCree;

        public UserControlCreerCommande(Employe employe)
        {
            InitializeComponent();
            this.employeConnecte = employe;
            LesDemandes = new ObservableCollection<Demande>();
            LesCommandes = new ObservableCollection<Commande>();
            LesCommandesParFournisseur = new ObservableCollection<GroupeFournisseur>();
            this.DataContext = this;
            ChargeData();
        }

        public void ChargeData()
        {
            try
            {
                List<Demande> demandesFromDb = Demande.FindAll();
                LesDemandes.Clear();
                foreach (var d in demandesFromDb.Where(d => (d.Accepter == "Accepter" && d.NumCommande == null) || d.Accepter == "En Attente" || d.Accepter == "Refuser"))
                {
                    LesDemandes.Add(d);
                }

                List<Commande> commandesFromDb = Commande.FindAll();
                LesCommandes.Clear();
                foreach (var c in commandesFromDb)
                {
                    LesCommandes.Add(c);
                }

                RegrouperDemandesParFournisseur();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problème lors de récupération des données: " + ex.Message);
                LogError.Log(ex, "Erreur SQL");
            }
        }

        public void ActualiserDonnees()
        {
            ChargeData();
        }

        private void RegrouperDemandesParFournisseur()
        {
            List<Demande> demandesAcceptees = LesDemandes.Where(d => d.Accepter == "Accepter").ToList();
            Dictionary<int, List<Demande>> demandesParFournisseur = new Dictionary<int, List<Demande>>();

            foreach (Demande demande in demandesAcceptees)
            {
                if (demande.NumVin?.NumFournisseur != null)
                {
                    int numFournisseur = demande.NumVin.NumFournisseur.NumFournisseur;
                    if (!demandesParFournisseur.ContainsKey(numFournisseur))
                    {
                        demandesParFournisseur[numFournisseur] = new List<Demande>();
                    }
                    demandesParFournisseur[numFournisseur].Add(demande);
                }
            }

            LesCommandesParFournisseur.Clear();

            foreach (KeyValuePair<int, List<Demande>> kvp in demandesParFournisseur)
            {
                GroupeFournisseur groupeFournisseur = new GroupeFournisseur();
                groupeFournisseur.NomFournisseur = kvp.Value[0].NumVin.NumFournisseur.NomFournisseur;
                groupeFournisseur.NumFournisseur = kvp.Key;
                groupeFournisseur.DemandesVins = new ObservableCollection<Demande>(kvp.Value);
                groupeFournisseur.PrixTotal = CalculerPrixTotal(kvp.Value);
                LesCommandesParFournisseur.Add(groupeFournisseur);
            }
        }

        private double CalculerPrixTotal(List<Demande> demandes)
        {
            double total = 0;
            foreach (Demande demande in demandes)
            {
                if (demande.NumVin != null)
                {
                    total += demande.NumVin.PrixVin * demande.QuantiteDemande;
                }
            }
            return total;
        }

        private void buttEditerDemande_Click(object sender, RoutedEventArgs e)
        {
            if (dgCommandes.SelectedItem is Demande demandeSelectionnee)
            {
                UserControlModifierDemande ucModifierDemande = new UserControlModifierDemande(demandeSelectionnee);
                Window dialogWindow = new Window()
                {
                    Title = "Modifier le statut de la demande",
                    Content = ucModifierDemande,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    ResizeMode = ResizeMode.NoResize
                };

                if (dialogWindow.ShowDialog() == true)
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
            else
            {
                MessageBox.Show("Veuillez sélectionner une demande", "Attention", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void buttValiderCommande_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button bouton && bouton.DataContext is GroupeFournisseur groupeFournisseur)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Êtes-vous sûr de vouloir valider la commande pour {groupeFournisseur.NomFournisseur} ?\n" +
                    $"Prix total : {groupeFournisseur.PrixTotal:C2}",
                    "Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        Commande commande = new Commande
                        {
                            NumEmploye = this.employeConnecte.NumEmploye,
                            DateCommande = DateTime.Now,
                            Valider = true,
                            PrixTotal = groupeFournisseur.PrixTotal
                        };
                        int numeroCommande = commande.Create();

                        foreach (Demande demande in groupeFournisseur.DemandesVins)
                        {
                            demande.NumCommande = new Commande { NumCommande = numeroCommande };
                            demande.UpdateCommande();
                        }

                        MessageBox.Show($"Commande #{numeroCommande} créée avec succès pour {groupeFournisseur.NomFournisseur}!");
                        ChargeData();
                        CommandeCree?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur lors de l'ajout : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        LogError.Log(ex, "Erreur lors de la création de commande");
                    }
                }
            }
        }
    }

    public class GroupeFournisseur
    {
        public string NomFournisseur { get; set; } = string.Empty;
        public int NumFournisseur { get; set; }
        public ObservableCollection<Demande> DemandesVins { get; set; }
        public double PrixTotal { get; set; }

        public GroupeFournisseur()
        {
            DemandesVins = new ObservableCollection<Demande>();
        }
    }
}