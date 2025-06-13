using SAE201.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE201.Tests
{
    [TestClass]
    public class DemandeTests
    {
        [TestMethod]
        public void PrixLigne_CalculSimple_RetourneLeBonTotal()
        {
            Vin vinDeTest = new Vin();
            vinDeTest.PrixVin = 15.50;
            Demande demande = new Demande
            {
                NumVin = vinDeTest,
                QuantiteDemande = 4
            };
            double prixLigneAttendu = 62.0;

            double prixLigneCalcule = demande.PrixLigne;

            Assert.AreEqual(prixLigneAttendu, prixLigneCalcule);
        }

        [TestMethod]
        public void PrixLignAvecQuantiteNulleRetourneZero()
        {
            Vin vinDeTest = new Vin();
            vinDeTest.PrixVin = 25.0;
            Demande demande = new Demande
            {
                NumVin = vinDeTest,
                QuantiteDemande = 0
            };
            double prixLigneAttendu = 0.0;

            double prixLigneCalcule = demande.PrixLigne;

            Assert.AreEqual(prixLigneAttendu, prixLigneCalcule);
        }

        [TestMethod]
        public void AccepteAvecStatutValide()
        {
            Demande demande = new Demande();
            string statutAttendu = "Accepter";

            demande.Accepter = "En Attente"; 
            demande.Accepter = statutAttendu; 

            Assert.AreEqual(statutAttendu, demande.Accepter);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AccepterAvecStatutInvalide()
        {
            Demande demande = new Demande();
            string statutInvalide = "Peut-être";

            demande.Accepter = statutInvalide;
        }
    }
}
