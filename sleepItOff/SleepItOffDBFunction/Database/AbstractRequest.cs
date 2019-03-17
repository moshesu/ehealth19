using System;
using System.Net;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Data;

namespace SleepItOff.SleepItOffDBFunction.Database
{
    public abstract class AbstractRequest
    {
        public abstract string TableName { get; }

        protected const string DateFormat = "yyyy-MM-ddTHH\\:mm\\:ss";
        public IsoDateTimeConverter DateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = DateFormat };

        public abstract void InitSaveData(HttpRequestMessage req);
        public abstract void InitGetData(HttpRequestMessage req);

        public HttpResponseMessage Get(HttpRequestMessage req, ILogger log)
        {
            InitGetData(req);
            try
            {
                var query = GetSelectQuery();

                var result = GetFromDatabase(query, log);

                if (result == null)
                    return req.CreateResponse(HttpStatusCode.NotFound, "No such user!");

                return SerializeRespone(req, result);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return req.CreateResponse(HttpStatusCode.BadRequest, "Caught Exception - " + e);
            }
        }

        public object Get(string query, ILogger log = null)
        {
            return GetFromDatabase(query, log);
        }

        public HttpResponseMessage Save(HttpRequestMessage req, ILogger log)
        {
            try
            {
                InitSaveData(req);
                var query = GetUpsertQuery();

                var successful = SaveToDatabase(query, log);

                return !successful
                    ? req.CreateResponse(HttpStatusCode.BadRequest, "Unable to process your request!")
                    : req.CreateResponse(HttpStatusCode.OK, "OK");
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return req.CreateResponse(HttpStatusCode.BadRequest, "Caught Exception - " + e);
            }
        }

        public HttpResponseMessage Update(HttpRequestMessage req, ILogger log)
        {
            try
            {
                InitSaveData(req);
                var query = GetUpsertQuery();

                var successful = SaveToDatabase(query, log);

                return !successful
                    ? req.CreateResponse(HttpStatusCode.BadRequest, "Unable to process your update request!")
                    : req.CreateResponse(HttpStatusCode.OK, "OK");
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return req.CreateResponse(HttpStatusCode.BadRequest, "Caught Exception - " + e);
            }
        }

        public abstract string GetUpsertQuery();
        public abstract string GetSelectQuery();

        public object GetFromDatabase(string query, ILogger log)
        {
            try
            {
                /*
                string cnnString = "Server = tcp:sleepitoffserver.database.windows.net,1433; Initial Catalog = SleepItOffDatabase; Persist Security Info = False; User ID = SleepItOffAdmin; Password = SleepItOff1; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30; ";
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
                {
                    DataSource = "sleepitoffserver.database.windows.net",
                    UserID = "SleepItOffAdmin",
                    Password = "SleepItOff1",
                    InitialCatalog = "SleepItOffDatabase"
                };
                using (var connection = new SqlConnection(cnnString))
                */

                string connectionString = getSqlConnectionString();
                using (var connection = new SqlConnection(connectionString))
                {

                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    log?.LogInformation("Query executed successfully!");
                    return ReaderToObject(reader);
                }
            }
            catch (Exception e)
            {
                log?.LogInformation(e.ToString());
                return null;
            }
        }

        public static bool SaveToDatabase(string query, ILogger log)
        {
            try
            {
                /*
                var cnnString = ConfigurationManager.ConnectionStrings["sqldb_connection"].ConnectionString;
                using (var connection = new SqlConnection(cnnString))
                */

                string connectionString = getSqlConnectionString();
                using (var connection = new SqlConnection(connectionString))
                {
                    var command = new SqlCommand(query, connection);

                    connection.Open();

                    command.ExecuteReader();
                }
            }
            catch (Exception e)
            {
                log.LogInformation(e.ToString());
                return false;
            }

            return true;
        }

        public abstract object ReaderToObject(SqlDataReader reader);

        public static string GetParameter(HttpRequestMessage req, string key)
        {
            NameValueCollection queryParameters = new NameValueCollection();
            string[] querySegments = req.RequestUri.ToString().Split('&');
            string[] queryParams = new string[querySegments.Length - 1];
            for (int i = 0; i < queryParams.GetLength(0); i++)
            {
                queryParams[i] = querySegments[i + 1];
            }

            foreach (string segment in queryParams)
            {
                string[] parts = segment.Split('=');
                if (parts.Length > 0)
                {
                    string keyy = parts[0].Trim(new char[] { '?', ' ' });
                    string val = parts[1].Trim();
                    queryParameters.Add(keyy, val);
                }
            }

            foreach (string s in queryParameters)
            {
                if (String.Compare(s, key, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return queryParameters[s];
                }
            }
            return null;
        }

        /*public static string GetParameter(HttpRequestMessage req, string key)
        {
             NameValueCollection queryParameters = new NameValueCollection();
             string[] querySegments = req.ToString().Split('&');
             foreach (string segment in querySegments)
             {
                 string[] parts = segment.Split('=');
                 if (parts.Length > 0)
                 {
                     string keyy = parts[0].Trim(new char[] { '?', ' ' });
                     string val = parts[1].Trim();

                     queryParameters.Add(keyy, val);
                 }
             }
             foreach (KeyValuePair<string,string> kvp in queryParameters)
             {
                 if (String.Compare(kvp.Key, key, StringComparison.OrdinalIgnoreCase) == 0)
                 {
                     return kvp.Value;
                 }
             }
                 req.GetQueryNameValuePairs()
                     .FirstOrDefault(q => String.Compare(q.Key, key, StringComparison.OrdinalIgnoreCase) == 0)
                     .Value;

                 return result.Contains("\\\"") ? Regex.Unescape(result) : result;     

             return null;    
        }*/

        public static string PadString(string str) => "'" + str + "'";

        public static string PadDate(DateTime date) => PadString(date.ToString("yyyy-MM-ddTHH\\:mm\\:ss"));

        public static HttpResponseMessage SerializeRespone(HttpRequestMessage req, object result)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);

            response.Content = new StringContent(JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json");
            return response;
        }

        public static string getSqlConnectionString(){
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = "sleepitoffserver.database.windows.net",
                UserID = "SleepItOffAdmin",
                Password = "SleepItOff1",
                InitialCatalog = "SleepItOffDatabase"
            };
            return builder.ConnectionString;
        }
	}
}