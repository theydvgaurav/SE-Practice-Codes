using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;

namespace User_Management
{
    class Program
    {
        private static string strConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString; }
        }

        public void CreateEmployee()
        {
            
            try
            {
                SqlConnection connect = new SqlConnection(strConnectionString);
                connect.Open();
                string strEmpName,
                    strOffice,
                    strProjectName,
                    strProjectOwner,
                    strDeptName,
                    strDeptHead;

                Console.WriteLine("Enter Employee Name:");
                strEmpName = Console.ReadLine();

                Console.WriteLine("Enter Employee's Office Location:");
                strOffice = Console.ReadLine();

                Console.WriteLine("Enter Employee's Project Name:");
                strProjectName = Console.ReadLine();

                Console.WriteLine("Enter Project Owner's Name:");
                strProjectOwner = Console.ReadLine();

                Console.WriteLine("Enter Employee's Department Name:");
                strDeptName = Console.ReadLine();

                Console.WriteLine("Enter Department Head's Name:");
                strDeptHead = Console.ReadLine();

                SqlDataReader dreader;
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Projects(NAME, OWNER) VALUES(" + "@name,@owner)",
                    connect
                );
                cmd.Parameters.AddWithValue("@Name", strProjectName);
                cmd.Parameters.AddWithValue("@Owner", strProjectOwner);

                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("SELECT MAX(ID) AS ID FROM Projects", connect);
                dreader = cmd.ExecuteReader();

                string ProjectID = "";

                while (dreader.Read())
                {
                    ProjectID = ProjectID + dreader.GetValue(0);
                }

                dreader.Close();

                cmd = new SqlCommand(
                    "INSERT INTO Department(NAME, DEPT_HEAD) VALUES(" + "@name,@head)",
                    connect
                );
                cmd.Parameters.AddWithValue("@Name", strDeptName);
                cmd.Parameters.AddWithValue("@Head", strDeptHead);

                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("SELECT MAX(ID) AS ID FROM Department", connect);
                dreader = cmd.ExecuteReader();

                string DepartmentID = "";

                while (dreader.Read())
                {
                    DepartmentID = DepartmentID + dreader.GetValue(0);
                }

                dreader.Close();

                cmd = new SqlCommand(
                    "INSERT INTO Employee(NAME, OFFICE, PROJECT_ID, DEPT_ID) VALUES("
                        + "@name,@office,@project,@dept)",
                    connect
                );
                cmd.Parameters.AddWithValue("@Name", strEmpName);
                cmd.Parameters.AddWithValue("@Office", strOffice);
                cmd.Parameters.AddWithValue("@Project", int.Parse(ProjectID));
                cmd.Parameters.AddWithValue("@Dept", int.Parse(DepartmentID));

                cmd.ExecuteNonQuery();
                connect.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void GetAllEmployees()
        {
            
            try
            {
                SqlConnection connect = new SqlConnection(strConnectionString);
                connect.Open();

                SqlDataReader dreader;

                SqlCommand cmd = new SqlCommand(
                    "select Employee.ID, Employee.NAME, Employee.OFFICE, Projects.NAME, Projects.OWNER, Department.NAME, Department.DEPT_HEAD from Employee inner join Projects on Employee.PROJECT_ID = Projects.ID inner join Department on Employee.DEPT_ID = Department.ID",
                    connect
                );

                dreader = cmd.ExecuteReader();

                string output =
                    "ID - Name - Office - Project - Project Owner - Department - Department Head \n";

                while (dreader.Read())
                {
                    output =
                        output
                        + dreader.GetValue(0)
                        + " - "
                        + dreader.GetValue(1)
                        + " - "
                        + dreader.GetValue(2)
                        + " - "
                        + dreader.GetValue(3)
                        + " - "
                        + dreader.GetValue(4)
                        + " - "
                        + dreader.GetValue(5)
                        + " - "
                        + dreader.GetValue(6)
                        + "\n";
                }

                dreader.Close();

                Console.Write(output);
                connect.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex);
            }

            
        }

        public void GetEmployee()
        {
            
            try
            {
                SqlConnection connect = new SqlConnection(strConnectionString);
                connect.Open();

                string strEmpID;

                Console.WriteLine("Enter Employee's ID:");
                strEmpID = Console.ReadLine();

                SqlDataReader dreader;

                SqlCommand cmd = new SqlCommand(
                    "select Employee.ID, Employee.NAME, Employee.OFFICE, Projects.NAME, Projects.OWNER, Department.NAME, Department.DEPT_HEAD from Employee inner join Projects on Employee.PROJECT_ID = Projects.ID inner join Department on Employee.DEPT_ID = Department.ID where Employee.ID = "
                        + strEmpID,
                    connect
                );

                dreader = cmd.ExecuteReader();

                string output =
                    "ID - Name - Office - Project - Project Owner - Department - Department Head \n";

                while (dreader.Read())
                {
                    output =
                        output
                        + dreader.GetValue(0)
                        + " - "
                        + dreader.GetValue(1)
                        + " - "
                        + dreader.GetValue(2)
                        + " - "
                        + dreader.GetValue(3)
                        + " - "
                        + dreader.GetValue(4)
                        + " - "
                        + dreader.GetValue(5)
                        + " - "
                        + dreader.GetValue(6)
                        + "\n";
                }

                dreader.Close();

                Console.Write(output);
                connect.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex);
            }
            
        }

        public void RemoveEmployee()
        {
            

            try
            {
                SqlConnection connect = new SqlConnection(strConnectionString);
                connect.Open();
                string strEmpID;

                Console.WriteLine("Enter Employee's ID:");
                strEmpID = Console.ReadLine();

                SqlCommand cmd = new SqlCommand(
                    "delete from Employee where ID = " + strEmpID,
                    connect
                );

                cmd.ExecuteNonQuery();

                Console.Write("Employee Details Removed Successfully");
                connect.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex);
            }
            
        }

        public void getEmployeeByProject()
        {
            SqlConnection connect = new SqlConnection(strConnectionString);
            connect.Open();

            try
            {
                string strEmpProject;

                Console.WriteLine("Enter Employee's Project Name:");
                strEmpProject = Console.ReadLine();

                SqlDataReader dreader;

                SqlCommand cmd = new SqlCommand(
                    "select * from Projects where Projects.NAME = @ProjectName",
                    connect
                );

                cmd.Parameters.AddWithValue("@ProjectName", strEmpProject);

                dreader = cmd.ExecuteReader();

                List<string> strListProjectIds = new List<string>();

                while (dreader.Read())
                {
                    strListProjectIds.Add(dreader.GetValue(0).ToString());
                }

                dreader.Close();

                string output =
                    "ID - Name - Office - Project - Project Owner - Department - Department Head \n";

                for (int i = 0; i < strListProjectIds.Count; i++)
                {
                    cmd = new SqlCommand(
                        "select Employee.ID, Employee.NAME, Employee.OFFICE, Projects.NAME, Projects.OWNER, Department.NAME, Department.DEPT_HEAD from Employee inner join Projects on Employee.PROJECT_ID = Projects.ID inner join Department on Employee.DEPT_ID = Department.ID where Employee.PROJECT_ID = "
                            + strListProjectIds[i],
                        connect
                    );

                    dreader = cmd.ExecuteReader();

                    while (dreader.Read())
                    {
                        output =
                            output
                            + dreader.GetValue(0)
                            + " - "
                            + dreader.GetValue(1)
                            + " - "
                            + dreader.GetValue(2)
                            + " - "
                            + dreader.GetValue(3)
                            + " - "
                            + dreader.GetValue(4)
                            + " - "
                            + dreader.GetValue(5)
                            + " - "
                            + dreader.GetValue(6)
                            + "\n";
                    }

                    dreader.Close();
                }

                Console.WriteLine(output);
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex);
            }
            connect.Close();
        }

        static void Main(string[] args)
        {
            Program MyObj = new Program();
            int intChoice;

            do
            {
                Console.WriteLine(
                    "Enter Your Choice: \n "
                        + "0 - Exit \n 1 - Create An Employee \n "
                        + "2 - Get Employee Details using ID \n "
                        + "3 - Remove An Employee \n 4 - Get All Employees \n "
                        + "5 - Get Employee Details by Project Name"
                );

                intChoice = int.Parse(Console.ReadLine());
                switch (intChoice)
                {
                    case 1:
                        MyObj.CreateEmployee();
                        break;
                    case 2:
                        MyObj.GetEmployee();
                        break;
                    case 3:
                        MyObj.RemoveEmployee();
                        break;
                    case 4:
                        MyObj.GetAllEmployees();
                        break;
                    case 5:
                        MyObj.getEmployeeByProject();
                        break;
                    default:
                        MyObj.GetAllEmployees();
                        break;
                }
            } while (intChoice != 0);
        }
    }
}
