using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace Sae_2._01
{
    public class ApplicationData
    {

            private ObservableCollection<Client> lesClients;
            private ObservableCollection<Reservation> lesReservations;
            private ObservableCollection<Vehicule> lesVehicules;
            private ObservableCollection<Vehicule> lesVehiculesAjoutes = new ObservableCollection<Vehicule>();
            private ObservableCollection<Assurance> lesAssurances;
            private NpgsqlConnection connexion = null;   // futur lien à la BD

            public ObservableCollection<Client> LesClients
            {
                get
                {
                    return this.lesClients;
                }

                set
                {
                    this.lesClients = value;
                }
            }

            public ObservableCollection<Reservation> LesReservations
            {
                get
                {
                    return this.lesReservations;
                }

                set
                {
                    this.lesReservations = value;
                }
            }

            public ObservableCollection<Vehicule> LesVehicules
            {
                get
                {
                    return this.lesVehicules;
                }

                set
                {
                    this.lesVehicules = value;
                }
            }

            public ObservableCollection<Vehicule> LesVehiculesAjoutes
            {
                get
                {
                    return this.lesVehiculesAjoutes;
                }

                set
                {
                    this.lesVehiculesAjoutes = value;
                }
            }

            public ObservableCollection<Assurance> LesAssurances
            {
                get
                {
                    return this.lesAssurances;
                }

                set
                {
                    this.lesAssurances = value;
                }
            }

        public NpgsqlConnection Connexion
            {
                get
                {
                    return this.connexion;
                }

                set
                {
                    this.connexion = value;
                }
            }

       

        public ApplicationData()
            {

                this.ConnexionBD("lacrclem", "AmnezZ");
                this.ReadClient();
                this.ReadReservation();
                this.ReadVehicule();
                this.ReadAssurance();
            }
            public bool ConnexionBD(string login, string mdp)
            {
            try
            {
                Connexion = new NpgsqlConnection();
                Connexion.ConnectionString = "Server=srv-peda-new;" +
                                             "port=5433;" +
                                             "Database=saedev;" +
                                             "Search Path = saedev;" +
                                             "uid=" + login +";" +
                                             "password="+mdp+";";
                // à compléter dans les "" 
                // @ sert à enlever tout pb avec les caractères 
                Connexion.Open();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("pb de connexion : " + e);
                // juste pour le debug : à transformer en MsgBox 
                return false;
            }
        }

        public int ReadAssurance()
        {
            this.LesAssurances = new ObservableCollection<Assurance>();
            String sql = "SELECT * FROM Assurance";
            try
            {
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(sql, Connexion);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                foreach (DataRow res in dataTable.Rows)
                {
                    int num_assurance = int.Parse(res["num_assurance"].ToString());
                    string description_assurance = res["description_assurance"].ToString();
                    int prix_assurance = int.Parse(res["prix_assurance"].ToString());

                    Assurance nouveau = new Assurance(num_assurance,description_assurance,prix_assurance);

                    lesAssurances.Add(nouveau);
                }
                return dataTable.Rows.Count;
            }
            catch (NpgsqlException e)
            { Console.WriteLine("pb de requete : " + e); return 0; }
        }

        public int ReadClient()
        {
            this.LesClients = new ObservableCollection<Client>();
            String sql = "SELECT * FROM Client";
            try
            {
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(sql, Connexion);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                foreach (DataRow res in dataTable.Rows)
                {
                    Client nouveau = new Client(int.Parse(res["num_client"].ToString()),
                    res["nom_client"].ToString(), res["adresse_rue_client"].ToString(), 
                    res["adresse_cp_client"].ToString(), res["adresse_ville_client"].ToString(),
                    res["telephone_client"].ToString(),
                    res["mail_client"].ToString());
                    LesClients.Add(nouveau);
                }
                return dataTable.Rows.Count;
            }
            catch (NpgsqlException e)
            { Console.WriteLine("pb de requete : " + e); return 0; }
        }



        public int Createclient(Client c)
        {
            String sql = $"insert into client (num_client,nom_client,adresse_rue_client,adresse_cp_client,adresse_ville_client,telephone_client, mail_client)"
            + $" values ({LesClients.Count+1},'{c.Nom_client}','{c.Adresse_rue_client}','{c.Adresse_cp_client}'"
            + $",'{c.Adresse_ville_client}','{c.Telephone_client}', "
            + $"'{c.Mail_client}'); ";
            try
            {
                int nb;
                NpgsqlCommand cmd = new NpgsqlCommand(sql, Connexion);
                nb = cmd.ExecuteNonQuery();
                return nb;
                //nb permet de connaître le nb de lignes affectées par un insert, update, delete
            }
            catch (Exception sqlE)
            {
                Console.WriteLine("pb de requete : " + sql + "" + sqlE);
                // juste pour le debug : à transformer en MsgBox 
                return 0;
            }
        }

        public int Createreservation(Reservation r)
        {
            String sql = $"insert into reservation (num_reservation,num_assurance,num_client,date_reservation,date_debut_reservation,date_fin_reservation, montant_reservation,forfait_km)"
            + $" values ({r.Num_reservation},{r.Une_assurance.Num_assurance},{r.Un_client.Num_client},'{r.Date_reservation}'"
            + $",'{r.Date_debut_reservation}','{r.Date_fin_reservation}', "
            + $"'{r.Montant_reservation}','{r.Forfait_km}'); ";
            try
            {
                int nb;
                NpgsqlCommand cmd = new NpgsqlCommand(sql, Connexion);
                nb = cmd.ExecuteNonQuery();

                foreach (Vehicule vec in r.Vehicules)
                {
                    String sql2 = $"insert into detail_reservation(immatriculation,num_reservation) values ('{vec.Immatriculation}',{r.Num_reservation})";
                    int nb1;
                    NpgsqlCommand cmd2 = new NpgsqlCommand(sql2, Connexion);
                    nb1 = cmd2.ExecuteNonQuery();

                }

                return nb;
                //nb permet de connaître le nb de lignes affectées par un insert, update, delete
            }
            catch (Exception sqlE)
            {
                Console.WriteLine("pb de requete : " + sql + "" + sqlE);
                // juste pour le debug : à transformer en MsgBox 
                return 0;
            }
        }


        public int ReadReservation()
        {
            this.LesReservations = new ObservableCollection<Reservation>();
            String sql = "SELECT r.*, a.*, c.* FROM Reservation r join Assurance a on r.num_assurance = a.num_assurance join Client c on r.num_client = c.num_client";
            string sql2 = "SELECT * FROM reservation";
            try
            {
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(sql, Connexion);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                List<List<Vehicule>> list_nv1 = new List<List<Vehicule>>();

                NpgsqlDataAdapter dataAdapter2 = new NpgsqlDataAdapter(sql2, Connexion);
                DataTable dataTable2 = new DataTable();
                dataAdapter2.Fill(dataTable2);


                foreach (DataRow res2 in dataTable2.Rows)
                {
                    int num_reservation = int.Parse(res2["num_reservation"].ToString());
                    List<Vehicule> list_ = new List<Vehicule>();
                    string sql3 = "select v.* from vehicule v join detail_reservation d on v.immatriculation = d.immatriculation join reservation r on d.num_reservation = r.num_reservation  where r.num_reservation = " + num_reservation;
                    NpgsqlDataAdapter dataAdapter3 = new NpgsqlDataAdapter(sql3, Connexion);
                    DataTable dataTable3 = new DataTable();
                    dataAdapter3.Fill(dataTable3);
                    foreach (DataRow res3 in dataTable3.Rows)
                    {
                        String sql4 = "SELECT v.*,m.*,c.* FROM vehicule v join magasin m on v.num_magasin = m.num_magasin join categorie_vehicule c on v.nom_categorie = c.nom_categorie";
                        string sql5 = "SELECT * FROM vehicule";
                        try
                        {
                            NpgsqlDataAdapter dataAdapte4 = new NpgsqlDataAdapter(sql4, Connexion);
                            DataTable dataTable4 = new DataTable();
                            dataAdapte4.Fill(dataTable4);

                            List<List<Caracteristique>> list_nv2 = new List<List<Caracteristique>>();

                            NpgsqlDataAdapter dataAdapter5 = new NpgsqlDataAdapter(sql5, Connexion);
                            DataTable dataTable5 = new DataTable();
                            dataAdapter5.Fill(dataTable5);


                            foreach (DataRow res5 in dataTable5.Rows)
                            {
                                string immatriculation = res5["immatriculation"].ToString();
                                List<Caracteristique> list_2 = new List<Caracteristique>();
                                string sql6 = "select c.* from caracteristique c join detail_caracteristique d on c.num_caracteristique = d.num_caracteristique join vehicule v on d.immatriculation = v.immatriculation  where v.immatriculation = '" + immatriculation + "'";
                                NpgsqlDataAdapter dataAdapter6 = new NpgsqlDataAdapter(sql6, Connexion);
                                DataTable dataTable6 = new DataTable();
                                dataAdapter6.Fill(dataTable6);
                                foreach (DataRow res6 in dataTable6.Rows)
                                {
                                    int num_caracteristique = int.Parse(res6["num_caracteristique"].ToString());
                                    string nom_caracteristqiue = res6["nom_caracteristique"].ToString();
                                    Caracteristique car = new Caracteristique(num_caracteristique, nom_caracteristqiue);
                                    list_2.Add(car);

                                }
                                list_nv2.Add(list_2);



                            }
                            int compteur2 = 0;

                            foreach (DataRow res4 in dataTable4.Rows)
                            {
                                string immatriculation = res4["immatriculation"].ToString();
                                string type_boite = res4["type_boite"].ToString();
                                string nom_vehicule = res4["nom_vehicule"].ToString();
                                string description_vehicule = res4["description_vehicule"].ToString();
                                int nombre_places = int.Parse(res4["nombre_places"].ToString());
                                double prix_location = double.Parse(res4["prix_location"].ToString());
                                bool climatisation = bool.Parse(res4["climatisation"].ToString());
                                string lien_photo_url = res4["lien_photo_url"].ToString();
                                int num_magasin = int.Parse(res4["num_magasin"].ToString());
                                string nom_magasin = res4["nom_magasin"].ToString();
                                string adresse_rue_magasin = res4["adresse_rue_magasin"].ToString();
                                string adresse_cp_magasin = res4["adresse_cp_magasin"].ToString();
                                string adresse_ville_magasin = res4["adresse_ville_magasin"].ToString();
                                string horaire_magasin = res4["horaire_magasin"].ToString();
                                string nom_categorie = res4["nom_categorie"].ToString();


                                Vehicule vec = new Vehicule(immatriculation, type_boite, nom_vehicule, description_vehicule, nombre_places, prix_location, climatisation, lien_photo_url, num_magasin, nom_magasin, adresse_rue_magasin, adresse_cp_magasin, adresse_ville_magasin, horaire_magasin, nom_categorie, list_nv2[compteur2]);
                                list_.Add(vec);
                            }
                        }
                        catch (NpgsqlException e)
                        { Console.WriteLine("pb de requete : " + e); return 0; }

                    }
                    list_nv1.Add(list_);
                }

                int compteur = 0;

                foreach (DataRow res in dataTable.Rows)
                {
                    int num_reservation = int.Parse(res["num_reservation"].ToString());
                    DateTime date_reservation = DateTime.Parse(res["date_reservation"].ToString());
                    DateTime date_debut_reservation = DateTime.Parse(res["date_debut_reservation"].ToString());
                    DateTime date_fin_reservation = DateTime.Parse(res["date_fin_reservation"].ToString());
                    double montant_reservation = double.Parse(res["montant_reservation"].ToString());
                    string forfait_km = res["forfait_km"].ToString();
                    int num_assurance = int.Parse(res["num_assurance"].ToString());
                    string description_assurance = res["description_assurance"].ToString();
                    int prix_assurance = int.Parse(res["prix_assurance"].ToString());
                    int num_client = int.Parse(res["num_client"].ToString());
                    string nom_client = res["nom_client"].ToString();
                    string telephone_client = res["telephone_client"].ToString();
                    string mail_client = res["mail_client"].ToString();

                    Reservation nouveau = new Reservation(num_reservation, date_reservation, date_debut_reservation, date_fin_reservation, montant_reservation, forfait_km, num_assurance, description_assurance, prix_assurance, num_client, nom_client, telephone_client, mail_client, list_nv1[compteur]);
                    LesReservations.Add(nouveau);

                    compteur++;
                }
                
                return dataTable.Rows.Count;
                
            }
            catch (NpgsqlException e)
            { Console.WriteLine("pb de requete : " + e); return 0; }
        }

        public int ReadVehicule()
        {
            this.LesVehicules = new ObservableCollection<Vehicule>();
            String sql = "SELECT v.*,m.*,c.* FROM vehicule v join magasin m on v.num_magasin = m.num_magasin join categorie_vehicule c on v.nom_categorie = c.nom_categorie";
            string sql2 = "SELECT * FROM vehicule";
            try
            {
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(sql, Connexion);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                List<List<Caracteristique>> list_nv1 = new List<List<Caracteristique>>();

                NpgsqlDataAdapter dataAdapter2 = new NpgsqlDataAdapter(sql2, Connexion);
                DataTable dataTable2 = new DataTable();
                dataAdapter2.Fill(dataTable2);


                foreach (DataRow res2 in dataTable2.Rows)
                {
                    string immatriculation = res2["immatriculation"].ToString();
                    List<Caracteristique> list_ = new List<Caracteristique>();
                    string sql3 = "select c.* from caracteristique c join detail_caracteristique d on c.num_caracteristique = d.num_caracteristique join vehicule v on d.immatriculation = v.immatriculation  where v.immatriculation = '" + immatriculation + "'";
                    NpgsqlDataAdapter dataAdapter3 = new NpgsqlDataAdapter(sql3, Connexion);
                    DataTable dataTable3 = new DataTable();
                    dataAdapter3.Fill(dataTable3);  
                    foreach (DataRow res3 in dataTable3.Rows)
                    {
                        int num_caracteristique = int.Parse(res3["num_caracteristique"].ToString());
                        string nom_caracteristqiue = res3["nom_caracteristique"].ToString();
                        Caracteristique car = new Caracteristique(num_caracteristique, nom_caracteristqiue);
                        list_.Add(car);
                        
                    }
                    list_nv1.Add(list_);
                    
                    

                }
                int compteur = 0;

                foreach (DataRow res in dataTable.Rows)
                {
                    string immatriculation = res["immatriculation"].ToString();
                    string type_boite = res["type_boite"].ToString();
                    string nom_vehicule = res["nom_vehicule"].ToString();
                    string description_vehicule = res["description_vehicule"].ToString();
                    int nombre_places = int.Parse(res["nombre_places"].ToString());
                    double prix_location = double.Parse(res["prix_location"].ToString());
                    bool climatisation = bool.Parse(res["climatisation"].ToString());
                    string lien_photo_url =  res["lien_photo_url"].ToString();
                    int num_magasin = int.Parse(res["num_magasin"].ToString());
                    string nom_magasin = res["nom_magasin"].ToString();
                    string adresse_rue_magasin = res["adresse_rue_magasin"].ToString();
                    string adresse_cp_magasin = res["adresse_cp_magasin"].ToString();
                    string adresse_ville_magasin = res["adresse_ville_magasin"].ToString();
                    string horaire_magasin = res["horaire_magasin"].ToString();
                    string nom_categorie = res["nom_categorie"].ToString();


                    Vehicule nouveau = new Vehicule(immatriculation, type_boite,  nom_vehicule, description_vehicule, nombre_places, prix_location, climatisation, lien_photo_url, num_magasin,nom_magasin, adresse_rue_magasin, adresse_cp_magasin,adresse_ville_magasin,horaire_magasin, nom_categorie, list_nv1[compteur]);


                    LesVehicules.Add(nouveau);

                    compteur += 1;
                    
                }
                
                return dataTable.Rows.Count;
            }
            catch (NpgsqlException e)
            { Console.WriteLine("pb de requete : " + e); return 0; }
        }




    }
}
