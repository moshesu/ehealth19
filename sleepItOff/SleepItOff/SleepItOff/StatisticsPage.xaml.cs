using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;

namespace SleepItOff
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatisticsPage : ContentPage
    {
        // todo: edit those pages
        Page stressLevelPage = new SleepItOff.StressLevelPage();
        Page sleepQualityPage = new SleepItOff.SleepQualityPage();

        // todo: use the bellow values in the graph creation
        /* 0 = low sleep quality, 1 = medium sleep quality, 2 = high sleep quality */
        public static int userSleepQualityForHisAgeGraphValue;
        public static int userSleepQualityForHisGenderGraphValue;
        /* 0 = high stress level, 1 = medium stress level, 2 = low stress level */
        public static int userStressLevelGraphValue;
        public static int sleepEffAge;
        public static int wakeUpAge;
        public static int sleepEffGender;
        public static int wakeUpGender;
        /* */
        public static List<int> wakeUpsAcrossTime = new List<int>();
        public static List<int> sleepEfficiencyAcrossTime = new List<int>();

        public StatisticsPage()
        {
            InitializeComponent();
            this.Title = "Your Personal Statistics";
            NavigationPage.SetHasBackButton(this, true);
        }

        private async void OnButtonClicked(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            switch (bt.ClassId)
            {
                case "Stress Level":
                    await Navigation.PushAsync(this.stressLevelPage);
                    break;
                case "How's my Sleep":
                    await Navigation.PushAsync(this.sleepQualityPage);
                    break;
            }
        }
    }
}
