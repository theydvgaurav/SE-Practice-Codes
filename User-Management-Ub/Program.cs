using System;
using System.Data;
using System.Data.SqlClient;

namespace User_Management
{
    class Db_Connection
    {
        private string strConnectionConfig = @"Data Source=localhost;Initial Catalog=MyDB;User ID=sa;Password=Gaurav@007";
        static private Db_Connection Instance = new Db_Connection();
        private Db_Connection()
        {

        }

        public static Db_Connection GetInstance
        {
            get
            {
                return Instance;

            }
        }

        public SqlConnection getDBConnection()
        {
            SqlConnection connection = new SqlConnection(strConnectionConfig);
            return connection;

        }
    }
    class Program
    {
        public void CreateEmployee()
        {
            Db_Connection MyObj = Db_Connection.GetInstance;
            SqlConnection connect = MyObj.getDBConnection();
            connect.Open();
            Console.WriteLine("Connection Open");

            try
            {
                SqlDataReader dreader;
                SqlCommand cmd = new SqlCommand("INSERT INTO Projects(NAME, OWNER) VALUES(" +
                           "@name,@owner)", connect);

                cmd.Parameters.AddWithValue("@Name", "Ganga");
                cmd.Parameters.AddWithValue("@Owner", "Anuj Sharma");

                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("SELECT MAX(ID) AS ID FROM Projects", connect);
                dreader = cmd.ExecuteReader();

                string ProjectID = "";

                while (dreader.Read())
                {
                    ProjectID = ProjectID + dreader.GetValue(0);
                }

                dreader.Close();

                cmd = new SqlCommand("INSERT INTO Department(NAME, DEPT_HEAD) VALUES(" +
                           "@name,@head)", connect);

                cmd.Parameters.AddWithValue("@Name", "Engineering");
                cmd.Parameters.AddWithValue("@Head", "Gaurav Yadav");

                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("SELECT MAX(ID) AS ID FROM Department", connect);
                dreader = cmd.ExecuteReader();

                string DepartmentID = "";

                while (dreader.Read())
                {
                    DepartmentID = DepartmentID + dreader.GetValue(0);
                }

                dreader.Close();

                cmd = new SqlCommand("INSERT INTO Employee(NAME, OFFICE, PROJECT_ID, DEPT_ID) VALUES(" +
                                           "@name,@office,@project,@dept)", connect);

                cmd.Parameters.AddWithValue("@Name", "Deepak Kumar");
                cmd.Parameters.AddWithValue("@Office", "Gurugram");
                cmd.Parameters.AddWithValue("@Project", int.Parse(ProjectID));
                cmd.Parameters.AddWithValue("@Dept", int.Parse(DepartmentID));

                cmd.ExecuteNonQuery();


            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex);
            }

            connect.Close();
            Console.WriteLine("Connection Terminated");
        }

        public void GetAllEmployees()
        {
            Db_Connection MyObj = Db_Connection.GetInstance;
            SqlConnection connect = MyObj.getDBConnection();
            connect.Open();
            Console.WriteLine("Connection Open");

            SqlDataReader dreader;

            SqlCommand cmd = new SqlCommand("select Employee.NAME, Employee.OFFICE, Projects.NAME, Projects.OWNER, Department.NAME, Department.DEPT_HEAD from Employee inner join Projects on Employee.PROJECT_ID = Projects.ID inner join Department on Employee.DEPT_ID = Department.ID ", connect);

            dreader = cmd.ExecuteReader();

            string output = "";

            while (dreader.Read())
            {
                output = output + dreader.GetValue(0) + " - " +
                                    dreader.GetValue(1) + " - " +
                                    dreader.GetValue(2) + " - " +
                                    // dreader.GetValue(3) + " - " +
                                    dreader.GetValue(4)
                                    // + " - " +
                                    // dreader.GetValue(5)
                                    + "\n";
            }

            dreader.Close();

            Console.Write(output);
            connect.Close();
            Console.WriteLine("Connection Terminated");
        }
        static void Main(string[] args)
        {
            Program MyObj = new Program();
            MyObj.GetAllEmployees();

        }
    }
}