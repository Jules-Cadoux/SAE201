
namespace SAE201.test
{ 
        [TestClass]
        public class ClientTests
        {
            [TestMethod]
            public void NomClient_AvecNomComposeEnMinuscules_DoitEtreConvertiEnCasseTitre()
            {
                Client client = new Client();
                string nomInitial = "jean-michel de la roche";
                string nomAttendu = "Jean-Michel De La Roche";

                // Action
                client.NomClient = nomInitial;

                // Vérification
                Assert.AreEqual(nomAttendu, client.NomClient, "Le nom du client devrait être correctement formaté.");
            }

            [TestMethod]
            public void MailClient_AvecEmailGmailValide_DoitEtreAccepte()
            {
                // Préparation
                Client client = new Client();
                string emailValide = "jules.cadoux74@gmail.com";

                // Action
                client.MailClient = emailValide;

                // Vérification
                Assert.AreEqual(emailValide, client.MailClient, "Une adresse email valide de type gmail devrait être acceptée.");
            }

            [TestMethod]
            [ExpectedException(typeof(FormatException), "Une adresse email avec un domaine non autorisé devrait lever une exception.")]
            public void MailClient_AvecDomaineNonAutorise_DoitLeverUneException()
            {
                // Préparation
                Client client = new Client();
                string emailInvalide = "contact@mon-entreprise.fr";

                // Action
                client.MailClient = emailInvalide; // Cette action doit lever une FormatException.
            }
        }
}