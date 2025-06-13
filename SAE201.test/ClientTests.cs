
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

                // V�rification
                Assert.AreEqual(nomAttendu, client.NomClient, "Le nom du client devrait �tre correctement format�.");
            }

            [TestMethod]
            public void MailClient_AvecEmailGmailValide_DoitEtreAccepte()
            {
                // Pr�paration
                Client client = new Client();
                string emailValide = "jules.cadoux74@gmail.com";

                // Action
                client.MailClient = emailValide;

                // V�rification
                Assert.AreEqual(emailValide, client.MailClient, "Une adresse email valide de type gmail devrait �tre accept�e.");
            }

            [TestMethod]
            [ExpectedException(typeof(FormatException), "Une adresse email avec un domaine non autoris� devrait lever une exception.")]
            public void MailClient_AvecDomaineNonAutorise_DoitLeverUneException()
            {
                // Pr�paration
                Client client = new Client();
                string emailInvalide = "contact@mon-entreprise.fr";

                // Action
                client.MailClient = emailInvalide; // Cette action doit lever une FormatException.
            }
        }
}