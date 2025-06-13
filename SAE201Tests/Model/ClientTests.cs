using Microsoft.VisualStudio.TestTools.UnitTesting;
using SAE201.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE201.Tests
{
    [TestClass]
    public class ClientTests
    {
        [TestMethod]
        public void NomClient_AvecNomComposeEnMinuscules()
        {
            Client client = new Client();
            string nomInitial = "jean-michel de la roche";
            string nomAttendu = "Jean-Michel De La Roche";

            client.NomClient = nomInitial;

            Assert.AreEqual(nomAttendu, client.NomClient, "Le nom du client devrait être correctement formaté.");
        }

        [TestMethod]
        public void MailClient_AvecEmailGmailValide()
        {
            Client client = new Client();
            string emailValide = "jules.cadoux74@gmail.com";

            client.MailClient = emailValide;

            Assert.AreEqual(emailValide, client.MailClient, "Une adresse email valide de type gmail devrait être acceptée.");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException), "Une adresse email avec un domaine non autorisé devrait lever une exception.")]
        public void MailClient_AvecDomaineNonAutorise()
        {
            Client client = new Client();
            string emailInvalide = "contact@mon-entreprise.fr";

            client.MailClient = emailInvalide;
        }

        [TestMethod]
        public void PrenomClient_AvecAccentsEtMinuscules()
        {
            Client client = new Client();
            string prenomInitial = "émilie-noëlle";
            string prenomAttendu = "Émilie-Noëlle";

            client.PrenomClient = prenomInitial;

            Assert.AreEqual(prenomAttendu, client.PrenomClient, "Le prénom avec accents devrait être correctement formaté.");
        }

        [TestMethod]
        public void MailClient_AvecEmailEmailComValide()
        {
            Client client = new Client();
            string emailValide = "utilisateur.test@email.com";

            client.MailClient = emailValide;

            Assert.AreEqual(emailValide, client.MailClient, "Une adresse email valide de type email.com devrait être acceptée.");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException), "Une adresse email sans le symbole '@' devrait lever une exception.")]
        public void MailClient_SansArobase_DoitLeverUneException()
        {
            Client client = new Client();
            string emailInvalide = "test.gmail.com";

            client.MailClient = emailInvalide;
        }
    }
}