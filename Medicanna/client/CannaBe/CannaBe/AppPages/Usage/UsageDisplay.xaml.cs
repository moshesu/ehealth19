using System;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace CannaBe.AppPages.Usage
{
    public sealed partial class UsageDisplay : Page
    {
        public UsageDisplay()
        {
            InitializeComponent();
            PagesUtilities.AddBackButtonHandler((object sender, Windows.UI.Core.BackRequestedEventArgs e) => 
            {
                GoBack(null, null);
            });

        }
            private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            var u = UsageContext.DisplayUsage;

            if (u != null)
            { // Display usage
                StrainChosenText.Text = u.UsageStrain.Name;
                var st_u = new Underline();
                st_u.Inlines.Add(new Run()
                { // Start time
                    FontWeight = FontWeights.Bold,
                    Text = "Start Time"
                });
                StartTime.Inlines.Add(st_u);
                StartTime.Inlines.Add(new Run()
                { // With date
                    FontSize = 18,
                    Text = ": " + u.StartTime.ToString("dd.MM.yy HH:mm:ss")
                });
                var et_u = new Underline();
                et_u.Inlines.Add(new Run()
                { // End time
                    FontWeight = FontWeights.Bold,
                    Text = "End Time"
                });
                EndTime.Inlines.Add(et_u);
                EndTime.Inlines.Add(new Run()
                { // With date
                    FontSize = 18,
                    Text = ": " + u.EndTime.ToString("dd.MM.yy HH:mm:ss")
                });
                var du_u = new Underline();
                du_u.Inlines.Add(new Run()
                {
                    FontWeight = FontWeights.Bold,
                    Text = "Duration"
                });
                Duration.Inlines.Add(du_u);
                Duration.Inlines.Add(new Run()
                { // With duration display
                    FontSize = 18,
                    Text = ": " + u.DurationString
                });

                if (!u.UseBandData)
                {
                    HeartRate.Text = "Band was not used";
                }
                else
                { // Band was used, show heart rate
                    Underline HeartRateUnderline = new Underline();
                    HeartRateUnderline.Inlines.Add(new Run()
                    {
                        FontWeight = FontWeights.Bold,
                        Text = "Heartrate"
                    });
                    HeartRate.Inlines.Add(HeartRateUnderline);
                    HeartRate.Inlines.Add(new Run()
                    {
                        Text = ": "
                    });
                    HeartRate.Inlines.Add(new Run()
                    { // Minimu heart rate
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        Text = "Min: "
                    });

                    HeartRate.Inlines.Add(new Run()
                    {
                        FontSize = 18,
                        Text = u.HeartRateMin.ToString()
                    });

                    HeartRate.Inlines.Add(new Run()
                    { // Average heart rate
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        Text = " Avg: "
                    });

                    HeartRate.Inlines.Add(new Run()
                    {
                        FontSize = 18,
                        Text = Math.Round(u.HeartRateAverage, 0).ToString()
                    });

                    HeartRate.Inlines.Add(new Run()
                    { // Maximum heart rate
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        Text = " Max: "
                    });

                    HeartRate.Inlines.Add(new Run()
                    {
                        FontSize = 18,
                        Text = u.HeartRateMax.ToString()
                    });

                }

                if (u.usageFeedback != null)
                { // Display post treatment feedback
                    foreach (var q in u.usageFeedback)
                    {
                        var t = new TextBlock()
                        {
                            Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Black),
                            FontSize = 18,
                            TextWrapping = TextWrapping.Wrap,
                            VerticalAlignment = VerticalAlignment.Top,
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                        };
                        t.Inlines.Add(new Run()
                        {
                            FontWeight = FontWeights.Bold,
                            Text = q.Key + " "
                        });
                        t.Inlines.Add(new Run()
                        {
                            Text = q.Value
                        });
                        //t.Inlines.Add(new LineBreak());

                        Questions.Children.Add(t);
                    }
                }
                else
                {
                    Scroller.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void GoBack(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            UsageContext.DisplayUsage = null;
            Frame.Navigate(typeof(UsageHistory));
        }
    }
}
