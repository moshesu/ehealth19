using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microcharts;

namespace SleepItOff
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SleepQualityAcrossTime : ContentPage
	{
		public SleepQualityAcrossTime ()
		{
			InitializeComponent ();
            this.Title = "Check Your Sleep Quality Across Time";
            NavigationPage.SetHasBackButton(this, true);

            // building the graph
            int sleepEfficiencyDataLength = StatisticsPage.sleepEfficiencyAcrossTime.Count;
            int howMany = Math.Min(7, sleepEfficiencyDataLength);
            var efficiencyEntries = new Microcharts.Entry[howMany];
            for(int i=0; i<howMany; i++)
            {
                efficiencyEntries[i] = new Microcharts.Entry(StatisticsPage.sleepEfficiencyAcrossTime[sleepEfficiencyDataLength - howMany + i])
                    { ValueLabel = (StatisticsPage.sleepEfficiencyAcrossTime[sleepEfficiencyDataLength - howMany + i]).ToString(),
                        Label = i.ToString() };
            }

            int wakeUpsDataLength = StatisticsPage.wakeUpsAcrossTime.Count;
            int howManyWakeUps = Math.Min(7, wakeUpsDataLength);
            var wakeUpsEntries = new Microcharts.Entry[howManyWakeUps];
            for (int i = 0; i < howManyWakeUps; i++)
            {
                wakeUpsEntries[i] = new Microcharts.Entry(StatisticsPage.wakeUpsAcrossTime[wakeUpsDataLength - howManyWakeUps + i])
                {
                    ValueLabel = (StatisticsPage.wakeUpsAcrossTime[wakeUpsDataLength - howManyWakeUps + i]).ToString(),
                    Label = i.ToString()
                };
            }

            var efficiencyChart = new LineChart()
            {
                Entries = efficiencyEntries
            };

            var wakeUpsChart = new LineChart()
            {
                Entries = wakeUpsEntries
            };

            this.efficiencyChart.Chart = efficiencyChart;
            this.wakeUpsChart.Chart = wakeUpsChart;
        }


	}
}