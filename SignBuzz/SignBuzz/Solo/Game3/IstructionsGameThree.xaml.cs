using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SignBuzz.Solo.Game3
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IstructionsGameThree : ContentPage
	{
		public IstructionsGameThree ()
		{
            InitializeComponent();

        }
        async void onClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Game3.TabbedPage2());
        }
    }
}