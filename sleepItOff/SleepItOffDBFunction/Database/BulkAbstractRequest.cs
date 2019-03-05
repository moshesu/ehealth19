namespace SleepItOff.SleepItOffDBFunction.Database
{
	public abstract class BulkAbstractRequest : AbstractRequest
	{
		public int BulkSize = 0;

		public abstract string ExistsRowIfCheck(int index);
		public abstract string UpdateRowQuery(int index);
		public abstract string InsertRowQuery(int index);

		private string GetSingleUpsertQuery(int index = 0)
		{
			return
				$" IF EXISTS ({ExistsRowIfCheck(index)}) " +
				"begin " +
				$"{UpdateRowQuery(index)}; " +
				"end " +
				"else begin " +
				$"{InsertRowQuery(index)}; " +
				"end; ";
		}

		public override string GetUpsertQuery()
		{
			var query = "begin tran ";
			for (var i = 0; i < BulkSize; i++)
			{
				query += GetSingleUpsertQuery(i);
			}
			return query + "commit tran";
		}
	}
}