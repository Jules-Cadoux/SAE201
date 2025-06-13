using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE201.Model
{
    public class Employe
    {
        private int numEmploye;
        private int numRole;
        private string nom;
        private string prenom;
        private string login;
        private string mdp;

        public Employe()
        {
        }

        public Employe(int numEmploye)
        {
            this.NumEmploye = numEmploye;
        }

        public Employe(int numEmploye, int numRole, string nom, string prenom, string login, string mdp)
        {
            this.NumEmploye = numEmploye;
            this.NumRole = numRole;
            this.Nom = nom;
            this.Prenom = prenom;
            this.Login = login;
            this.Mdp = mdp;
        }

        public int NumEmploye
        {
            get
            {
                return this.numEmploye;
            }

            set
            {
                this.numEmploye = value;
            }
        }

        public int NumRole
        {
            get
            {
                return this.numRole;
            }

            set
            {
                this.numRole = value;
            }
        }

        public string Nom
        {
            get
            {
                return nom;
            }

            set
            {
                nom = value;
            }
        }

        public string Prenom
        {
            get
            {
                return prenom;
            }

            set
            {
                prenom = value;
            }
        }

        public string Login
        {
            get
            {
                return login;
            }

            set
            {
                login = value;
            }
        }

        public string Mdp
        {
            get
            {
                return this.mdp;
            }

            set
            {
                this.mdp = value;
            }
        }

        public int Create()
        {
            int nb = 0;
            using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO sae201_nicolas.employe (numrole, nom, prenom, login, mdp) VALUES (@numrole, @nom, @prenom, @login, @mdp) RETURNING numemploye"))
            {
                cmd.Parameters.AddWithValue("numrole", this.NumRole);
                cmd.Parameters.AddWithValue("nom", this.Nom);
                cmd.Parameters.AddWithValue("prenom", this.Prenom);
                cmd.Parameters.AddWithValue("login", this.Login);
                cmd.Parameters.AddWithValue("mdp", this.Mdp);

                nb = DataAccess.Instance.ExecuteInsert(cmd);
                this.NumEmploye = nb;
            }
            return nb;
        }

        public void Read()
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM sae201_nicolas.employe WHERE numemploye = @numemploye"))
            {
                cmd.Parameters.AddWithValue("numemploye", this.NumEmploye);
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    this.Nom = (string)row["nom"];
                    this.Prenom = (string)row["prenom"];
                    this.Login = (string)row["login"];
                    this.Mdp = (string)row["mdp"];

                    int idRole = (int)row["numrole"];
                    this.NumRole = idRole ; 
                }
            }
        }

        public int Update()
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE sae201_nicolas.employe SET numrole = @numrole, nom = @nom, prenom = @prenom, login = @login, mdp = @mdp WHERE numemploye = @numemploye"))
            {
                cmd.Parameters.AddWithValue("numrole", this.NumRole);
                cmd.Parameters.AddWithValue("nom", this.Nom);
                cmd.Parameters.AddWithValue("prenom", this.Prenom);
                cmd.Parameters.AddWithValue("login", this.Login);
                cmd.Parameters.AddWithValue("mdp", this.Mdp);
                cmd.Parameters.AddWithValue("numemploye", this.NumEmploye);

                return DataAccess.Instance.ExecuteSet(cmd);
            }
        }

        public int Delete()
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM sae201_nicolas.employe WHERE numemploye = @numemploye"))
            {
                cmd.Parameters.AddWithValue("numemploye", this.NumEmploye);
                return DataAccess.Instance.ExecuteSet(cmd);
            }
        }

        public List<Employe> FindAll()
        {
            List<Employe> lesEmployes = new List<Employe>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("SELECT * FROM sae201_nicolas.employe"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    int numEmploye = (int)dr["numemploye"];
                    int numRole = (int)dr["numrole"];
                    string nom = (string)dr["nom"];
                    string prenom = (string)dr["prenom"];
                    string login = (string)dr["login"];
                    string mdp = (string)dr["mdp"];

                    int role = numRole ;

                    lesEmployes.Add(new Employe(numEmploye, role, nom, prenom, login, mdp));
                }
            }
            return lesEmployes;
        }

        public static List<Employe> FindBySelection(string criteres)
        {
            throw new NotImplementedException();
        }


        public override bool Equals(object? obj)
        {
            return obj is Employe employe &&
                   this.NumEmploye == employe.NumEmploye;
        }

        public static Employe FindByLoginAndPassword(string login, string password)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM sae201_nicolas.employe WHERE login = @login AND mdp = @mdp"))
            {
                cmd.Parameters.AddWithValue("login", login);
                cmd.Parameters.AddWithValue("mdp", password);

                DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    int numEmploye = (int)row["numemploye"];
                    int numRole = (int)row["numrole"];
                    string nom = (string)row["nom"];
                    string prenom = (string)row["prenom"];
                    string loginEmp = (string)row["login"];
                    string mdp = (string)row["mdp"];

                    int role = numRole ;

                    return new Employe(numEmploye, role, nom, prenom, loginEmp, mdp);
                }
                return null;
            }
        }
    }
}
