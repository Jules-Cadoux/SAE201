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
    public partial class RechercherVin : Page
    {
        public ObservableCollection<Vin> Vins { get; set; }
        public ICollectionView VinsView { get; set; }
        public RechercherVin()
        {
            InitializeComponent();
            Vins = new ObservableCollection<Vin>();
            VinsView = CollectionViewSource.GetDefaultView(Vins);
            VinsView.Filter = RechercheMotClefVin;
            List<Vin> vinsFromDb = Vin.FindAll();
            if (vinsFromDb != null)
            {
                foreach (Vin vin in vinsFromDb)
                {
                    Vins.Add(vin);
                }
            }
            DataContext = this;
        }
        private bool RechercheMotClefVin(object obj)
        {
            if (String.IsNullOrEmpty(textRechercheVin.Text))
                return true;
            Vin unVin = obj as Vin;
            return (unVin.NomVin.StartsWith(textRechercheVin.Text, StringComparison.OrdinalIgnoreCase));
        }
        private void RefreshRecherche(object sender, TextChangedEventArgs e)
        {
            if (VinsView != null)
            {
                CollectionViewSource.GetDefaultView(VinsView).Refresh();
            }
        }

        private void textRechercheVin_GotFocus(object sender, RoutedEventArgs e)
        {
            labRechercheVin.Content = "";
        }
    }
}
