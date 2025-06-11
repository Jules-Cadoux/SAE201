using SAE201.Model;
using System;
using System.Collections.Generic;
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
    /// Logique d'interaction pour UserControlModifierDemande.xaml
    /// </summary>
    public partial class UserControlModifierDemande : UserControl
    {
        public Demande DemandeSelectionnee { get; set; }

        public UserControlModifierDemande(Demande demande)
        {
            InitializeComponent();
            DemandeSelectionnee = demande;
            ChargerStatut();
        }
        private void ChargerStatut()
        {
            switch (DemandeSelectionnee.Accepter)
            {
                case "Accepter":
                    cbStatut.SelectedIndex = 0;
                    break;
                case "En Attente":
                    cbStatut.SelectedIndex = 1;
                    break;
                case "Refuser":
                    cbStatut.SelectedIndex = 2;
                    break;
            }
        }

        private void ButtonEnregistrer_Click(object sender, RoutedEventArgs e)
        {
            DemandeSelectionnee.Accepter = ((ComboBoxItem)cbStatut.SelectedItem).Content.ToString();
            MessageBox.Show("Le statut a été mis à jour.");
            Window parentWindow = Window.GetWindow(this);
            parentWindow.DialogResult = true;
            parentWindow.Close();
        }
    }
}
