using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace SleepItOff.SleepItOffDBFunction.Database
{
	class SleepQuality : SingleAbstractRequest
    {
		public override string TableName => "SleepQuality";
		private SleepQualityModel Model { get; set; }

        public override void InitGetData(HttpRequestMessage req)
        {
            GetRequestIdValue = GetParameter(req, GetRequestIdKey);
        }
        
		public override string GetSelectQuery()
		{
			return $"SELECT * FROM [dbo].[{TableName}] WHERE [userId] = {PadString(GetRequestIdValue)} " +
			       "; ";
		}

        public override void InitSaveData(HttpRequestMessage req)
		{
			var data = GetParameter(req, "data");
            GetRequestIdValue = GetParameter(req, GetRequestIdKey);
            Model = JsonConvert.DeserializeObject<SleepQualityModel>(data);
		}

        public override (string, string)[] GetColumns()
		{
			return new[]
			{
				("averageWakeUps", PadString(Model.averageWakeUps.ToString())),
				("averageSleepEfficiency", PadString(Model.averageSleepEfficiency.ToString())),
            };
		}

        public string CheckIfRowExistsQuery()
        {
            return $"SELECT * FROM [dbo].[{TableName}] WHERE [userId] = {PadString(GetRequestIdValue)} ";
        }

        public string UpdateRowQuery()
        {
            var updateStrings = GetColumns().Select(column => $"[{column.Item1}] = {column.Item2} ");
            var updatePart = string.Join(", ", updateStrings);

            return $"UPDATE [dbo].[{TableName}] SET {updatePart} WHERE [userId] = {PadString(GetRequestIdValue)} ";
        }

        
        public string InsertRowQuery()
        {
            var allColumns = new[] { ("userId", PadString(GetRequestIdValue)) }.Concat(GetColumns()).ToArray();

            var keys = allColumns.Select(column => $"[{column.Item1}]");
            var keysString = string.Join(", ", keys);

            var values = allColumns.Select(column => column.Item2);
            var valuesString = string.Join(", ", values);

            return $"INSERT INTO [dbo].[{TableName}] ({keysString}) VALUES ({valuesString});";
        }


        public override string GetUpsertQuery()
        {
            return
                $" IF EXISTS ({CheckIfRowExistsQuery()}) " +
                "begin " +
                $"{UpdateRowQuery()}; " +
                "end " +
                "else begin " +
                $"{InsertRowQuery()}; " +
                "end; ";
        }

        public class SleepQualityModel
        {
			public string userId { get; set; }
            public int averageWakeUps { get; set; }
            public int averageSleepEfficiency { get; set; }
		}

        public override object ReaderToObject(SqlDataReader reader)
		{
            if (reader.Read())
            {
                string uid = reader.GetString(0);
                int uaverageWakeUps = reader.GetInt32(1);
                int uaverageSleepEfficiency = reader.GetInt32(2);
 
                return new SleepQualityModel
                {
                    userId = uid,
                    averageWakeUps = uaverageWakeUps,
                    averageSleepEfficiency = uaverageSleepEfficiency,
                };
            }

            return null; 
		}


        /* ======================= utils =========================== */

        public string GetIsUserInTopUsersBySleepEfficiencyQuery(string _userId, int top)
        {
            return $"select * from (select TOP({top}) * from dbo.SleepQuality ORDER BY [averageSleepEfficiency] DESC) as A " +
                    $"where A.userId={PadString(_userId)}" +
                   "; ";
        }

        public string GetIsUserInTopUsersByWakeUpsQuery(string _userId, int top)
        {
            return $"select * from (select TOP({top}) * from dbo.SleepQuality ORDER BY [averageWakeUps] DESC) as A " +
                    $"where A.userId={PadString(_userId)}" +
                   "; ";
        }

        public string GetAverageForMultipleUsersQuery(string users, string param)
        {
            return $"select [{param}] from dbo.SleepQuality where userId in ({users})" +
                   "; ";
        }

        //NotFound response means the user doesn't sleep well relatively
        //if userId Found by this query, it means the user sleeps well relatively
        public HttpResponseMessage GetIsUserInTopTenUsersBySleepEfficiency(HttpRequestMessage req, ILogger log)
        {
            try
            {
                string _userId = GetParameter(req, "userId");

                var query = GetIsUserInTopUsersBySleepEfficiencyQuery(_userId, 10);

                var result = GetFromDatabase(query, log);

                if (result == null)
                    return req.CreateResponse(HttpStatusCode.NotFound, "User isn't in top ten users by sleep efficiency!");

                return SerializeRespone(req, result);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return req.CreateResponse(HttpStatusCode.BadRequest, "Caught Exception - " + e);
            }
        }

        public HttpResponseMessage GetIsUserInTopFiveUsersBySleepEfficiency(HttpRequestMessage req, ILogger log)
        {
            try
            {
                string _userId = GetParameter(req, "userId");

                var query = GetIsUserInTopUsersBySleepEfficiencyQuery(_userId, 5);

                var result = GetFromDatabase(query, log);

                if (result == null)
                    return req.CreateResponse(HttpStatusCode.NotFound, "User isn't in top five users by sleep efficiency!");

                return SerializeRespone(req, result);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return req.CreateResponse(HttpStatusCode.BadRequest, "Caught Exception - " + e);
            }
        }


        //NotFound response means the user sleeps well relatively
        //if userId Found by this query, it means the user doesn't sleep well relatively
        public HttpResponseMessage GetIsUserInTopTenUsersByWakeUps(HttpRequestMessage req, ILogger log)
        {
            try
            {
                string _userId = GetParameter(req, "userId");

                var query = GetIsUserInTopUsersByWakeUpsQuery(_userId, 10);

                var result = GetFromDatabase(query, log);

                if (result == null)
                    return req.CreateResponse(HttpStatusCode.NotFound, "User isn't top ten users by wake ups!");

                return SerializeRespone(req, result);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return req.CreateResponse(HttpStatusCode.BadRequest, "Caught Exception - " + e);
            }
        }

        public HttpResponseMessage GetIsUserInTopFiveUsersByWakeUps(HttpRequestMessage req, ILogger log)
        {
            try
            {
                string _userId = GetParameter(req, "userId");

                var query = GetIsUserInTopUsersByWakeUpsQuery(_userId, 5);

                var result = GetFromDatabase(query, log);

                if (result == null)
                    return req.CreateResponse(HttpStatusCode.NotFound, "User isn't top five users by wake ups!");

                return SerializeRespone(req, result);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return req.CreateResponse(HttpStatusCode.BadRequest, "Caught Exception - " + e);
            }
        }

        public HttpResponseMessage GetAverageWakeUpsForUsers(HttpRequestMessage req, ILogger log)
        {
            try
            {
                string _users = GetParameter(req, "users");
                string[] __users = _users.Split(',');
                for(int i = 0; i< __users.Length; i++)
                {
                    __users[i] = PadString(__users[i]);
                }

                StringBuilder sb = new StringBuilder(__users[0]);
                for (int i = 1; i < __users.Length; i++)
                {
                    sb.Append("," + __users[i]);
                }

                string users = sb.ToString();

                var query = GetAverageForMultipleUsersQuery(users, "averageWakeUps");

                var result = GetValueFromDatabase(query, log, true);

                if (result == null)
                    return req.CreateResponse(HttpStatusCode.NotFound, "No data for users in this age range!");

                return SerializeRespone(req, result);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return req.CreateResponse(HttpStatusCode.BadRequest, "Caught Exception - " + e);
            }
        }

        public HttpResponseMessage GetAverageSleepEfficiencyForUsers(HttpRequestMessage req, ILogger log)
        {
            try
            {
                string _users = GetParameter(req, "users");
                string[] __users = _users.Split(',');
                for (int i = 0; i < __users.Length; i++)
                {
                    __users[i] = PadString(__users[i]);
                }

                StringBuilder sb = new StringBuilder(__users[0]);
                for (int i = 1; i < __users.Length; i++)
                {
                    sb.Append("," + __users[i]);
                }

                string users = sb.ToString();

                var query = GetAverageForMultipleUsersQuery(users, "averageSleepEfficiency");

                var result = GetValueFromDatabase(query, log, false);

                if (result == null)
                    return req.CreateResponse(HttpStatusCode.NotFound, "No data for users in this age range!");

                return SerializeRespone(req, result);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return req.CreateResponse(HttpStatusCode.BadRequest, "Caught Exception - " + e);
            }
        }


        public object GetValueFromDatabase(string query, ILogger log, bool wakeUps)
        {
            try
            {
                string connectionString = getSqlConnectionString();
                using (var connection = new SqlConnection(connectionString))
                {

                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    log?.LogInformation("Query executed successfully!");

                    if(wakeUps) return ExtractAverageWakeUpTimesFromQueryResults(reader);
                    else return ExtractAverageSleepEfficiencyFromQueryResults(reader);


                }
            }
            catch (Exception e)
            {
                log?.LogInformation(e.ToString());
                return null;
            }
        }

        /* get averages */

        public double ExtractAverageWakeUpTimesFromQueryResults(SqlDataReader reader)
        {
            List<int> _wakeUps = new List<int>();
            while (reader.Read())
            {
                _wakeUps.Add(reader.GetInt32(0));
            }
            return _wakeUps.Average();
        }

        public double ExtractAverageSleepEfficiencyFromQueryResults(SqlDataReader reader)
        {
            List<int> _sleepEfficiency = new List<int>();
            while (reader.Read())
            {
                _sleepEfficiency.Add(reader.GetInt32(0));
            }
            return _sleepEfficiency.Average();
        }

    }
}
 
 
 
 
 
 
 
 
 