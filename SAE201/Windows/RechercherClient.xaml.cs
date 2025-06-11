using SAE201.Model;
using SAE201.UserControls;
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
using System.Windows.Shapes;

namespace SAE201.Windows
{
    /// <summary>
    /// Logique d'interaction pour RechercherClient.xaml
    /// </summary>
    public partial class RechercherClient : Window
    {
        public ObservableCollection<Client> LesClients { get; set; }
        public RechercherClient()
        {
            InitializeComponent();            
            ChargeData();
            dgClients.Items.Filter = RechercheMotClefClient;

        }

        public void ChargeData()
        {
            try
            {
                List<Client> clients = new Client().FindAll();
                LesClients = new ObservableCollection<Client>(clients);
                this.DataContext = this; 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problème lors de récupération des données");
                LogError.Log(ex, "Erreur SQL");
            }
        }


        private bool RechercheMotClefClient(object obj)
        {
            if (String.IsNullOrEmpty(textMotClefClient.Text))
                return true;
            Client unClient = obj as Client;
            return unClient != null &&
                   (unClient.NomClient.StartsWith(textMotClefClient.Text, StringComparison.OrdinalIgnoreCase)
                   || unClient.PrenomClient.StartsWith(textMotClefClient.Text, StringComparison.OrdinalIgnoreCase));
        }

        private void textMotClefClient_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dgClients.ItemsSource)?.Refresh();
        }

        private void buttAjouter_Click(object sender, RoutedEventArgs e)
        {
            Client unClient = new Client();
            UserControlFicheClient ucClient = new UserControlFicheClient(unClient, ActionClient.Creer);

            Window dialogWindow = new Window()
            {
                Title = "Nouveau Client",
                Content = ucClient,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                ResizeMode = ResizeMode.NoResize
            };

            bool? result = dialogWindow.ShowDialog();
            if (result == true)
            {
                LesClients.Add(unClient);
            }
        }


        private void buttEditer_Click(object sender, RoutedEventArgs e)
        {
            if (dgClients.SelectedItem == null)
            {
                MessageBox.Show(this, "Veuillez sélectionner un client", "Attention", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Client clientSelectionne = (Client)dgClients.SelectedItem;
                Client copie = new Client(clientSelectionne.NumClient, clientSelectionne.NomClient, clientSelectionne.PrenomClient, clientSelectionne.MailClient);

                UserControlFicheClient ucClient = new UserControlFicheClient(copie, ActionClient.Modifier);

                Window dialogWindow = new Window()
                {
                    Title = "Modifier Client",
                    Content = ucClient,
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
                        clientSelectionne.NomClient = copie.NomClient;
                        clientSelectionne.PrenomClient = copie.PrenomClient;
                        clientSelectionne.MailClient = copie.MailClient;

                        dgClients.Items.Refresh();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "Le client n'a pas pu être modifié.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void buttSupprimer_Click(object sender, RoutedEventArgs e)
        {
            if (dgClients.SelectedItem == null)
            {
                MessageBox.Show(this, "Veuillez sélectionner un client à supprimer", "Attention", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Client clientAsupprimer = (Client)dgClients.SelectedItem;            

            MessageBoxResult result = MessageBox.Show(this, "Voulez-vous vraiment supprimer ce client ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    int nb = clientAsupprimer.Delete();
                    LesClients.Remove(clientAsupprimer);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Le chien n'a pas pu être supprimé.\n" + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
