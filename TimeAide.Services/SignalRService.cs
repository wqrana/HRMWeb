using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAide.Services
{
    public static class SignalRService
    {
        static SignalRService()
        {
            CompanySessionTimeout = new Dictionary<int, int>();
        }
        //SqlConnection sqlcon = new SqlConnection(ConfigurationManager.ConnectionStrings["TimeAideContext"].ConnectionString);
        public static void UpdateChateMessage(string username, string userid, string connectionid, string chatConversationId, string clientId, string companyId, string name)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["TimeAideContext"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("sp_UpdateSignalROnlineChatUsers", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@UserName", SqlDbType.VarChar).Value = username;
                    sqlCommand.Parameters.AddWithValue("@UserId", SqlDbType.VarChar).Value = userid;
                    sqlCommand.Parameters.AddWithValue("@ConnectionId", SqlDbType.VarChar).Value = connectionid;
                    sqlCommand.Parameters.AddWithValue("@UserInformationName", SqlDbType.VarChar).Value = name;
                    sqlCommand.Parameters.AddWithValue("@ActiveChatConversationId", SqlDbType.VarChar).Value = chatConversationId;
                    sqlCommand.Parameters.AddWithValue("@CreatedBy", SqlDbType.VarChar).Value = 1;
                    sqlCommand.Parameters.AddWithValue("@ClientId", SqlDbType.VarChar).Value = clientId;
                    sqlCommand.Parameters.AddWithValue("@CompanyId", SqlDbType.VarChar).Value = companyId;

                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }
        public static int GetCount()
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["TimeAideContext"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("select COUNT([UserName]) as TotalCount from [ChatUsers]", sqlConnection))
                {
                    sqlConnection.Open();
                    return int.Parse(sqlCommand.ExecuteScalar().ToString()); ;
                }
            }
        }
        public static string GetUsers(string username)
        {
            string list = "";

            int count = SignalRService.GetCount();
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["TimeAideContext"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("select [UserName],[ConnectionID] from [ChatUSers] where ([UserName]<>'" + username + "')", sqlConnection))
                {
                    sqlConnection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    reader.Read();

                    for (int i = 0; i < (count - 1); i++)
                    {
                        list += reader.GetValue(0).ToString() + " ( " + reader.GetValue(1).ToString() + " )#";
                        reader.Read();
                    }
                }
            }
            return list;
        }
        public static bool DeleteRecord(string connectionid)
        {
            bool result = false;
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["TimeAideContext"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("delete from [ChatUsers] where ([ConnectionID]='" + connectionid + "')", sqlConnection))
                {
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    result = true;
                }
                return result;
            }
        }

        public static string GetUserInformationName(string username)
        {
            var dt =  GetUserInformation(username);
            string userName = "";
            foreach (DataRow each in dt.Rows)
            {
                userName = each["ShortFullName"].ToString();
                break;
            }
            return username;
        }

        public static DataTable GetUserInformation(string username)
        {
            string query = "Select top 1 ShortFullName,ui.CompanyId,ui.ClientId from UserContactInformation uc Inner join UserInformation ui on uc.UserInformationId=ui.UserInformationId where uc.LoginEmail = '" + username + "'";
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["TimeAideContext"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlConnection.Open();
                    DataTable dt = new DataTable();
                    dt.Load(sqlCommand.ExecuteReader());
                    
                    return dt;
                }
            }
        }

        public static DataTable GetApplicationConfiguration(string configurationName, int clientId, int companyId)
        {
            //string query = "Select * from ApplicationConfiguration where ApplicationConfigurationName = '" + configurationName + "'";
            string query = "Select *from ApplicationConfiguration  where ApplicationConfigurationName = '" + configurationName + "' AND ClientId = "+ clientId + " and(CompanyId is null Or  CompanyId = "+ companyId + ")";
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["TimeAideContext"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlConnection.Open();
                    DataTable dt = new DataTable();
                    dt.Load(sqlCommand.ExecuteReader());

                    return dt;
                }
            }
        }
        public static string GetUserByUserInformationId(string userInformationId)
        {
            string query = "SELECT UserInformationId, LoginEmail FROM UserContactInformation where LoginEmail is not null and UserInformationId = '" + userInformationId + "'";
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["TimeAideContext"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlConnection.Open();
                    DataTable dt = new DataTable();
                    dt.Load(sqlCommand.ExecuteReader());
                    string loginEmail = "";
                    foreach (DataRow each in dt.Rows)
                    {
                        loginEmail = each["LoginEmail"].ToString();
                        break;
                    }
                    return loginEmail;
                }
            }
        }

        public static string GetUserConnectionId(string loginEmail)
        {
            string query = "SELECT [UserName],[UserID],[ConnectionID],[UserInformationName] FROM ChatUsers where [ConnectionID] is not null and RTRIM(LTrim([ConnectionID]))<>'' And UserInformationName = '" + loginEmail + "' Order by CreatedDate desc ";
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["TimeAideContext"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    sqlConnection.Open();
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    string connectionID = "";
                    foreach (DataRow each in dt.Rows)
                    {
                        connectionID = each["ConnectionID"].ToString();
                        break;
                    }
                    return connectionID;
                }
            }
        }

        private static Dictionary<int, int> _CompanySessionTimeout;
        public static Dictionary<int, int> CompanySessionTimeout
        {
            get;
            set;
        }

        public static void UpdateSessionLog(string username)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["TimeAideContext"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("sp_UpdateEventHub", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Details", SqlDbType.VarChar).Value = username;

                    sqlConnection.Open();
                    //sqlCommand.ExecuteNonQuery();
                }
            }
        }

    }
}
