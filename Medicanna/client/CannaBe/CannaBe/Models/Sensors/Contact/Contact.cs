using Microsoft.Band;
using Microsoft.Band.Sensors;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace CannaBe
{
    public class ContactModel : ViewModel
    {
        public delegate void ChangedHandler(BandContactState state);
        public event ChangedHandler Changed;

        private BandContactState _state;

        public BandContactState State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        public async Task InitAsync()
        {
            _state = BandContactState.NotWorn;

            if (BandModel.IsConnected)
            {
                if (BandModel.BandClient.SensorManager.Contact.GetCurrentUserConsent() != UserConsent.Granted)
                {
                    await BandModel.BandClient.SensorManager.Contact.RequestUserConsentAsync();
                }
                BandModel.BandClient.SensorManager.Contact.ReadingChanged += Contact_ReadingChanged;
            }
        }

        public void Start()
        {
            try
            {
                if (BandModel.IsConnected)
                {
                    BandModel.BandClient.SensorManager.Contact.StartReadingsAsync();
                }

            }
            catch (Exception x)
            {
                AppDebug.Line("Exception caught in Start");
                AppDebug.Line(x.Message);
                AppDebug.Line(x.StackTrace);
            }
        }

        public void Stop()
        {
            if (BandModel.IsConnected)
            {
                BandModel.BandClient.SensorManager.Contact.StopReadingsAsync();
            }
        }

        async void Contact_ReadingChanged(object sender, BandSensorReadingEventArgs<IBandContactReading> e)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 () =>
                 {
                     ContactSensorReading reading = new ContactSensorReading { Contact = e.SensorReading.State };
                     if (Changed != null)
                     {
                         AppDebug.Line("Contact_ReadingChanged value<" + reading.Value.ToString() + ">");
                         Changed(reading.Value);
                         State = reading.Value;
                     }
                 });
        }
    }
}
