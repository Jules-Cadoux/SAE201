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
            Vin unVin = obj as Vin;
            if (unVin == null)
                return false;

            // Filtre par nom de vin
            if (textRechercheVin != null && !String.IsNullOrEmpty(textRechercheVin.Text))
            {
                if (String.IsNullOrEmpty(unVin.NomVin) ||
                    !unVin.NomVin.ToLower().Contains(textRechercheVin.Text.ToLower()))
                {
                    return false;
                }
            }

            // Filtre par type de vin - CORRIGÉ
            if (comboTypeVin != null && comboTypeVin.SelectedItem is ComboBoxItem typeItem &&
                typeItem.Content.ToString() != "Tous les types")
            {
                string typeSelectionne = typeItem.Content.ToString();

                // Mapping des types selon votre logique métier
                // Adaptez cette partie selon la structure de votre classe Vin
                string typeVin = "";
                switch (unVin.NumType)
                {
                    case 1:
                        typeVin = "Rouge";
                        break;
                    case 2:
                        typeVin = "Blanc";
                        break;
                    case 3:
                        typeVin = "Rosé";
                        break;
                    default:
                        typeVin = "";
                        break;
                }

                if (!typeVin.Equals(typeSelectionne, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            // Filtre par appellation - CORRIGÉ
            if (comboAppellation != null && comboAppellation.SelectedItem is ComboBoxItem appellationItem &&
                appellationItem.Content.ToString() != "Toutes appellations")
            {
                // Vérification de la propriété d'appellation
                if (unVin.NumType2 == null ||
                    String.IsNullOrEmpty(unVin.NumType2.NomAppelation.ToString()) ||
                    !unVin.NumType2.NomAppelation.ToString().Equals(appellationItem.Content.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            // Filtre par année exacte - CORRIGÉ
            if (textAnnee != null && !String.IsNullOrEmpty(textAnnee.Text))
            {
                if (int.TryParse(textAnnee.Text, out int anneeRecherchee))
                {
                    if (unVin.Millesime != anneeRecherchee)
                    {
                        return false;
                    }
                }
                else
                {
                    // Si la saisie n'est pas un nombre valide, on ignore ce filtre
                    // Ou vous pouvez choisir de retourner false pour masquer tous les résultats
                }
            }

            // Filtre par prix exact - CORRIGÉ
            if (textPrix != null && !String.IsNullOrEmpty(textPrix.Text))
            {
                if (double.TryParse(textPrix.Text, out double prixRecherche))
                {
                    if (Math.Abs(unVin.PrixVin - prixRecherche) > 0.01) // Comparaison avec tolérance pour les doubles
                    {
                        return false;
                    }
                }
                else
                {
                    // Si la saisie n'est pas un nombre valide, on ignore ce filtre
                    // Ou vous pouvez choisir de retourner false pour masquer tous les résultats
                }
            }

            return true;
        }

        private void RefreshRecherche(object sender, TextChangedEventArgs e)
        {
            VinsView?.Refresh();
        }

        private void textRechercheVin_GotFocus(object sender, RoutedEventArgs e)
        {
            if (labRechercheVin != null)
                labRechercheVin.Content = "";
        }

        private void ComboTypeVin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VinsView?.Refresh();
        }

        private void ComboAppellation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VinsView?.Refresh();
        }

        private void TextAnnee_TextChanged(object sender, TextChangedEventArgs e)
        {
            VinsView?.Refresh();
        }

        private void TextPrix_TextChanged(object sender, TextChangedEventArgs e)
        {
            VinsView?.Refresh();
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------

    }
}