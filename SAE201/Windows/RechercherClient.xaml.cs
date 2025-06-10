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

            // Créer une fenêtre pour contenir le UserControl
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
    }
}
