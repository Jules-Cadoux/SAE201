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
    /// Logique d'interaction pour UserControlCréerCommande.xaml
    /// </summary>
    public partial class UserControlCréerCommande : UserControl
    {
        public ObservableCollection<Demande> LesDemandes { get; set; }

        public UserControlCréerCommande()
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
    }
}
