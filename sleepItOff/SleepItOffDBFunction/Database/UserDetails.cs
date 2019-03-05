using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace SleepItOff.SleepItOffDBFunction.Database
{
	class UserDetails : SingleAbstractRequest
    {
		public override string TableName => "UserDetails";
		private UserDetailsModel Model { get; set; }

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
            Model = JsonConvert.DeserializeObject<UserDetailsModel>(data);
		}

		public string ExistsRowIfCheck()
		{
			return $"SELECT * FROM [dbo].[{TableName}] WHERE [userId] = {PadString(Model.userId.ToString())} ";
		}

		public string UpdateRowQuery()
		{
			var updateStrings = GetColumns().Select(column => $"[{column.Item1}] = {column.Item2} ");
			var updatePart = string.Join(", ", updateStrings);

			return $"UPDATE [dbo].[{TableName}] SET {updatePart} WHERE [userId] = {PadString(Model.userId.ToString())} ";
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

        public override (string, string)[] GetColumns()
		{
			return new[]
			{
				("FirstName", PadString(Model.FirstName)),
				("LastName", PadString(Model.LastName)),
                ("Gender", PadString(Model.Gender)),
                ("Age", PadString(Model.Age.ToString())),
				("Height", PadString(Model.Height.ToString())),
                ("Weight", PadString(Model.Weight.ToString()))
            };
		}

        public class UserDetailsModel
        {
			public string userId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Gender { get; set; }
            public int Age { get; set; }
            public int Height { get; set; }
            public int Weight { get; set; }
		}

		public override object ReaderToObject(SqlDataReader reader)
		{
            if (reader.Read())
            {
                string uid = reader.GetString(0);
                string ufname = reader.GetString(1);
                string ulname = reader.GetString(2);
                string ugender = reader.GetString(3);
                int uage = reader.GetInt32(4);
                int uheight = reader.GetInt32(5);
                int uweight = reader.GetInt32(6);
                return new UserDetailsModel
                {
                    userId = uid,
                    FirstName = ufname,
                    LastName = ulname,
                    Gender = ugender,
                    Age = uage,
                    Height = uheight,
                    Weight = uweight
                };
            }

            return null; 
		}

        public class UserIdsModel
        {
            public List<string> userIds { get; set; }

            public UserIdsModel()
            {
                userIds = new List<string>();
            }
        }

        public string GetUserIdsByGenderQuery(string _gender)
        {
            return $"SELECT [userId] FROM [dbo].[{TableName}] WHERE [gender] = {PadString(_gender)} " +
                   "; ";
        }

        public string GetUserIdsByAgeRangeQuery(int _min, int _max)
        {
            return $"SELECT [userId] FROM [dbo].[{TableName}] WHERE [age] >= {PadString(_min.ToString())} " +
                $"and [age] <= {PadString(_max.ToString())} " +
                   "; ";
        }


        public string GetUserIdsByGenderAndAgeRangeQuery(int _min, int _max, string _gender)
        {
            return $"SELECT [userId] FROM [dbo].[{TableName}] WHERE [gender] = {PadString(_gender)} and [age] >= {PadString(_min.ToString())} " +
                $"and [age] <= {PadString(_max.ToString())} " +
                   "; ";
        }

        public string GetUserIdsByGenderHeightAndWeightQuery(int _minHeight, int _maxHeight, int _minWeight, int _maxWeight, string _gender)
        {
            return $"SELECT [userId] FROM [dbo].[{TableName}] WHERE [gender] = {PadString(_gender)} and [height] >= {PadString(_minHeight.ToString())} " +
                $"and [height] <= {PadString(_maxHeight.ToString())} and [weight] >= {PadString(_minWeight.ToString())} and [weight] <= {PadString(_maxWeight.ToString())}" +
                   "; ";
        }

        public string GetUserIdsByGenderHeightWeightAndAgeRangeQuery(int _minAge, int _maxAge, int _minHeight, int _maxHeight, int _minWeight, int _maxWeight, string _gender)
        {
            return $"SELECT [userId] FROM [dbo].[{TableName}] WHERE [gender] = {PadString(_gender)} and [height] >= {PadString(_minHeight.ToString())} " +
                $"and [height] <= {PadString(_maxHeight.ToString())} and [weight] >= {PadString(_minWeight.ToString())} and [weight] <= {PadString(_maxWeight.ToString())}" +
                   $"and[age] >= { PadString(_minAge.ToString())} and[age] <= { PadString(_maxAge.ToString())} ; ";
        }

        public HttpResponseMessage GetUserIdsByGender(HttpRequestMessage req, ILogger log)
        {
            try
            {
                string _gender = GetParameter(req, "gender");

                var query = GetUserIdsByGenderQuery(_gender);

                var result = GetUserIdsFromDatabase(query, log);

                if (result == null)
                    return req.CreateResponse(HttpStatusCode.NotFound, "No users from specified gender!");

                return SerializeRespone(req, result);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return req.CreateResponse(HttpStatusCode.BadRequest, "Caught Exception - " + e);
            }
        }

        public HttpResponseMessage GetUserIdsByAgeRange(HttpRequestMessage req, ILogger log)
        {
            try
            {
                string _minStr = GetParameter(req, "min");
                string _maxStr = GetParameter(req, "max");

                int _min = Int32.Parse(_minStr);
                int _max = Int32.Parse(_maxStr);

                var query = GetUserIdsByAgeRangeQuery(_min, _max);

                var result = GetUserIdsFromDatabase(query, log);

                if (result == null)
                    return req.CreateResponse(HttpStatusCode.NotFound, "No users in specified age range!");

                return SerializeRespone(req, result);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return req.CreateResponse(HttpStatusCode.BadRequest, "Caught Exception - " + e);
            }
        }

        public HttpResponseMessage GetUserIdsByGenderAndAgeRange(HttpRequestMessage req, ILogger log)
        {
            try
            {
                string _gender = GetParameter(req, "gender");
                string _minStr = GetParameter(req, "min");
                string _maxStr = GetParameter(req, "max");

                int _min = Int32.Parse(_minStr);
                int _max = Int32.Parse(_maxStr);

                var query = GetUserIdsByGenderAndAgeRangeQuery(_min, _max, _gender);

                var result = GetUserIdsFromDatabase(query, log);

                if (result == null)
                    return req.CreateResponse(HttpStatusCode.NotFound, "No users in specified gender & age range!");

                return SerializeRespone(req, result);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return req.CreateResponse(HttpStatusCode.BadRequest, "Caught Exception - " + e);
            }
        }

        public HttpResponseMessage GetUserIdsByGenderHeightAndWeight(HttpRequestMessage req, ILogger log)
        {
            try
            {
                string _gender = GetParameter(req, "gender");
                string _height = GetParameter(req, "height");
                string _weight = GetParameter(req, "weight");

                int height = Int32.Parse(_height);
                int weight = Int32.Parse(_weight);

                int _minHeight = height-4;
                int _maxHeight = height+4;
                int _minWeight = weight-4;
                int _maxWeight = weight+4;

                var query = GetUserIdsByGenderHeightAndWeightQuery(_minHeight, _maxHeight, _minWeight, _maxWeight, _gender);

                var result = GetUserIdsFromDatabase(query, log);

                if (result == null)
                    return req.CreateResponse(HttpStatusCode.NotFound, "No users in specified gender ,height (range) & weight (range)!");

                return SerializeRespone(req, result);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return req.CreateResponse(HttpStatusCode.BadRequest, "Caught Exception - " + e);
            }
        }

        public HttpResponseMessage GetUserIdsByGenderHeightWeightAndAgeRange(HttpRequestMessage req, ILogger log)
        {
            try
            {
                string _gender = GetParameter(req, "gender");
                string _age = GetParameter(req, "age");
                string _height = GetParameter(req, "height");
                string _weight = GetParameter(req, "weight");

                int age = Int32.Parse(_age);
                int height = Int32.Parse(_height);
                int weight = Int32.Parse(_weight);

                int _minAge = age - 4;
                int _maxAge = age + 4;
                int _minHeight = height - 4;
                int _maxHeight = height + 4;
                int _minWeight = weight - 4;
                int _maxWeight = weight + 4;

                var query = GetUserIdsByGenderHeightWeightAndAgeRangeQuery(_minAge, _maxAge, _minHeight, _maxHeight, _minWeight, _maxWeight, _gender);

                var result = GetUserIdsFromDatabase(query, log);

                if (result == null)
                    return req.CreateResponse(HttpStatusCode.NotFound, "No users in specified gender , age (range), height (range) & weight (range)!");

                return SerializeRespone(req, result);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return req.CreateResponse(HttpStatusCode.BadRequest, "Caught Exception - " + e);
            }
        }

        public object GetUserIdsFromDatabase(string query, ILogger log)
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

                    return ExtractUserIdsFromQueryResults(reader);
                }
            }
            catch (Exception e)
            {
                log?.LogInformation(e.ToString());
                return null;
            }
        }

        public UserIdsModel ExtractUserIdsFromQueryResults(SqlDataReader reader)
        {
            UserIdsModel _userIds = new UserIdsModel();            
            while (reader.Read())
            {
                _userIds.userIds.Add(reader.GetString(0));
            }
            return _userIds;
        }
    }
}
 
 
 
 
 
 
 
 
 