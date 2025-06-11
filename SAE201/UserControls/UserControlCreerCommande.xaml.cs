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
                this.DataContext = this;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problème lors de récupération des données" + ex.Message);
                LogError.Log(ex, "Erreur SQL");
            }
        }

        /*private void buttEditerDemande_Click(object sender, RoutedEventArgs e)
        {
            if (dgCommandes.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un client", "Attention", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Demande demandeSelectionne = (Demande)dgCommandes.SelectedItem;
                Demande copie = new Demande(demandeSelectionne.NumDemande, demandeSelectionne.NumVin, demandeSelectionne.NumEmploye, demandeSelectionne.NumClient);

                UserControlCréerCommande ucCommande = new UserControlCréerCommande();

                Window dialogWindow = new Window()
                {
                    Title = "Modifier Demande",
                    Content = ucCommande,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Owner = this,
                    ResizeMode = ResizeMode.NoResize
                };

                bool? result = dialogWindow.ShowDialog();
                if (result == true)
                {
                    try
                    {
                        copie.Update();
                        demandeSelectionne.Accepter = copie.Accepter;

                        dgCommandes.Items.Refresh();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Le client n'a pas pu être modifié.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }*/
    }
}
