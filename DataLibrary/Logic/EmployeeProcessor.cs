using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.DataAccess;
using DataLibrary.Models;

namespace DataLibrary.Logic
{
    public static class EmployeeProcessor
    {
        public static int CreateEmployee(int employeeId, string usernameID, string firstName, string secondname, string emailAddress)
        {
            EmployeeModel data = new EmployeeModel
            {
                EmployeeID = employeeId,
                UsernameID = usernameID,
                FirstName = firstName,
                SecondName = secondname,
                EmailAddress = emailAddress
            };

            string sql = @"insert into dbo.Employee (EmployeeID, UsernameID, FirstName, SecondName, EmailAddress)
                            values (@EmployeeId, @UsernameID, @FirstName, @SecondName, @EmailAddress);";
            return SqlDataAccess.SaveData(sql, data);
        }

        public static List<EmployeeModel> LoadEmployees()
        {
            string sql = @"select EmployeeID, UsernameID, FirstName, SecondName, EmailAddress
                            from dbo.Employee";

            return SqlDataAccess.LoadData<EmployeeModel>(sql);
        }
    }
}
