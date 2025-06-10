using SAE201.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Logique d'interaction pour RechercherVin.xaml
    /// </summary>
    public partial class RechercherVin : UserControl 
    {
        public ObservableCollection<Vin> Vins { get; set; }
        public ICollectionView VinsView { get; set; }

        public RechercherVin()
        {
            InitializeComponent();
            Vins = new ObservableCollection<Vin>();
            VinsView = CollectionViewSource.GetDefaultView(Vins);
            VinsView.Filter = RechercheMotClefVin;

            // Charger les données depuis la base
            ChargerVins();

            DataContext = this;
        }

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

        private bool RechercheMotClefVin(object obj)
        {
            // Vérification de sécurité
            if (textRechercheVin == null || String.IsNullOrEmpty(textRechercheVin.Text))
                return true;

            Vin unVin = obj as Vin;
            if (unVin == null || String.IsNullOrEmpty(unVin.NomVin))
                return false;

            return unVin.NomVin.StartsWith(textRechercheVin.Text, StringComparison.OrdinalIgnoreCase);
        }

        private void RefreshRecherche(object sender, TextChangedEventArgs e)
        {
            // Correction : refresh direct sur VinsView
            VinsView?.Refresh();
        }

        private void textRechercheVin_GotFocus(object sender, RoutedEventArgs e)
        {
            if (labRechercheVin != null)
                labRechercheVin.Content = "";
        }




        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------

    }
}