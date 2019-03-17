using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace CannaBe.AppPages.InformationPages
{
    public sealed partial class MedicalInformationPage : Page
    {
        Dictionary<string, string> doctors = null;
        List<string> doctorNames = null;
        List<string> cities = new List<string>();
        List<string> medicalCenters = new List<string>();
        private static readonly string NoResult = "No Result";
        string doctorChosen = null;

        public MedicalInformationPage()
        {
            this.InitializeComponent();
            this.FixPageSize();
            PagesUtilities.AddBackButtonHandler();
        }

        private void BoxGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBoxSender = sender as TextBox;

            if (textBoxSender.Text == ("Enter " + textBoxSender.Name))
            {
                textBoxSender.Text = "";
                textBoxSender.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Black);
            }
        }

        private void BoxLostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBoxSender = sender as TextBox;

            if (textBoxSender.Text.Length == 0)
            {
                textBoxSender.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Black);
                textBoxSender.Text = "Enter " + textBoxSender.Name;

            }
        }


        public void OnPageLoaded(object sender, RoutedEventArgs e) // Import doctors list into objects
        {
            string[] data;
            AppDebug.Line("Loading doctors list..");
            try
            {

                doctors = File.ReadAllLines("Assets/doctors.txt") // Read into dictionary, doctor name as key
                                        .Select(a => a.Split(' '))
                                        .ToDictionary(x => x[0].Replace('-', ' ').Replace('_', ' '),
                                                        x => x[1].Replace('-', ' '));

                doctorNames = doctors.Keys.ToList();
                foreach (string val in doctors.Values)
                {
                    data = val.Split('_'); // Split into city and medical center
                    if (!medicalCenters.Contains(data[0].Replace('-', ' '))) medicalCenters.Add(data[0].Replace('-', ' '));
                    if (!cities.Contains(data[1].Replace('-', ' '))) cities.Add(data[1].Replace('-', ' '));
                }

                AppDebug.Line($"loaded {doctorNames.Count} doctors");

            }
            catch (Exception exc)
            {
                AppDebug.Exception(exc, "LoadDoctorsList");
            }

        }


        private void DoctorsList_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Only get results when it was a user typing,
            // otherwise assume the value got filled in by TextMemberPath
            // or the handler for SuggestionChosen.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                //Set the ItemsSource to be your filtered dataset
                //sender.ItemsSource = dataset;

                var lst = doctorNames.Where(item => item.ToLower().Contains(sender.Text.ToLower())).ToList();
                lst.Sort();
                if (lst.Count() == 0)
                {
                    lst.Add(NoResult);
                }

                sender.ItemsSource = lst;
            }
        }

        private void CityList_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var lst = cities.Where(item => item.ToLower().Contains(sender.Text.ToLower())).ToList();
                lst.Sort();
                if (lst.Count() == 0)
                {
                    lst.Add(NoResult);
                }

                sender.ItemsSource = lst;
            }
        }
        private void MedicalCenterList_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var lst = medicalCenters.Where(item => item.ToLower().Contains(sender.Text.ToLower())).ToList();
                lst.Sort();
                if (lst.Count() == 0)
                {
                    lst.Add(NoResult);
                }

                sender.ItemsSource = lst;
            }
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var item = args.SelectedItem.ToString();

            if (item != NoResult)
            {
                AppDebug.Line($"AutoSuggestBox_SuggestionChosen [{item}]");

                sender.Text = item;
            }
            else
            {
                sender.Text = "";
            }
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var item = args.ChosenSuggestion;

            if (item != null)
            {
                var itemString = item.ToString();

                if (itemString != NoResult)
                {
                    ErrorNoDoctorChosen.Visibility = Visibility.Collapsed;

                    AppDebug.Line($"AutoSuggestBox_QuerySubmitted [{item}]");

                    sender.Text = itemString;
                    doctorChosen = itemString;
                }
                else
                {
                    sender.Text = "";
                }
            }
        }
        private void SubmitDoctorButton_Click(object sender, RoutedEventArgs e)
        {
            doctor_chosen.Text = "";
            doctor_data.Text = "";
            CityList.Text = "";
            MedicalCenterList.Text = "";

            string data = "";
            string name = DoctorsList.Text;
            if (!string.IsNullOrEmpty(name)) // Doctor name entered
            {
                try
                { // Display city and medical center for selected doctor
                    doctor_chosen.Text = name;
                    doctors.TryGetValue(name, out data);
                    string[] data_split = data.Split('_');
                    doctor_data.Text = "Medical Center: " + data_split[0] + "\nCity: " + data_split[1];
                }
                catch
                { // Invalid name
                    doctor_chosen.Text = "Please enter a valid doctor name";
                }
            }

        }
        private void SubmitCityButton_Click(object sender, RoutedEventArgs e)
        {
            doctor_chosen.Text = "";
            doctor_data.Text = "";
            DoctorsList.Text = "";
            MedicalCenterList.Text = "";

            try
            {
                string city = CityList.Text;
                if (!string.IsNullOrEmpty(city))
                { // Display doctors and medical centers for chosen city

                    string data = "";
                    int i = 1;

                    doctor_chosen.Text = city + ":";
                    foreach (string key in doctors.Keys)
                    { // Check doctors that operate in chosen city
                        doctors.TryGetValue(key, out data);
                        if (data.Contains(city))
                        { // Display
                            string[] data_split = data.Split('_');
                            doctor_data.Text += i + ". " + key + " at " + data_split[0] + " medical center\n";
                            i++;
                        }
                    }
                }
            }
            catch
            {
                doctor_chosen.Text = "Please enter a valid city name";
            }
        }

        private void SubmitMedicalCenterButton_Click(object sender, RoutedEventArgs e)
        {
            doctor_chosen.Text = "";
            doctor_data.Text = "";
            DoctorsList.Text = "";
            CityList.Text = "";

            try
            {
                string medicalCenter = MedicalCenterList.Text;
                if (!string.IsNullOrEmpty(medicalCenter))
                { // Display doctors and cities for medical center chosen
                    string data = "";
                    int i = 1;

                    doctor_chosen.Text = medicalCenter;
                    foreach (string key in doctors.Keys)
                    { // Check doctors for chosen medical center
                        doctors.TryGetValue(key, out data);
                        if (data.Contains(medicalCenter))
                        { // Display
                            string[] data_split = data.Split('_');
                            doctor_data.Text += i + ". " + key + ", located at: " + data_split[1] + "\n";
                            i++;

                        }
                    }
                }
            }
            catch
            {
                doctor_chosen.Text = "Please enter a valid medical center name";
            }
        }


        private void BackToInformationPage(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(InformationPage));
        }

        private void GoToDashboard(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(DashboardPage));
        }

    }
}
