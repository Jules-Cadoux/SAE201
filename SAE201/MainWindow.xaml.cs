using SAE201.Model;
using SAE201.UserControls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SAE201
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private Employe employeConnecte;

        public MainWindow()
        {
            InitializeComponent();
            Login();
            ChargeUserControl();
            

        }
        private void Vendeur()
        {
            Main.Content = new RechercherVin();
        }
        private void Responsable()
        {
            Main.Content = new UserControlCreerCommande();
        }
        private void Login()
        {
            Connection co = new Connection();
            if (co.ShowDialog()==true)
            {
                employeConnecte = co.EmployeConnecte;
            }
        }
        private void ChargeUserControl()
        {
            Main.Content=null;
            switch(employeConnecte.NumRole)
            {
                case 1:
                    Vendeur();
                    break;

                case 2:
                    Responsable();
                    break;
                default:
                    // Gérer le cas où le rôle n'est pas reconnu
                    MessageBox.Show("Rôle non reconnu");
                    break;
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    }
}