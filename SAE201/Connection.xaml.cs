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

namespace SAE201
{
    /// <summary>
    /// Logique d'interaction pour Connection.xaml
    /// </summary>
    public partial class Connection : Window
    {
        public ObservableCollection<Employe> LesEmployes { get; set; }
        public Employe EmployeConnecte { get; set; }
        public Connection()
        {
            InitializeComponent();
            ChargeData();
        }
        
        public void ChargeData()
        {
            try
            {
                List<Employe> employes = new Employe().FindAll();
                LesEmployes = new ObservableCollection<Employe>(employes);
                this.DataContext = this;
            }
            catch(Exception ex) 
            {
                MessageBox.Show("Problème lors de la récupération des données,veuillez consulter votre admin");
                LogError.Log(ex, "Erreur SQL");
                Application.Current.Shutdown();
            }   
        }

        public void seConnecter_Click(object sender, RoutedEventArgs e)
        {
            Employe user = Employe.FindByLoginAndPassword(txLogin.Text, txMdp.Password);
            if (user is null)
            {
                MessageBox.Show("Mot de passe ou login incorrect, réessayer");
            }
            else
            {
                EmployeConnecte = user; 
                DialogResult = true;
            }
        }
    }
}
