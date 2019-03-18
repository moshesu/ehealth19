using CloudStorage;
using eHealthWorkshopGroup4.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static CloudStorage.CloudStorageHandler;
using static CloudStorage.UserStorage;

namespace eHealthWorkshopGroup4.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SingUpPage : ContentPage
	{
        int currentRow = 0;
        CloudStorageHandler storage;
        
        private string fullName = "";
        private string userName = "";
        private string password = "";
        private string traningGroupName = "";
        private string coachTrainedGroupName = "";
        private Rank userRank = Rank.Beginner;

        private bool _isCoach;
        public bool IsCoach
        {
            get { return _isCoach; }
            set
            {
                _isCoach = value;
                if (!value)
                {
                    coachGroupNameEntry.Text = null;
                }
                OnPropertyChanged(nameof(IsCoach));
            }
        }

        private bool _ignoreTextChanged;

        public SingUpPage ()
		{
			InitializeComponent ();
            storage = new CloudStorageHandler();
            IsCoach = false;
            _ignoreTextChanged = false;
        }

        protected override void OnAppearing()
        {
            for (int i = 1; i < singUpGrid.Children.Count - 1; i++)
            {
                singUpGrid.Children[i].IsVisible = false;
                singUpGrid.Children[i].FadeTo(0, 0, Easing.SinInOut);
            }

            var rankArray = Enum.GetValues(typeof(Rank));
            for (int j = 0; j < rankArray.Length; j++)
            {
                rankPicker.Items.Add(rankArray.GetValue(j).ToString());
            }
        }

        private async void NextClick (Object sender, EventArgs e)
        {

            bool legal = true;

            if (currentRow < singUpGrid.Children.Count - 2)
            {

                legal = await isleagalChaild();
                if (legal)
                {
                    switch (currentRow)
                    {
                        case 0:
                            fullName = fnameEntry.Text + " " + lnameEntry.Text;
                            break;
                        case 1:
                            userName = unameEntry.Text;
                            break;
                        case 2:
                            Enum.TryParse(rankPicker.SelectedItem.ToString(), true, out userRank);
                            traningGroupName = groupNameEntry.Text;
                            coachTrainedGroupName = coachGroupNameEntry.Text;
                            break;
                        case 3:
                            password = pass1.Text;
                            break;
                    }
                    FadeNavigation();
                }
  
            }
            else
            {

                var applicationTypeInfo = Application.Current.GetType().GetTypeInfo();
                Stream profileStream = applicationTypeInfo.Assembly.GetManifestResourceStream($"{applicationTypeInfo.Namespace}.BruceLeeProfile.png");
                Stream subjectStream = applicationTypeInfo.Assembly.GetManifestResourceStream($"{applicationTypeInfo.Namespace}.KungFuBackGround.jpg");

                App.MyUserName = userName;
                App.IsCoach = IsCoach;

                UserProfileData upd = new UserProfileData(fullName, userName, userRank,
                    IsCoach, traningGroupName);
                await storage.addUserAsync(upd, password, profileStream, subjectStream, coachTrainedGroupName);
                
                App.Current.MainPage = new MainPage();
            }
        }


        private async Task<bool> isleagalChaild()
        {
            // Check that the relevant entries isn't empty and without hyphen mark
            bool check = true;
            StackLayout child = (StackLayout)singUpGrid.Children[currentRow];
            var grandchildren = child.Children;
            //Entry entry = null;
            foreach (View grandchild in grandchildren)
            {
                if (grandchild is Entry)
                {
                    //entry = (Entry)grandchild;
                    check &= CheckEntryLegality((Entry)grandchild);
                }
            }

            if (currentRow == 2)
            {
                check &= CheckEntryLegality(groupNameEntry);
                if (IsCoach)
                    check &= CheckEntryLegality(coachGroupNameEntry);
            }
            
            if (!check)
            {
                await DisplayAlert("App message", "Please fill all the entries without using hyphen mark", "OK");
                return false;
            }

            switch (currentRow)
            {
                //Check whether the user name is available
                case 1:
                    string uname = unameEntry.Text;
                    int firstAscii = (int)uname[0];
                    if (!(((64 < firstAscii)&&(firstAscii < 123)) || ((96 < firstAscii)&&(firstAscii < 123))))
                    {
                        unameEntry.BackgroundColor = Color.FromHex("ff3232");
                        await DisplayAlert("App message", "Username can't contain non-English first characters.", "OK");
                        return false;
                    }
                    if (await storage.isUsernameTaken(uname))
                    {
                        unameEntry.BackgroundColor = Color.FromHex("ff3232");
                        await DisplayAlert("App message", "The username has already been taken. Please select different name.", "OK");
                        return false;
                    }
                    return true;
                //Check whether the group name exists and coach trained group name is available
                case 2:
                    if (!await IsGroupNameExists(groupNameEntry.Text))
                    {
                        groupNameEntry.BackgroundColor = Color.FromHex("ff3232");
                        await DisplayAlert("App message", "the entered group name is not exist. Please select the group from" +
                            " the suggested list", "OK");
                        return false;
                    }
                    if(IsCoach && await IsGroupNameExists(coachGroupNameEntry.Text))
                    {
                        coachGroupNameEntry.BackgroundColor = Color.FromHex("ff3232");
                        await DisplayAlert("App message", "The group name has already been taken. Please select different name.", "OK");
                        return false;
                    }
                    return true;
                //Check the password legality
                case 3:
                    if (pass1.Text.Length < 7 || pass2.Text.Length < 7)
                    {
                        if (pass1.Text.Length < 7)
                            pass1.BackgroundColor = Color.FromHex("ff3232");
                        if (pass2.Text.Length < 7)
                            pass2.BackgroundColor = Color.FromHex("ff3232");
                        await DisplayAlert("App message", "Please enter a password with at least 7 characters", "OK");
                        return false;
                    }
                    if (!(pass1.Text).Equals(pass2.Text))
                    {
                        pass1.BackgroundColor = Color.FromHex("ff3232");
                        pass2.BackgroundColor = Color.FromHex("ff3232");
                        await DisplayAlert("App message", "Please enter two identical passwords", "OK");
                        return false;
                    }
                    return true;
                default:
                    return true;
            }
            
        }

        private async void FadeNavigation()
        {
            var fadeTasks = new Task<bool>[2];
            fadeTasks[0] = singUpGrid.Children[currentRow].FadeTo(0, 500, Easing.SinInOut);
            fadeTasks[1] = nextButton.FadeTo(0, 500, Easing.SinInOut);
            await Task.WhenAll(fadeTasks);

            singUpGrid.Children[currentRow].IsVisible = false;
            singUpGrid.Children[currentRow + 1].IsVisible = true;

            if (currentRow == singUpGrid.Children.Count - 3)
            {
                nextButton.Text = "start using the app";
            }

            var brightenTasks = new Task<bool>[2];
            brightenTasks[0] = singUpGrid.Children[currentRow + 1].FadeTo(1, 500, Easing.SinInOut);
            brightenTasks[1] = nextButton.FadeTo(1, 500, Easing.SinInOut);
            await Task.WhenAll(brightenTasks);

            currentRow += 1;
        }

        private bool CheckEntryLegality(Entry entry)
        {
            if (entry.Text == null || entry.Text == "" || entry.Text.Contains("-"))
            {
                entry.BackgroundColor = Color.FromHex("ff5a5a");
                return false;
            }
            return true;
        }

        private void switch_onToggled(object sender, ToggledEventArgs e)
        {
            IsCoach = e.Value;
        }

        private void pickRank(Object sender, EventArgs e)
        {
            var rank = rankPicker.Items[rankPicker.SelectedIndex];
        }

        /*private void pickGroup(Object sender, EventArgs e)
        {
            var group = GroupPicker.Items[GroupPicker.SelectedIndex];
        }*/

        private async void Handle_TextChanged (object sender, EventArgs e)
        {
            Entry entry = sender as Entry;
            entry.BackgroundColor = Color.Transparent;
            if(entry == groupNameEntry)
            {
                string key = entry.Text;
                if (!_ignoreTextChanged && (key != null && key.Length >= 1))
                {
                    var suggestions = await storage.getGroups(key);
                    SuggestionsListView.ItemsSource = suggestions;  
                    stackName.IsVisible = false;
                    SuggestionsListView.IsVisible = true;
                }
                else
                {
                    SuggestionsListView.IsVisible = false;
                    stackName.IsVisible = true;
                    _ignoreTextChanged = false;
                }
            }
        }

        private void Handle_ItemTapped(Object sender, ItemTappedEventArgs e)
        {
            var groupName = e.Item as string;
            groupNameEntry.Text = groupName;
            SuggestionsListView.IsVisible = false;
            stackName.IsVisible = true;
            _ignoreTextChanged = true;
        }

        private async Task<bool> IsGroupNameExists(string key)
        {
            var suggestions = await storage.getGroups(key);
            if (suggestions.Contains(key))
            {
                return true;
            }
            return false;
        }

    }
}