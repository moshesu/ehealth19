
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microcharts;

namespace SleepItOff
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StressLevelPage : ContentPage
	{
        int graphValue;

        public StressLevelPage()
        {
            InitializeComponent();
            this.graphValue = StatisticsPage.userStressLevelGraphValue;
            this.Title = "Your Stress Level";
            NavigationPage.SetHasBackButton(this, true);
            switch (this.graphValue)
            {
                case 0:
                    this.image.Source = "stress0.png";
                    break;
                case 1:
                    this.image.Source = "stress1.png";
                    break;
                case 2:
                    this.image.Source = "stress2.png";
                    break;
            }
        }
    }
}