using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace SleepItOff.SleepItOffDBFunction.Database
{
	class AwakeningAccuracy : SingleAbstractRequest
	{
		public override string TableName => "AwakeningAccuracy";
		private AwakeningAccuracyModel Model { get; set; }

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
            Model = JsonConvert.DeserializeObject<AwakeningAccuracyModel>(data);
		}

        public override (string, string)[] GetColumns()
		{
			return new[]
			{                
                ("sleepType", PadString(Model.sleepType.ToString())),
				("definedAlarmTime", PadDate(Model.definedAlarmTime)),
				("actualAlarmTime", PadDate(Model.actualAlarmTime)),
                ("timeDifference", PadString(Model.timeDifference.ToString()))
            };
		}

        /*  
	    sleepType 1 = combat nap
	    sleepType 2 = smart alarm
	    sleepType 3 = creative sleep
        */
        public class AwakeningAccuracyModel
        {
			public string userId { get; set; }
            public int sleepType { get; set; }
            public DateTime definedAlarmTime { get; set; }
            public DateTime actualAlarmTime { get; set; }
            public TimeSpan timeDifference { get; set; }
		}

		public override object ReaderToObject(SqlDataReader reader)
		{
			var result = new List<AwakeningAccuracyModel>();
			while (reader.Read())
			{
				result.Add(new AwakeningAccuracyModel
                {
                    userId = reader.GetString(0),
                    sleepType = reader.GetInt32(1),
                    definedAlarmTime = reader.GetDateTime(2),
                    actualAlarmTime = reader.GetDateTime(3),
                    timeDifference = reader.GetTimeSpan(4)
				});
			}

			return result.ToArray();
		}


        /*public override string ExistsRowIfCheck(int index)
         {
             return $"SELECT * FROM [dbo].[{TableName}] WHERE " +
                    $"[userId] = {PadString(Model[index].userId.ToString())} ";
         }

         public override string UpdateRowQuery(int index)
         {
             var updateStrings = GetColumns(index).Select(column => $"[{column.Item1}] = {column.Item2} ");
             var updatePart = string.Join(", ", updateStrings);

             return $"UPDATE [dbo].[{TableName}] SET {updatePart} WHERE " +
                    $"[userId] = {PadString(Model[index].userId.ToString())}";
         }

         public override string InsertRowQuery(int index)
         {
             var allColumns = new[]
             {
                 ("userId", PadString(Model[index].userId.ToString())),
                 ("sleepType", PadString(Model[index].sleepType.ToString())),
                 ("definedAlarmTime", PadDate(Model[index].definedAlarmTime)),
                 ("actualAlarmTime", PadDate(Model[index].actualAlarmTime)),
                 ("timeDifference", PadString(Model[index].timeDifference.ToString()))
             }.Concat(GetColumns(index)).ToArray();

             var keys = allColumns.Select(column => $"[{column.Item1}]");
             var keysString = string.Join(", ", keys);

             var values = allColumns.Select(column => column.Item2);
             var valuesString = string.Join(", ", values);

             return $"INSERT INTO [dbo].[{TableName}] ({keysString}) VALUES ({valuesString});";
         }*/
    }
}