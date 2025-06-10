using Npgsql;
using SAE201.Model;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;

namespace SAE201.UserControls
{
    public enum ActionClient { Creer, Modifier }

    public partial class UserControlFicheClient : UserControl
    {
        private ActionClient action;
        public UserControlFicheClient()
        {
            InitializeComponent();
        }
        public UserControlFicheClient(Client unClient, ActionClient action)
        {
            InitializeComponent();
            this.DataContext = unClient;
            this.action = action;
            if (action == ActionClient.Creer)
            {
                butValider.Content = "Créer";
            }
            else
            {
                butValider.Content = "Modifier";
            }
        }

        public void butValider_Click(object sender, RoutedEventArgs e)
        {
            bool ok = true;
            foreach (UIElement uie in panelUCFormClient.Children)
            {
                if (uie is TextBox)
                {
                    TextBox txt = (TextBox)uie;
                    BindingExpression binding = txt.GetBindingExpression(TextBox.TextProperty);
                    if (binding != null)
                    {
                        binding.UpdateSource();
                    }
                }

                if (Validation.GetHasError(uie))
                {
                    ok = false;
                }
            }

            if (!ok)
            {
                MessageBox.Show("Veuillez corriger les erreurs dans le formulaire.", "Erreur de validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (action == ActionClient.Creer)
            {
                Client client = (Client)this.DataContext;

                try
                {
                    NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO client (nomclient, prenomclient, mailclient) VALUES (@nomclient, @prenomclient, @mailclient) RETURNING numclient;");
                    cmd.Parameters.AddWithValue("@nomclient", client.NomClient);
                    cmd.Parameters.AddWithValue("@prenomclient", client.PrenomClient);
                    cmd.Parameters.AddWithValue("@mailclient", client.MailClient);
                    int num = DataAccess.Instance.ExecuteInsert(cmd);
                    client.NumClient = num;
                    MessageBox.Show("Client ajouté avec succès !");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de l'ajout : " + ex.Message);
                    return;
                }
            }
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.DialogResult = true;
            }
        }
    }
}
