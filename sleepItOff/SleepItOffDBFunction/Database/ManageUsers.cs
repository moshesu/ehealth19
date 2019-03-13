using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using Newtonsoft.Json;

namespace SleepItOff.SleepItOffDBFunction.Database
{
	class ManageUsers : SingleAbstractRequest
	{
		public override string TableName => "ManageUsers";
		private ManagedUserModel Model { get; set; }

		public override void InitSaveData(HttpRequestMessage req)
		{
			var data = GetParameter(req, "data"); 
            GetRequestIdValue = GetParameter(req, GetRequestIdKey);
            Model = JsonConvert.DeserializeObject<ManagedUserModel>(data);
		}

		public override void InitGetData(HttpRequestMessage req)
		{
			GetRequestIdValue = GetParameter(req, GetRequestIdKey);
		}

		public override (string, string)[] GetColumns() => new[]
		{
            ("lastUpdated", PadDate(Model.lastUpdated)),
		};

		class ManagedUserModel
        {
			public string userId;
			public DateTime lastUpdated;
		}

		public override object ReaderToObject(SqlDataReader reader)
		{
            if (reader.Read()) {
                string uid = reader.GetString(0);
                DateTime ulastUpdated = reader.GetDateTime(1);

                return new ManagedUserModel
                {
                    userId = uid,
                    lastUpdated = ulastUpdated
                };
            }
            return null;
        }
    }
}