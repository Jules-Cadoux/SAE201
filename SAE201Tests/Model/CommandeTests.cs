using SAE201.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE201.Tests
{
    [TestClass]
    public class CommandeTests
    {
        [TestMethod]
        public void Constructeur_AvecParametres_InitialiseCorrectement()
        {
            int numCommande = 1;
            int numEmploye = 101;
            DateTime dateCommande = new DateTime(2025, 06, 15);
            bool valider = true;
            double prixTotal = 250.50;

            Commande commande = new Commande(numCommande, numEmploye, dateCommande, valider, prixTotal);

            Assert.AreEqual(numCommande, commande.NumCommande);
            Assert.AreEqual(numEmploye, commande.NumEmploye);
            Assert.AreEqual(dateCommande, commande.DateCommande);
            Assert.AreEqual(valider, commande.Valider);
            Assert.AreEqual(prixTotal, commande.PrixTotal);
        }

        [TestMethod]
        public void MethodeEquals_AvecMemeNumCommande_RetourneTrue()
        {
            Commande commande1 = new Commande { NumCommande = 5 };
            Commande commande2 = new Commande { NumCommande = 5 };

            bool sontEgales = commande1.Equals(commande2);

            Assert.IsTrue(sontEgales, "Deux commandes avec le même NumCommande devraient être considérées comme égales.");
        }

        [TestMethod]
        public void MethodeEquals_AvecNumCommandeDifferent_RetourneFalse()
        {
            Commande commande1 = new Commande { NumCommande = 10 };
            Commande commande2 = new Commande { NumCommande = 12 };

            bool sontEgales = commande1.Equals(commande2);

            Assert.IsFalse(sontEgales, "Deux commandes avec des NumCommande différents ne devraient pas être égales.");
        }
    }
}
