
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microcharts;
namespace SleepItOff
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SleepQualityByGenderPage : ContentPage
    {
        int graphValue;
        public SleepQualityByGenderPage ()
		{
			InitializeComponent ();
            this.graphValue = StatisticsPage.userSleepQualityForHisGenderGraphValue;
            this.Title = "Your Sleep Quality For Your Gender";
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