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
	class SleepSegmentsStats : SingleAbstractRequest
	{

        public override string TableName => "SleepSegmentsStats";
		private SleepSegmentsStatsModel Model { get; set; }

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
            Model = JsonConvert.DeserializeObject<SleepSegmentsStatsModel>(data);
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
			var allColumns = new[] {("userId", PadString(Model.userId))}.Concat(GetColumns()).ToArray();

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

        public override (string, string)[] GetColumns()
        {
            return new[]
            {
                ("lastUpdated", PadDate(Model.lastUpdated)),

                ("awakeCountTimes", PadString(Model.awakeCountTimes.ToString())),
                ("awakeTotalDuration", PadString(Model.awakeTotalDuration.ToString())),
                ("awakeToAwakeCount", PadString(Model.awakeToAwakeCount.ToString())),
                ("awakeToSnoozeCount", PadString(Model.awakeToSnoozeCount.ToString())),
                ("awakeToDozeCount", PadString(Model.awakeToDozeCount.ToString())),
                ("awakeToRestlessSleepCount", PadString(Model.awakeToRestlessSleepCount.ToString())),
                ("awakeToRestfulSleepCount", PadString(Model.awakeToRestfulSleepCount.ToString())),
                ("awakeToREMCount", PadString(Model.awakeToREMCount.ToString())),

                ("snoozeCountTimes", PadString(Model.snoozeCountTimes.ToString())),
                ("snoozeTotalDuration", PadString(Model.snoozeTotalDuration.ToString())),
                ("snoozeToAwakeCount", PadString(Model.snoozeToAwakeCount.ToString())),
                ("snoozeToSnoozeCount", PadString(Model.snoozeToSnoozeCount.ToString())),
                ("snoozeToDozeCount", PadString(Model.snoozeToDozeCount.ToString())),
                ("snoozeToRestlessSleepCount", PadString(Model.snoozeToRestlessSleepCount.ToString())),
                ("snoozeToRestfulSleepCount", PadString(Model.snoozeToRestfulSleepCount.ToString())),
                ("snoozeToREMCount", PadString(Model.snoozeToREMCount.ToString())),

                ("dozeCountTimes", PadString(Model.dozeCountTimes.ToString())),
                ("dozeTotalDuration", PadString(Model.dozeTotalDuration.ToString())),
                ("dozeToAwakeCount", PadString(Model.dozeToAwakeCount.ToString())),
                ("dozeToSnoozeCount", PadString(Model.dozeToSnoozeCount.ToString())),
                ("dozeToDozeCount", PadString(Model.dozeToDozeCount.ToString())),
                ("dozeToRestlessSleepCount", PadString(Model.dozeToRestlessSleepCount.ToString())),
                ("dozeToRestfulSleepCount", PadString(Model.dozeToRestfulSleepCount.ToString())),
                ("dozeToREMCount", PadString(Model.dozeToREMCount.ToString())),

                ("restlessSleepCountTimes", PadString(Model.restlessSleepCountTimes.ToString())),
                ("restlessSleepTotalDuration", PadString(Model.restlessSleepTotalDuration.ToString())),
                ("restlessSleepToAwakeCount", PadString(Model.restlessSleepToAwakeCount.ToString())),
                ("restlessSleepToSnoozeCount", PadString(Model.restlessSleepToSnoozeCount.ToString())),
                ("restlessSleepToDozeCount", PadString(Model.restlessSleepToDozeCount.ToString())),
                ("restlessSleepToRestlessSleepCount", PadString(Model.restlessSleepToRestlessSleepCount.ToString())),
                ("restlessSleepToRestfulSleepCount", PadString(Model.restlessSleepToRestfulSleepCount.ToString())),
                ("restlessSleepToREMCount", PadString(Model.restlessSleepToREMCount.ToString())),

                ("restfulSleepCountTimes", PadString(Model.restfulSleepCountTimes.ToString())),
                ("restfulSleepTotalDuration", PadString(Model.restfulSleepTotalDuration.ToString())),
                ("restfulSleepToAwakeCount", PadString(Model.restfulSleepToAwakeCount.ToString())),
                ("restfulSleepToSnoozeCount", PadString(Model.restfulSleepToSnoozeCount.ToString())),
                ("restfulSleepToDozeCount", PadString(Model.restfulSleepToDozeCount.ToString())),
                ("restfulSleepToRestlessSleepCount", PadString(Model.restfulSleepToRestlessSleepCount.ToString())),
                ("restfulSleepToRestfulSleepCount", PadString(Model.restfulSleepToRestfulSleepCount.ToString())),
                ("restfulSleepToREMCount", PadString(Model.restfulSleepToREMCount.ToString())),

                ("REMSleepCountTimes", PadString(Model.REMSleepCountTimes.ToString())),
                ("REMSleepTotalDuration", PadString(Model.REMSleepTotalDuration.ToString())),
                ("REMSleepToAwakeCount", PadString(Model.REMSleepToAwakeCount.ToString())),
                ("REMSleepToSnoozeCount", PadString(Model.REMSleepToSnoozeCount.ToString())),
                ("REMSleepToDozeCount", PadString(Model.REMSleepToDozeCount.ToString())),
                ("REMSleepToRestlessSleepCount", PadString(Model.REMSleepToRestlessSleepCount.ToString())),
                ("REMSleepToRestfulSleepCount", PadString(Model.REMSleepToRestfulSleepCount.ToString())),
                ("REMSleepToREMCount", PadString(Model.REMSleepToREMCount.ToString()))
            };
        }

        public class SleepSegmentsStatsModel
        {
            public string userId { get; set; }
            public DateTime lastUpdated { get; set; }

            public int awakeCountTimes { get; set; }
            public int awakeTotalDuration { get; set; }
            public int awakeToAwakeCount { get; set; }
            public int awakeToSnoozeCount { get; set; }
            public int awakeToDozeCount { get; set; }
            public int awakeToRestlessSleepCount { get; set; }
            public int awakeToRestfulSleepCount { get; set; }
            public int awakeToREMCount { get; set; }

            public int snoozeCountTimes { get; set; }
            public int snoozeTotalDuration { get; set; }
            public int snoozeToAwakeCount { get; set; }
            public int snoozeToSnoozeCount { get; set; }
            public int snoozeToDozeCount { get; set; }
            public int snoozeToRestlessSleepCount { get; set; }
            public int snoozeToRestfulSleepCount { get; set; }
            public int snoozeToREMCount { get; set; }

            public int dozeCountTimes { get; set; }
            public int dozeTotalDuration { get; set; }
            public int dozeToAwakeCount { get; set; }
            public int dozeToSnoozeCount { get; set; }
            public int dozeToDozeCount { get; set; }
            public int dozeToRestlessSleepCount { get; set; }
            public int dozeToRestfulSleepCount { get; set; }
            public int dozeToREMCount { get; set; }

            public int restlessSleepCountTimes { get; set; }
            public int restlessSleepTotalDuration { get; set; }
            public int restlessSleepToAwakeCount { get; set; }
            public int restlessSleepToSnoozeCount { get; set; }
            public int restlessSleepToDozeCount { get; set; }
            public int restlessSleepToRestlessSleepCount { get; set; }
            public int restlessSleepToRestfulSleepCount { get; set; }
            public int restlessSleepToREMCount { get; set; }

            public int restfulSleepCountTimes { get; set; }
            public int restfulSleepTotalDuration { get; set; }
            public int restfulSleepToAwakeCount { get; set; }
            public int restfulSleepToSnoozeCount { get; set; }
            public int restfulSleepToDozeCount { get; set; }
            public int restfulSleepToRestlessSleepCount { get; set; }
            public int restfulSleepToRestfulSleepCount { get; set; }
            public int restfulSleepToREMCount { get; set; }

            public int REMSleepCountTimes { get; set; }
            public int REMSleepTotalDuration { get; set; }
            public int REMSleepToAwakeCount { get; set; }
            public int REMSleepToSnoozeCount { get; set; }
            public int REMSleepToDozeCount { get; set; }
            public int REMSleepToRestlessSleepCount { get; set; }
            public int REMSleepToRestfulSleepCount { get; set; }
            public int REMSleepToREMCount { get; set; }
        }

		public override object ReaderToObject(SqlDataReader reader)
		{
			if (reader.Read())
			{
                string uuserId = reader.GetString(0);
                DateTime ulastUpdated = reader.GetDateTime(1);

                int uawakeCountTimes = reader.GetInt32(2);
                int uawakeTotalDuration = reader.GetInt32(3);
                int uawakeToAwakeCount = reader.GetInt32(4);
                int uawakeToSnoozeCount = reader.GetInt32(5);
                int uawakeToDozeCount = reader.GetInt32(6);
                int uawakeToRestlessSleepCount = reader.GetInt32(7);
                int uawakeToRestfulSleepCount = reader.GetInt32(8);
                int uawakeToREMCount = reader.GetInt32(9);

                int usnoozeCountTimes = reader.GetInt32(10);
                int usnoozeTotalDuration = reader.GetInt32(11);
                int usnoozeToAwakeCount = reader.GetInt32(12);
                int usnoozeToSnoozeCount = reader.GetInt32(13);
                int usnoozeToDozeCount = reader.GetInt32(14);
                int usnoozeToRestlessSleepCount = reader.GetInt32(15);
                int usnoozeToRestfulSleepCount = reader.GetInt32(16);
                int usnoozeToREMCount = reader.GetInt32(17);

                int udozeCountTimes = reader.GetInt32(18);
                int udozeTotalDuration = reader.GetInt32(19);
                int udozeToAwakeCount = reader.GetInt32(20);
                int udozeToSnoozeCount = reader.GetInt32(21);
                int udozeToDozeCount = reader.GetInt32(22);
                int udozeToRestlessSleepCount = reader.GetInt32(23);
                int udozeToRestfulSleepCount = reader.GetInt32(24);
                int udozeToREMCount = reader.GetInt32(25);

                int urestlessSleepCountTimes = reader.GetInt32(26);
                int urestlessSleepTotalDuration = reader.GetInt32(27);
                int urestlessSleepToAwakeCount = reader.GetInt32(28);
                int urestlessSleepToSnoozeCount = reader.GetInt32(29);
                int urestlessSleepToDozeCount = reader.GetInt32(30);
                int urestlessSleepToRestlessSleepCount = reader.GetInt32(31);
                int urestlessSleepToRestfulSleepCount = reader.GetInt32(32);
                int urestlessSleepToREMCount = reader.GetInt32(33);

                int urestfulSleepCountTimes = reader.GetInt32(34);
                int urestfulSleepTotalDuration = reader.GetInt32(35);
                int urestfulSleepToAwakeCount = reader.GetInt32(36);
                int urestfulSleepToSnoozeCount = reader.GetInt32(37);
                int urestfulSleepToDozeCount = reader.GetInt32(38);
                int urestfulSleepToRestlessSleepCount = reader.GetInt32(39);
                int urestfulSleepToRestfulSleepCount = reader.GetInt32(40);
                int urestfulSleepToREMCount = reader.GetInt32(41);

                int uREMSleepCountTimes = reader.GetInt32(42);
                int uREMSleepTotalDuration = reader.GetInt32(43);
                int uREMSleepToAwakeCount = reader.GetInt32(44);
                int uREMSleepToSnoozeCount = reader.GetInt32(45);
                int uREMSleepToDozeCount = reader.GetInt32(46);
                int uREMSleepToRestlessSleepCount = reader.GetInt32(47);
                int uREMSleepToRestfulSleepCount = reader.GetInt32(48);
                int uREMSleepToREMCount = reader.GetInt32(49);


                return new SleepSegmentsStatsModel
				{

                userId = uuserId,
                lastUpdated = ulastUpdated,

                awakeCountTimes = uawakeCountTimes,
                awakeTotalDuration = uawakeTotalDuration,
                awakeToAwakeCount = uawakeToAwakeCount,
                awakeToSnoozeCount = uawakeToSnoozeCount,
                awakeToDozeCount = uawakeToDozeCount,
                awakeToRestlessSleepCount = uawakeToRestlessSleepCount,
                awakeToRestfulSleepCount = uawakeToRestfulSleepCount,
                awakeToREMCount = uawakeToREMCount,

                snoozeCountTimes = usnoozeCountTimes,
                snoozeTotalDuration = usnoozeTotalDuration,
                snoozeToAwakeCount = usnoozeToAwakeCount,
                snoozeToSnoozeCount = usnoozeToSnoozeCount,
                snoozeToDozeCount = usnoozeToDozeCount,
                snoozeToRestlessSleepCount = usnoozeToRestlessSleepCount,
                snoozeToRestfulSleepCount = usnoozeToRestfulSleepCount,
                snoozeToREMCount = usnoozeToREMCount,

                dozeCountTimes = udozeCountTimes,
                dozeTotalDuration = udozeTotalDuration,
                dozeToAwakeCount = udozeToAwakeCount,
                dozeToSnoozeCount = udozeToSnoozeCount,
                dozeToDozeCount = udozeToDozeCount,
                dozeToRestlessSleepCount = udozeToRestlessSleepCount,
                dozeToRestfulSleepCount = udozeToRestfulSleepCount,
                dozeToREMCount = udozeToREMCount,

                restlessSleepCountTimes = urestlessSleepCountTimes,
                restlessSleepTotalDuration = urestlessSleepTotalDuration,
                restlessSleepToAwakeCount = urestlessSleepToAwakeCount,
                restlessSleepToSnoozeCount = urestlessSleepToSnoozeCount,
                restlessSleepToDozeCount = urestlessSleepToDozeCount,
                restlessSleepToRestlessSleepCount = urestlessSleepToRestlessSleepCount,
                restlessSleepToRestfulSleepCount = urestlessSleepToRestfulSleepCount,
                restlessSleepToREMCount = urestlessSleepToREMCount,

                restfulSleepCountTimes = urestfulSleepCountTimes,
                restfulSleepTotalDuration = urestfulSleepTotalDuration,
                restfulSleepToAwakeCount = urestfulSleepToAwakeCount,
                restfulSleepToSnoozeCount = urestfulSleepToSnoozeCount,
                restfulSleepToDozeCount = urestfulSleepToDozeCount,
                restfulSleepToRestlessSleepCount = urestfulSleepToRestlessSleepCount,
                restfulSleepToRestfulSleepCount = urestfulSleepToRestfulSleepCount,
                restfulSleepToREMCount = urestfulSleepToREMCount,

                REMSleepCountTimes = uREMSleepCountTimes,
                REMSleepTotalDuration = uREMSleepTotalDuration,
                REMSleepToAwakeCount = uREMSleepToAwakeCount,
                REMSleepToSnoozeCount = uREMSleepToSnoozeCount,
                REMSleepToDozeCount = uREMSleepToDozeCount,
                REMSleepToRestlessSleepCount = uREMSleepToRestlessSleepCount,
                REMSleepToRestfulSleepCount = uREMSleepToRestfulSleepCount,
                REMSleepToREMCount = uREMSleepToREMCount

                };
            }

            return null;
        }


        /* 
         
         
         
         
         
         */

        public string GetSegmentsStatsFromMultipleUsersQuery(string users)
        {
            return $"select avg(awakeCountTimes), avg(awakeTotalDuration), avg(awakeToAwakeCount), avg(awakeToSnoozeCount),"+
                    "avg(awakeToDozeCount),avg(awakeToRestlessSleepCount),avg(awakeToRestfulSleepCount),avg(awakeToREMCount),"+
                    "avg(snoozeCountTimes), avg(snoozeTotalDuration), avg(snoozeToAwakeCount), avg(snoozeToSnoozeCount)," +
                    "avg(snoozeToDozeCount), avg(snoozeToRestlessSleepCount), avg(snoozeToRestfulSleepCount), avg(snoozeToREMCount),"+
                    "avg(dozeCountTimes), avg(dozeTotalDuration),avg(dozeToAwakeCount), avg(dozeToSnoozeCount), avg(dozeToDozeCount),"+
                    "avg(dozeToRestlessSleepCount),avg(dozeToRestfulSleepCount), avg(dozeToREMCount),avg(restlessSleepCountTimes),"+
                    "avg(restlessSleepTotalDuration), avg(restlessSleepToAwakeCount),avg(restlessSleepToSnoozeCount),"+
                    "avg(restlessSleepToDozeCount),avg(restlessSleepToRestlessSleepCount),avg(restlessSleepToRestfulSleepCount),"+
                    "avg(restlessSleepToREMCount),avg(restfulSleepCountTimes),avg(restfulSleepTotalDuration),"+
                    "avg(restfulSleepToAwakeCount),avg(restfulSleepToSnoozeCount),avg(restfulSleepToDozeCount),"+
                    "avg(restfulSleepToRestlessSleepCount),avg(restfulSleepToRestfulSleepCount),avg(restfulSleepToREMCount),"+
                    "avg(REMSleepCountTimes),avg(REMSleepTotalDuration),avg(REMSleepToAwakeCount),avg(REMSleepToSnoozeCount),"+
                    "avg(REMSleepToDozeCount),avg(REMSleepToRestlessSleepCount),avg(REMSleepToRestfulSleepCount),"+
                    $"avg(REMSleepToREMCount) from dbo.SleepSegmentsStats where userId in ({users})" +
                    "; ";
        }

        public HttpResponseMessage GetAverageSleepSegmentsStatsFromUsers(HttpRequestMessage req, ILogger log)
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

                var query = GetSegmentsStatsFromMultipleUsersQuery(users);

                var result = GetValueFromDatabase(query, log);

                if (result == null)
                    return req.CreateResponse(HttpStatusCode.NotFound, "No data for users!");

                return SerializeRespone(req, result);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return req.CreateResponse(HttpStatusCode.BadRequest, "Caught Exception - " + e);
            }
        }


        public object GetValueFromDatabase(string query, ILogger log)
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

                    return ExtractAveragesFromQueryResults(reader);
                    
                }
            }
            catch (Exception e)
            {
                log?.LogInformation(e.ToString());
                return null;
            }
        }

        /* get averages */

        public List<int> ExtractAveragesFromQueryResults(SqlDataReader reader)
        {
            List<int> _wakeUps = new List<int>();
            if (reader.Read())
            {
                _wakeUps.Add(reader.GetInt32(0));
                _wakeUps.Add(reader.GetInt32(1));
                _wakeUps.Add(reader.GetInt32(2));
                _wakeUps.Add(reader.GetInt32(3));
                _wakeUps.Add(reader.GetInt32(4));
                _wakeUps.Add(reader.GetInt32(5));
                _wakeUps.Add(reader.GetInt32(6));
                _wakeUps.Add(reader.GetInt32(7));

                _wakeUps.Add(reader.GetInt32(8));
                _wakeUps.Add(reader.GetInt32(9));
                _wakeUps.Add(reader.GetInt32(10));
                _wakeUps.Add(reader.GetInt32(11));
                _wakeUps.Add(reader.GetInt32(12));
                _wakeUps.Add(reader.GetInt32(13));
                _wakeUps.Add(reader.GetInt32(14));
                _wakeUps.Add(reader.GetInt32(15));

                _wakeUps.Add(reader.GetInt32(16));
                _wakeUps.Add(reader.GetInt32(17));
                _wakeUps.Add(reader.GetInt32(18));
                _wakeUps.Add(reader.GetInt32(19));
                _wakeUps.Add(reader.GetInt32(20));
                _wakeUps.Add(reader.GetInt32(21));
                _wakeUps.Add(reader.GetInt32(22));
                _wakeUps.Add(reader.GetInt32(23));

                _wakeUps.Add(reader.GetInt32(24));
                _wakeUps.Add(reader.GetInt32(25));
                _wakeUps.Add(reader.GetInt32(26));
                _wakeUps.Add(reader.GetInt32(27));
                _wakeUps.Add(reader.GetInt32(28));
                _wakeUps.Add(reader.GetInt32(29));
                _wakeUps.Add(reader.GetInt32(30));
                _wakeUps.Add(reader.GetInt32(31));

                _wakeUps.Add(reader.GetInt32(32));
                _wakeUps.Add(reader.GetInt32(33));
                _wakeUps.Add(reader.GetInt32(34));
                _wakeUps.Add(reader.GetInt32(35));
                _wakeUps.Add(reader.GetInt32(36));
                _wakeUps.Add(reader.GetInt32(37));
                _wakeUps.Add(reader.GetInt32(38));
                _wakeUps.Add(reader.GetInt32(39));

                _wakeUps.Add(reader.GetInt32(40));
                _wakeUps.Add(reader.GetInt32(41));
                _wakeUps.Add(reader.GetInt32(42));
                _wakeUps.Add(reader.GetInt32(43));
                _wakeUps.Add(reader.GetInt32(44));
                _wakeUps.Add(reader.GetInt32(45));
                _wakeUps.Add(reader.GetInt32(46));
                _wakeUps.Add(reader.GetInt32(47));
            }
            return _wakeUps;
        }
    }
}