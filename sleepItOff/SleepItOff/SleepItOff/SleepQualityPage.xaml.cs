using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SleepItOff
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SleepQualityPage : ContentPage
	{
        Page sleepQualityByAgePage = new SleepItOff.SleepQualityByAgePage();
        Page sleepQualityByGenderPage = new SleepItOff.SleepQualityByGenderPage();
        Page sleepQualityAcrossTime = new SleepItOff.SleepQualityAcrossTime();

        public SleepQualityPage ()
		{
			InitializeComponent ();
            this.Title = "Check Your Sleep Quality";
            NavigationPage.SetHasBackButton(this, true);
        }

        private async void OnButtonClicked(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            switch (bt.ClassId)
            {
                case "Changes in my sleep quality across time":
                    await Navigation.PushAsync(this.sleepQualityAcrossTime);
                    break;
                case "How's my Sleep for my age":
                    await Navigation.PushAsync(this.sleepQualityByAgePage);
                    break;
                case "How's my Sleep for my gender":
                    await Navigation.PushAsync(this.sleepQualityByGenderPage);
                    break;
            }
        }
    }
}