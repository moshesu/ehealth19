using CloudStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eHealthWorkshopGroup4.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MasterProfilePage : MasterDetailPage
	{
        string username;
        CloudStorageHandler storage = null;

        public MasterProfilePage ()
		{
			InitializeComponent ();
            Detail = new NavigationPage(new Profile());
            IsPresented = false;
            this.username = App.MyUserName;
            storage = new CloudStorageHandler();
            if (!App.IsCoach)
            {
                ((StackLayout)masterGrid.Children[1]).IsVisible = false;
            }
        }

        private void returnToProfile(Object sender, EventArgs e)
        {
            IsPresented = false;
        }

        private async void changePasswordHendler(Object sender, EventArgs e)
        {
            if(!await isLegal(0))
            {
                return;
            }
            string newpass = pass1.Text;
            await storage.updateUserPasswordAsync(username, newpass);
            await DisplayAlert("App message", "Password updated", "OK");
            currPass.Text = "";
            pass1.Text = "";
            pass2.Text = "";
        }

        private async void addGroupHendler(Object sender, EventArgs e)
        {
            if (!await isLegal(1))
            {
                return;
            }
            string newgroup = newGroup.Text;
            await storage.createTrainingGroup(newgroup, username);
            await DisplayAlert("App message", "Group added", "OK");
            newGroup.Text = "";
        }

        private async void deleteHndler(Object sender, EventArgs e)
        {
            var response = await DisplayAlert("App Warning", "Are you sure you want to delete your account?", "no", "yes");
            if (!response && !App.IsCoach)
            {
                await storage.removeUserAsync(username);
                await Navigation.PushAsync(new LogInPage());
            }
        }



        private async Task<bool> isLegal(int gridRow)
        {
            bool check = true;
            StackLayout child = (StackLayout)masterGrid.Children[gridRow];
            var grandchildren = child.Children;
            foreach (View grandchild in grandchildren)
            {
                if (grandchild is Entry)
                {
                    check &= CheckEntryLegality((Entry)grandchild);
                }
            }
            if (!check)
            {
                await DisplayAlert("App message", "Please fill all the entries without using hyphen mark", "OK");
                return false;
            }

            switch (gridRow)
            {
                case 0:

                    string pword = currPass.Text;
                    try
                    {
                        await storage.userPasswordQueryAsync(username, pword);
                    }
                    catch (WrongPasswordException)
                    {
                        currPass.BackgroundColor = Color.FromHex("ff5a5a");
                        await DisplayAlert("App message", "Worng password. Please try again.", "OK");
                        return false;
                    }
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

                case 1:
                    
                    if(App.IsCoach && await storage.IsGroupExists(newGroup.Text))
                    {
                        newGroup.BackgroundColor = Color.FromHex("ff3232");
                        await DisplayAlert("App message", "The group name has already been taken. Please select different name.", "OK");
                        return false;
                    }
                    
                    return true;

                default:
                    return true;
            }
            
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

        private void Handle_TextChanged(object sender, EventArgs e)
        {
            Entry entry = sender as Entry;
            entry.BackgroundColor = Color.Transparent;
        }
    }
}