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
using System.Windows.Shapes;

namespace SAE201
{
    /// <summary>
    /// Logique d'interaction pour Connection.xaml
    /// </summary>
    public partial class Connection : Window
    {
        public List<Employe> Employe {  get; set; }
        public Connection()
        {
            ChargeData();
            InitializeComponent();
        }
        
        public void ChargeData()
        {
            try
            {
                Employe = new List<Employe>(new Employe().FindAll());
            }
            catch(Exception ex) 
            {
                MessageBox.Show("Problème lors de la récupération des données,veuillez consulter votre admin");
                LogError.Log(ex, "Erreur SQL");
                Application.Current.Shutdown();
            }   
        }

        /*private void seConnecter_Click(object sender, RoutedEventArgs e)
        {
            Employe user = Employe.Find(x => x.Login == txLogin.Text && x.Mdp == txMdp.Text);
            if (user is null)
            {
                MessageBox.Show("Mot de passe ou login incorrect, réessayer");
            }
            else
            {
                DialogResult = true;
            }
        }*/
    }
}
