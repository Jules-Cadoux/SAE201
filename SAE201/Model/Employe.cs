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
        private Role numRole;
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

        public Employe(int numEmploye, Role numRole, string nom, string prenom, string login, string mdp)
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

        public Role NumRole
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
            using (NpgsqlCommand cmd = new NpgsqlCommand(
                "INSERT INTO employe (numrole, nom, prenom, login, mdp) VALUES (@numrole, @nom, @prenom, @login, @mdp) RETURNING numemploye"))
            {
                cmd.Parameters.AddWithValue("numrole", this.NumRole.NumRole);
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
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM employe WHERE numemploye = @numemploye"))
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
                    this.NumRole = new Role { NumRole = idRole }; 
                }
            }
        }

        public int Update()
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand(
                "UPDATE employe SET numrole = @numrole, nom = @nom, prenom = @prenom, login = @login, mdp = @mdp WHERE numemploye = @numemploye"))
            {
                cmd.Parameters.AddWithValue("numrole", this.NumRole.NumRole);
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
            using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM employe WHERE numemploye = @numemploye"))
            {
                cmd.Parameters.AddWithValue("numemploye", this.NumEmploye);
                return DataAccess.Instance.ExecuteSet(cmd);
            }
        }

        public static List<Employe> FindAll()
        {
            List<Employe> employes = new List<Employe>();
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM employe"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);
                foreach (DataRow row in dt.Rows)
                {
                    Employe emp = new Employe
                    {
                        NumEmploye = (int)row["numemploye"],
                        Nom = (string)row["nom"],
                        Prenom = (string)row["prenom"],
                        Login = (string)row["login"],
                        Mdp = (string)row["mdp"],
                        NumRole = new Role { NumRole = (int)row["numrole"] }
                    };
                    employes.Add(emp);
                }
            }
            //return employes;


            List<Employe> lesEmployes = new List<Employe>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from employe ;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                    lesEmployes.Add(new Employe((Int32)dr["numemploye"], (Role)dr["numrole"], (string)dr["nom"],
                   (string)dr["prenom"], (string)dr["login"], (string)dr["mdp"]));
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
    }
}
