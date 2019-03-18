using eHealthWorkshopGroup4.ViewModels;
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
	public partial class MessagesPage : ContentPage
    {

        public MessagesPage()
        {
            InitializeComponent();
            if (!App.IsCoach)
            {
                addMessageButton.IsVisible = false;
            }
        }

       private async void DeleteHandler(object sender, EventArgs e)
        {
            var Imbutton = sender as ImageButton;
            var response = await DisplayAlert("App warning","Are you sure you want to delete the message?", "yes", "no");
            if (response)
            {
                
                var message = Imbutton?.BindingContext as UIMessage;
                var vm = BindingContext as MessagesViewModel;
                vm?.DeleteCommand.Execute(message);
            }
           
        }

        private async void NewMessageHandler(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewMessagePage());
        }

        private void ListView_OnItemTapped (object sender, ItemTappedEventArgs e)
        {
            var vm = BindingContext as MessagesViewModel;
            var message = e.Item as UIMessage;
            vm.HideOrShowMessage(message);
        }
    }
}