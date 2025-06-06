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
using System.Windows.Shapes;

namespace SAE201
{
    /// <summary>
    /// Logique d'interaction pour Connection.xaml
    /// </summary>
    public partial class Connection : Window
    {
        public Connection()
        {
            InitializeComponent();
        }

        private void seConnecter_Click(object sender, RoutedEventArgs e)
        {
            string login = txLogin.Text;
            string mdp = txMdp.Text;
            bool res = dataReservation.ConnexionBD(login, mdp);
            if (res == true)
            {
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Mot de passe ou login incorrect, réessayer");
            }
        }
    }
}
