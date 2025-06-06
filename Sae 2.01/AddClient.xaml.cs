using Microsoft.VisualBasic;
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

namespace Sae_2._01
{
    /// <summary>
    /// Logique d'interaction pour AddClient.xaml
    /// </summary>
    public partial class AddClient : Window
    {
        public AddClient()
        {
            InitializeComponent();
        }

        private void btCréerClient_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
                     
        }

        private void btAnnulerClient_Click(object sender, RoutedEventArgs e)
        {
            DialogResult= false;
        }
    }
}
