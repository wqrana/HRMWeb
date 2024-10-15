using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Common.Helpers;
using TimeAide.Services.Helpers;
//using TimeAide.Web.Extensions;
using TimeAide.Web.Models;

namespace TimeAide.AdminConsole
{
    public class AdminConsoleHelper
    {
        public AdminConsoleHelper()
        {
            DbContext = new TimeAideContext();
            //LogHelper = new LogHelper(DbContext);
            //ScriptsHelper = new ScriptsHelper(DbContext, LogHelper);
        }
        //public LogHelper LogHelper { get; set; }
        //public ScriptsHelper ScriptsHelper { get; set; }
        public TimeAideContext DbContext { get; set; }
        //public Client GetMigrationClient()
        //{
        //    var clientName = ConfigurationManager.AppSettings["ClientName"].ToString();
        //    return DbContext.Client.FirstOrDefault(c => c.ClientName == clientName);
        //}
        //public Client GetClient(int clientId)
        //{
        //    return DbContext.Client.FirstOrDefault(c => c.Id == clientId);
        //}
        //public Client CreateMigrationClient()
        //{
        //    Client ClientInformation = new Client()
        //    {
        //        ClientName = ConfigurationManager.AppSettings["ClientName"].ToString(),
        //        CreatedBy = 1
        //    };
        //    DbContext.Add<Client>(ClientInformation);
        //    DbContext.SaveChanges();
        //    return ClientInformation;
        //}
        //public UserInformation CreateSeedUser()
        //{
        //    Client client = new Client()
        //    {
        //        ClientName = "Identech, Inc.",
        //        CreatedBy = 1,
        //        CreatedDate = DateTime.Now,
        //        DataEntryStatus = 1

        //    };
        //    DbContext.Client.Add(client);
        //    CreateCompany("TimeAide", 1, 1);
        //    UserInformation userInformation = new UserInformation()
        //    {
        //        EmployeeId = 1,
        //        ShortFullName = ConfigurationManager.AppSettings["DefaultShortFullName"].ToString(),
        //        ClientId = 1,
        //        CompanyId = 1,
        //    };
        //    DbContext.UserInformation.Add(userInformation);
        //    EmployeeStatus employeeStatusActive = new EmployeeStatus()
        //    {
        //        Id = 1,
        //        EmployeeStatusName = "Active",
        //        ClientId = 1,
        //    };
        //    DbContext.EmployeeStatus.Add(employeeStatusActive);
        //    EmployeeStatus employeeStatusInactive = new EmployeeStatus()
        //    {
        //        Id = 1,
        //        EmployeeStatusName = "Inactive",
        //        ClientId = 1,
        //    };
        //    DbContext.EmployeeStatus.Add(employeeStatusInactive);
        //    EmployeeStatus employeeStatusClosed = new EmployeeStatus()
        //    {
        //        Id = 1,
        //        EmployeeStatusName = "Closed",
        //        ClientId = 1,
        //    };
        //    DbContext.EmployeeStatus.Add(employeeStatusClosed);
        //    string loginEmail = ConfigurationManager.AppSettings["DefaultAdminEmail"];
        //    UserContactInformation userContactInformation = new UserContactInformation()
        //    {
        //        LoginEmail = loginEmail
        //    };
        //    userInformation.UserContactInformations.Add(userContactInformation);
        //    DbContext.SaveChanges();
        //    return userInformation;
        //}
        //public Company CreateCompany(string companyName,int clientId,int dataEntryStatus)
        //{
        //    if (DbContext.Company.Any(c => c.CompanyName.ToLower() == companyName.ToLower() && c.ClientId == clientId))
        //        return null;
        //    Company company = new Company()
        //    {
        //        CompanyName = companyName,
        //        ClientId = clientId,
        //        CreatedBy = 1,
        //        CreatedDate = DateTime.Now,
        //        DataEntryStatus = dataEntryStatus

        //    };
        //    DbContext.Company.Add(company);
        //    return company;
        //}
        //public TerminationType CreateTerminationType(string terminationTypeName, int clientId, int dataEntryStatus)
        //{
        //    if (DbContext.TerminationType.Any(c => c.TerminationTypeName.ToLower() == terminationTypeName.ToLower() && c.ClientId != clientId))
        //        return null;
        //    TerminationType terminationType = new TerminationType()
        //    {
        //        TerminationTypeName = terminationTypeName,
        //        ClientId = clientId,
        //        CreatedBy = 1,
        //        CreatedDate = DateTime.Now,
        //        DataEntryStatus = dataEntryStatus

        //    };
        //    DbContext.TerminationType.Add(terminationType);
        //    return terminationType;
        //}
        //public TerminationReason CreateTerminationReason(string terminationReasonName, int clientId, int dataEntryStatus)
        //{
        //    if (DbContext.TerminationReason.Any(c => c.TerminationReasonName.ToLower() == terminationReasonName.ToLower() && c.ClientId != clientId))
        //        return null;
        //    TerminationReason terminationReason = new TerminationReason()
        //    {
        //        TerminationReasonName = terminationReasonName,
        //        ClientId = clientId,
        //        CreatedBy = 1,
        //        CreatedDate = DateTime.Now,
        //        DataEntryStatus = dataEntryStatus

        //    };
        //    DbContext.TerminationReason.Add(terminationReason);
        //    return terminationReason;
        //}
        //public void CreateAddDBOwner()
        //{
        //    string sqlConnectionString = DbContext.Database.Connection.ConnectionString;
        //    var sqlScript = "CREATE USER TimeAideDbAdmin FROM LOGIN TimeAideDbAdmin; EXEC sp_addrolemember 'db_owner', 'TimeAideDbAdmin'";

        //    using (var conn = new SqlConnection(sqlConnectionString))
        //    {
        //        if (conn.State == System.Data.ConnectionState.Closed)
        //            conn.Open();

        //        using (var command = new SqlCommand(sqlScript.ToString(), conn))
        //        {
        //            command.ExecuteNonQuery();
        //        }
        //    }
        //}
        //public int LoadFilesFromDatabase(int clientId, string tableName, string binColumnNames, string fileNameColumn, string subFolderName,string sourceDatabase)
        //{
        //    string folderPath = ConfigurationManager.AppSettings["FilesDownloadPath"].ToString();

        //    string path = TimeAide.Common.UtilityHelper.GetPath(clientId,1, folderPath, subFolderName);

        //    string sqlConnectionString = DbContext.Database.Connection.ConnectionString;
        //    SqlConnection con = new SqlConnection(sqlConnectionString);
        //    con.Open();
        //    string sql = "Select " + fileNameColumn + "," + binColumnNames + " from " + sourceDatabase + ".dbo." + tableName + " where not " + binColumnNames + " is null";
        //    SqlCommand cmd = new SqlCommand(sql, con);
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    DataSet ds = new DataSet();
        //    da.Fill(ds);
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        foreach (DataRow each in ds.Tables[0].Rows)
        //        {
        //            byte[] bytes = (byte[])each[binColumnNames];
        //            MemoryStream memStream = new MemoryStream(bytes);
        //            string fileName = each[fileNameColumn].ToString();
        //            if (!string.IsNullOrEmpty(fileName) && !fileName.Contains("."))
        //                fileName =fileName + ".jpg"; 
        //            File.WriteAllBytes( path + fileName, bytes);
        //        }
        //    }
        //    return ds.Tables[0].Rows.Count;
        //}
        //public DataTable GetCompany()
        //{
        //    DataTable t1 = new DataTable();
        //    string sqlConnectionString = DbContext.Database.Connection.ConnectionString;
        //    using (SqlConnection con = new SqlConnection(sqlConnectionString))
        //    {
        //        con.ChangeDatabase("TimeAideSource");
        //        string query = "SELECT * FROM tblCompany";
        //        SqlCommand cmd = new SqlCommand(query, con);
        //        using (SqlDataAdapter a = new SqlDataAdapter(cmd))
        //        {
        //            a.Fill(t1);
        //        }
        //    }
        //    return t1;
        //}
    }
}
