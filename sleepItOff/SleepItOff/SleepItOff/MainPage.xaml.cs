using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace SleepItOff
{
    public enum AlarmType
    {
        SmartAlarm = 1,
        CombatNap = 2,
        CreativeSleep = 3
    };

    public enum SegmentStage
    {
        Awake = 1,
        Doze = 2,
        Snooze = 3,
        RestlessSleep = 4,
        RestfulSleep = 5,
        REM = 6
    };


    public partial class MainPage : ContentPage
    {
        Page smartAlarmPage = new SleepItOff.SmartAlarmPage();
        Page combatNapPage = new SleepItOff.CompatNapPage();
        Page creativeSleepPage = new SleepItOff.CreativeSleepPage();

        public MainPage()
        {
            InitializeComponent();
            this.Title ="Hello "+LiveIdCredentials.firstName;
            NavigationPage.SetHasBackButton(this, false);

        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
        private async void OnButtonClicked(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            switch (bt.ClassId)
            {
                case "Combat Nap":
                    await Navigation.PushAsync(this.combatNapPage);
                    break;
                case "Smart Alarm":
                    await Navigation.PushAsync(this.smartAlarmPage);
                    break;
                case "creative sleep":
                    await Navigation.PushAsync(this.creativeSleepPage);
                    break;
                case "Statistics":
                    await Navigation.PushAsync(new SleepItOff.StatisticsPage());
                    break;
                case "help":
                    await Navigation.PushAsync(new SleepItOff.HelpPage());
                    break;
                case "Sign Out":
                    await LoadingLabelXaml.SignoutButton_ClickAsync(sender, e);
                    //cancel alarms in all pages     
                    SleepItOff.Utils.cancel(SleepItOff.SmartAlarmPage.tokenSource, SleepItOff.SmartAlarmPage.token_for_logic);
                    SleepItOff.Utils.cancel(SleepItOff.CompatNapPage.tokenSource, SleepItOff.CompatNapPage.token_for_logic);
                    SleepItOff.Utils.cancel(SleepItOff.CreativeSleepPage.tokenSource, SleepItOff.CreativeSleepPage.token_for_logic);
                    await Navigation.PushAsync(new SleepItOff.SignOutPage());
                    await Task.Delay(new TimeSpan(0, 0, 1));
                    await Navigation.PushAsync(new SleepItOff.SignInPage());
                    break;
            }
        }
    }
}
