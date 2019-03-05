using System.Linq;

namespace SleepItOff.SleepItOffDBFunction.Mocker
{
	
	public class DtabaseTest
	{
		public void DatabaseTest()
		{
			var target = new SleepItOffRepositoryMock();

            var user = target.GetUsers(7171);

		}
	}
}
