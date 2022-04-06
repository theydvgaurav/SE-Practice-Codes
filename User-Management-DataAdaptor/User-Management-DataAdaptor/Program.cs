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

                DataSet ds = new DataSet();

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

                SqlDataAdapter sda = new SqlDataAdapter(
                    "INSERT INTO Projects(NAME, OWNER) VALUES(@name,@owner)",
                    connect
                );

                sda.SelectCommand.Parameters.AddWithValue("@Name", strProjectName);
                sda.SelectCommand.Parameters.AddWithValue("@Owner", strProjectOwner);

                sda.Fill(ds);

                sda = new SqlDataAdapter("SELECT MAX(ID) AS ID FROM Projects", connect);

                sda.Fill(ds);

                string ProjectID = "";

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ProjectID += row[0];
                }

                sda = new SqlDataAdapter(
                    "INSERT INTO Department(NAME, DEPT_HEAD) VALUES(@name,@head)",
                    connect
                );

                sda.SelectCommand.Parameters.AddWithValue("@Name", strDeptName);
                sda.SelectCommand.Parameters.AddWithValue("@Head", strDeptHead);

                sda.Fill(ds);

                ds.Clear();

                sda = new SqlDataAdapter("SELECT MAX(ID) AS ID FROM Department", connect);

                sda.Fill(ds);

                string DepartmentID = "";

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DepartmentID += row[0];
                }

                sda = new SqlDataAdapter(
                    "INSERT INTO Employee(NAME, OFFICE, PROJECT_ID, DEPT_ID) VALUES(@name,@office,@project,@dept)",
                    connect
                );

                sda.SelectCommand.Parameters.AddWithValue("@Name", strEmpName);
                sda.SelectCommand.Parameters.AddWithValue("@Office", strOffice);
                sda.SelectCommand.Parameters.AddWithValue("@Project", int.Parse(ProjectID));
                sda.SelectCommand.Parameters.AddWithValue("@Dept", int.Parse(DepartmentID));

                sda.Fill(ds);

                Console.WriteLine("Employee Details Added Successfully");
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

                SqlDataAdapter sda = new SqlDataAdapter(
                    "select Employee.ID, Employee.NAME, Employee.OFFICE, Projects.NAME, Projects.OWNER, Department.NAME, Department.DEPT_HEAD from Employee inner join Projects on Employee.PROJECT_ID = Projects.ID inner join Department on Employee.DEPT_ID = Department.ID",
                    connect
                );

                DataSet ds = new DataSet();

                sda.Fill(ds);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Console.WriteLine(
                        "{0} {1} {2} {3} {4} {5} {6}",
                        row[0],
                        row[1],
                        row[2],
                        row[3],
                        row[4],
                        row[5],
                        row[6]
                    );
                }
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
                DataSet ds = new DataSet();

                string strEmpID;

                Console.WriteLine("Enter Employee's ID:");
                strEmpID = Console.ReadLine();

                SqlDataAdapter sda = new SqlDataAdapter(
                    "select Employee.ID, Employee.NAME, Employee.OFFICE, Projects.NAME, Projects.OWNER, Department.NAME, Department.DEPT_HEAD from Employee inner join Projects on Employee.PROJECT_ID = Projects.ID inner join Department on Employee.DEPT_ID = Department.ID where Employee.ID = @EmpID",
                    connect
                );
                sda.SelectCommand.Parameters.AddWithValue("@EmpID", strEmpID);

                sda.Fill(ds);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Console.WriteLine(
                        "{0} {1} {2} {3} {4} {5} {6}",
                        row[0],
                        row[1],
                        row[2],
                        row[3],
                        row[4],
                        row[5],
                        row[6]
                    );
                }
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
                DataSet ds = new DataSet();
                string strEmpID;

                Console.WriteLine("Enter Employee's ID:");
                strEmpID = Console.ReadLine();

                SqlDataAdapter sda = new SqlDataAdapter(
                    "select * from Employee where ID = @EmpID select * from Projects",
                    connect
                );

                sda.SelectCommand.Parameters.AddWithValue("@EmpID", strEmpID);

                sda.Fill(ds);

                string strProjectId = "",
                    strDeptId = "";

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    strProjectId += row[3].ToString();
                    strDeptId += row[4].ToString();
                }

                sda = new SqlDataAdapter("delete from Employee where ID = @EmpID delete from Employee where ID = @ProjectId delete from Employee where ID = @DeptId", connect);

                sda.SelectCommand.Parameters.AddWithValue("@EmpID", strEmpID);

                sda.SelectCommand.Parameters.AddWithValue("@ProjectId", strProjectId);

                sda.SelectCommand.Parameters.AddWithValue("@DeptId", strDeptId);

                sda.Fill(ds);

                Console.Write("Employee Details Removed Successfully \n");
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void getEmployeeByProject()
        {
            try
            {
                SqlConnection connect = new SqlConnection(strConnectionString);
                DataSet ds = new DataSet();

                string strEmpProject;

                Console.WriteLine("Enter Employee's Project Name:");
                strEmpProject = Console.ReadLine();

                SqlDataAdapter sda = new SqlDataAdapter(
                    "select * from Projects where Projects.NAME = @ProjectName",
                    connect
                );

                sda.SelectCommand.Parameters.AddWithValue("@ProjectName", strEmpProject);

                sda.Fill(ds);

                List<string> strListProjectIds = new List<string>();

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    strListProjectIds.Add(row[0].ToString());
                }

                for (int i = 0; i < strListProjectIds.Count; i++)
                {
                    ds.Clear();

                    sda = new SqlDataAdapter(
                        "select Employee.ID, Employee.NAME, Employee.OFFICE, Projects.NAME, Projects.OWNER, Department.NAME, Department.DEPT_HEAD from Employee inner join Projects on Employee.PROJECT_ID = Projects.ID inner join Department on Employee.DEPT_ID = Department.ID where Employee.PROJECT_ID = @ProjectId",
                        connect
                    );

                    sda.SelectCommand.Parameters.AddWithValue("@ProjectId", strListProjectIds[i]);

                    sda.Fill(ds);

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        Console.WriteLine(
                            "{0} {1} {2} {3} {4} {5} {6}",
                            row[0],
                            row[1],
                            row[2],
                            row[3],
                            row[4],
                            row[5],
                            row[6]
                        );
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex);
            }
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
