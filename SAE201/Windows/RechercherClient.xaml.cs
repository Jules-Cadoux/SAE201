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

    }
}
