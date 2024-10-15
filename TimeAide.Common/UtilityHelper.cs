using MaxMind.GeoIP2.Responses;
using MaxMind.GeoIP2;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using MaxMind.GeoIP2.Model;
using UAParser;

namespace TimeAide.Common.Helpers
{
    public class FilePathHelper
    {
        public string RelativePath
        {
            get;
            set;
        }
        private string _ContentFolderPath = "Content\\doc";
        private string ContentFolderPath
        {
            get
            {
                return _ContentFolderPath;
            }
        }
        private int ClientId;
        private int CompanyId;
        public string ApplicationPath;
        public FilePathHelper()
        {
            ClientId = SessionHelper.SelectedClientId;
            CompanyId = SessionHelper.SelectedCompanyId;
            ApplicationPath = AppDomain.CurrentDomain.BaseDirectory;
        }
        public FilePathHelper(int companyid)
        {
            ClientId = SessionHelper.SelectedClientId;
            CompanyId = companyid;
            ApplicationPath = AppDomain.CurrentDomain.BaseDirectory;
        }
        public FilePathHelper(int clientId, int companyId, string applicationPath)
        {
            ClientId = clientId;
            CompanyId = companyId;
            ApplicationPath = applicationPath;
        }
        public string GetPath(string subFolderName)
        {
            RelativePath = "\\" + ContentFolderPath + "\\" + ClientId + "\\" + CompanyId + "\\" + subFolderName;
            string path = ApplicationPath + "\\" + ContentFolderPath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = ApplicationPath + "\\" + ContentFolderPath + "\\" + ClientId;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = ApplicationPath + "\\" + ContentFolderPath + "\\" + ClientId + "\\" + CompanyId;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = ApplicationPath + "\\" + ContentFolderPath + "\\" + ClientId + "\\" + CompanyId + "\\" + subFolderName;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
        public string GetPath(string subFolderName,bool ClientLevel)
        {
            RelativePath = "\\" + ContentFolderPath + "\\" + ClientId +  "\\" + subFolderName;
            string path = ApplicationPath + "\\" + ContentFolderPath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = ApplicationPath + "\\" + ContentFolderPath + "\\" + ClientId;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = ApplicationPath + "\\" + ContentFolderPath + "\\" + ClientId ;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = ApplicationPath + "\\" + ContentFolderPath + "\\" + ClientId +  "\\" + subFolderName;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
        public string GetPath(string subFolderName, string documentName)
        {
            //documentName = "" + userInformationId + "_" + recordId + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + documentName;
            string path = GetPath(subFolderName);
            RelativePath = RelativePath + "\\" + documentName;
            return path + "\\" + documentName;
        }
        public string GetPath(string subFolderName, string documentName,bool clientLevel )
        {
            //documentName = "" + userInformationId + "_" + recordId + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + documentName;
            string path = GetPath(subFolderName,clientLevel);
            RelativePath = RelativePath + "\\" + documentName;
            return path + "\\" + documentName;
        }
        public string GetPath(string subFolderName, ref string documentName,int userInformationId, int recordId)
        {
            documentName = "" + userInformationId + "_" + recordId + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + documentName;
            string path = GetPath(subFolderName);
            RelativePath = RelativePath + "\\" + documentName;
            return path + "\\" + documentName;
        }
        public string GetCompanyLogoPath(string companyName)
        {
            string logoFile = "\\" + ContentFolderPath + "\\" + ClientId + "\\" + CompanyId + "\\" + "Company\\" + companyName + ".jpg";

            return logoFile;
        }
        public string GetFileName(string fileNameWithPath)
        {
            string filePath = ApplicationPath + fileNameWithPath;
            return Path.GetFileName(filePath);
        }

        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder("");
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
    public class ClientDBConnectionHelper
    {

        public static string GetClientConnection(int clientId)
        {
            string retConnString = "";
           // var clientId = SessionHelper.SelectedClientId;           
            var clientConnectionName = "TimeAideWindow_Client_" + clientId;
            var clientConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[clientConnectionName];
            if (clientConnectionString != null)
            {
                retConnString= clientConnectionString.ConnectionString;
            }

            return retConnString;
        }

    }
    public static class DateToSpanishFormat
    {
        public static string GetDate(DateTime defaultDate)
        {
            var formatStr = defaultDate.ToString("D", new CultureInfo("es-ES"));
            //var formatSubStr = formatStr.Substring(formatStr.IndexOf(",") + 2, formatStr.Length -(formatStr.IndexOf(",") + 2));
            var formatSubStr = formatStr.Split(',')[1].TrimStart(' ');
            return formatSubStr;
        }
    }
    public static class DecimalToWordExtension
    {
        /// <summary>
        /// To the words.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToWords(this decimal value)
        {
            string decimals = "";
            string input = Math.Round(value, 2).ToString();

            if (input.Contains("."))
            {
                decimals = input.Substring(input.IndexOf(".") + 1);
                // remove decimal part from input
                input = input.Remove(input.IndexOf("."));
            }

            // Convert input into words. save it into strWords
            string strWords = GetWords(input) + " Dollars";


            if (decimals.Length > 0)
            {
                // if there is any decimal part convert it to words and add it to strWords.
                strWords += " and " + GetWords(decimals) + " Cents";
            }

            return strWords;
        }

        private static string GetWords(string input)
        {
            // these are seperators for each 3 digit in numbers. you can add more if you want convert beigger numbers.
            string[] seperators = { "", " Thousand ", " Million ", " Billion " };

            // Counter is indexer for seperators. each 3 digit converted this will count.
            int i = 0;

            string strWords = "";

            while (input.Length > 0)
            {
                // get the 3 last numbers from input and store it. if there is not 3 numbers just use take it.
                string _3digits = input.Length < 3 ? input : input.Substring(input.Length - 3);
                // remove the 3 last digits from input. if there is not 3 numbers just remove it.
                input = input.Length < 3 ? "" : input.Remove(input.Length - 3);

                int no = int.Parse(_3digits);
                // Convert 3 digit number into words.
                _3digits = GetWord(no);

                // apply the seperator.
                _3digits += seperators[i];
                // since we are getting numbers from right to left then we must append resault to strWords like this.
                strWords = _3digits + strWords;

                // 3 digits converted. count and go for next 3 digits
                i++;
            }
            return strWords;
        }

        // your method just to convert 3digit number into words.
        private static string GetWord(int no)
        {
            string[] Ones =
            {
            "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven",
            "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Ninteen"
        };

            string[] Tens = { "Ten", "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninty" };

            string word = "";

            if (no > 99 && no < 1000)
            {
                int i = no / 100;
                word = word + Ones[i - 1] + " Hundred ";
                no = no % 100;
            }

            if (no > 19 && no < 100)
            {
                int i = no / 10;
                word = word + Tens[i - 1] + " ";
                no = no % 10;
            }

            if (no > 0 && no < 20)
            {
                word = word + Ones[no - 1];
            }

            return word;
        }


      


    }


    public static class SecurityAlert
    {

        public static String ExtractIp()
        {

            var ip = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null
                       && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != "")
                        ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]
                          : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
          
            if (ip.Contains(","))
                ip = ip.Split(',').First().Trim();
            return ip;




        }

        public static String GetIpLocation(String ip)
        {
            String location = "";

            using (var reader = new DatabaseReader(AppDomain.CurrentDomain.BaseDirectory+"/GeoLite2-City.mmdb"))
            {
                var response = reader.City(ip);

                location = response.City.Name;
            }

            return location;
        }


        public static string GetDeviceDetails()
        {


            string deviceDetails = "";


            var userAgent = HttpContext.Current.Request.UserAgent;
            var uaParser = Parser.GetDefault();
            ClientInfo client = uaParser.Parse(userAgent);


            if (client != null)
            {
                deviceDetails = client.UA.Family
                  + " " + client.UA.Major + "."
                  + client.UA.Minor + " - "
                  + client.OS.Family + " " + client.OS.Major
                  + "." + client.OS.Minor;
            }
            return deviceDetails;

        }

    }
}