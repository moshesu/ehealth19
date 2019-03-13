
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace SleepItOff
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SleepQualityByAgePage : ContentPage
	{
        int graphValue;
        public SleepQualityByAgePage ()
		{
			InitializeComponent ();
            this.graphValue = StatisticsPage.userSleepQualityForHisAgeGraphValue;
            this.Title = "Your Sleep Quality For Your Age";
            NavigationPage.SetHasBackButton(this, true);
            switch (this.graphValue)
            {
                case 0:
                    this.image.Source = "SleepQuality0.png";
                    break;
                case 1:
                    this.image.Source = "SleepQuality1.png";
                    break;
                case 2:
                    this.image.Source = "SleepQuality2.png";
                    break;
            }
        }
    }
}