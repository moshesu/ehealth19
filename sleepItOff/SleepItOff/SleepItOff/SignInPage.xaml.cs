using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SleepItOff
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignInPage : ContentPage
    {
        public SignInPage()
        {
            InitializeComponent();
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
                case "Sign In":
                    await Navigation.PushAsync(new LoadingLabelXaml());
                    break;
                case "help":
                    await Navigation.PushAsync(new SleepItOff.HelpPage());
                    break;
                case "Sign Out":
                    //Navigation.PushAsync(new SleepItOff.SignOutPage());
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