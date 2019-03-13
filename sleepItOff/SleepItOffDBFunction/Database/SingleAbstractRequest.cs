using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;


namespace SleepItOff.SleepItOffDBFunction.Database
{
	public abstract class SingleAbstractRequest : AbstractRequest
	{
		public string GetRequestIdKey = "userId";
		public string GetRequestIdValue { get; set; }

		public (string, string) GetPrimaryKey() => (GetRequestIdKey, PadString(GetRequestIdValue));

		public abstract (string, string)[] GetColumns();

		public override string GetSelectQuery()
		{
			return  $"SELECT * FROM [dbo].[{TableName}] WHERE [{GetPrimaryKey().Item1}] = {GetPrimaryKey().Item2}; ";
		}

        public override string GetUpsertQuery()
		{
			// Update
			var updateStrings = GetColumns().Select(column => $"[{column.Item1}] = {column.Item2} ");
            var updatePart = string.Join(", ", updateStrings);
            
            //Insert
            var allColumns = new[] {GetPrimaryKey()}.Concat(GetColumns()).ToArray();
			var keys = allColumns.Select(column => $"[{column.Item1}]");
            var keysString = string.Join(", ", keys);

			var values = allColumns.Select(column => column.Item2);
            var valuesString = string.Join(", ", values);
			var insertPart = $"INSERT INTO [dbo].[{TableName}] ({keysString}) VALUES ({valuesString});";

			return
				"begin tran " +
				$"IF EXISTS (SELECT * FROM [dbo].[{TableName}] WHERE [{GetPrimaryKey().Item1}] = {GetPrimaryKey().Item2}) " +
				"begin " +
				$"UPDATE [dbo].[{TableName}] SET {updatePart} WHERE [{GetPrimaryKey().Item1}] = {GetPrimaryKey().Item2}; " +
				"end " +
				"else begin " +
				insertPart +
				"end " +
				"commit tran";
		}

		public HttpResponseMessage Remove(HttpRequestMessage req, ILogger log)
		{
			InitGetData(req);
			try
			{
				var query = GetRemoveQuery();

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

		public string GetRemoveQuery()
		{
			return $"DELETE FROM [dbo].[{TableName}] WHERE [{GetPrimaryKey().Item1}] = {GetPrimaryKey().Item2}; ";
		}
	}
}