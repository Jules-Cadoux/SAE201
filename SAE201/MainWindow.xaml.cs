using SAE201.Model;
using System.Collections.ObjectModel;
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
        public ObservableCollection<Vin> Vins { get; set; }
        public MainWindow()
        {
            Connection co = new Connection();
            bool? result = co.ShowDialog();
            if (result == true)
            {
                InitializeComponent();
            }
            else
            {
                Application.Current.Shutdown();
            }
            MainWindow mainWindow = this;
            mainWindow.Show();
            Vins = new ObservableCollection<Vin>(Vin.FindAll());
            DataContext = this;
        }
    }
}