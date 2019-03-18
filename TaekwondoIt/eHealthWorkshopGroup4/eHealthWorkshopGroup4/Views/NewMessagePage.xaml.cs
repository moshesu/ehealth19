using CloudStorage;
using eHealthWorkshopGroup4.Models;
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
    public partial class NewMessagePage : ContentPage
    {
        CloudStorageHandler storage = null;

        public NewMessagePage()
        {
            InitializeComponent();
            storage = new CloudStorageHandler();
        }

        protected async override void OnAppearing()
        {
            List<string> groupList = await storage.getGroupsOfCoach(App.MyUserName);
            foreach(string group in groupList)
            {
                groupPicker.Items.Add(group);
            }
        }

        private async void SendHandler(object sender, EventArgs e)
        {
            string groupName = groupPicker.SelectedItem.ToString();
            if(groupName == null)
            {
                await DisplayAlert("App message", "Please choose a traning group", "OK");
            }
            string title = titleEntry.Text;
            string content = contentEditor.Text;
            DateTime date = DateTime.Now;
            Message message = new Message(title, groupName, content, date);
            await storage.WriteMessage(message);
            await DisplayAlert("App message", "Message sent", "OK");
            await Navigation.PushAsync(new MessagesPage());
        }

        private async void GoBackHandler(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MessagesPage());
        }

        private void pickGroup(Object sender, EventArgs e)
        {
            var groupName = groupPicker.Items[groupPicker.SelectedIndex];
        }

    }


}