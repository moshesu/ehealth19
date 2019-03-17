using CannaBe.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace CannaBe.AppPages.PostTreatmentPages
{
    public sealed partial class PostTreatment2 : Page
    {
        Dictionary<string, string> questionDictionary = null;
        public PostTreatment2()
        {
            this.InitializeComponent();
            this.FixPageSize();
            PagesUtilities.AddBackButtonHandler((object sender, Windows.UI.Core.BackRequestedEventArgs e) => { });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter == null)
                return;

            questionDictionary = e.Parameter as Dictionary<string, string>;
        }

        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            PagesUtilities.DontFocusOnAnythingOnLoaded(sender, e);
            try
            {
                EnumDescriptions positive = new EnumDescriptions("Would you use this strain again?", "Rate the quality of the treatment:");
                PostQuestions.Items.Add(positive);
                questionDictionary[positive.q1] = "Don't know";
            }

            catch (Exception x)
            {
                AppDebug.Exception(x, "PostTreatment2 => OnPageLoaded");
            }

        }

        private double[] GetRanks(Dictionary<string, string> questionDictionary)
        {
            double positiveSum = 0, medicalSum = 0;
            int positiveCnt = 0, medicalCnt = 0;
            int cnt = questionDictionary.Count;
            int is_blacklist = 0;
            double[] ans = new double[4];

            foreach (KeyValuePair<string, string> question in questionDictionary)
            { // Check each question
                if (question.Key.Equals("Would you use this strain again?") || question.Key.Equals("Rate the quality of the treatment:"))
                { // General questions
                    if (question.Value.Equals("Yes")) positiveSum += 10;
                    else if (question.Value.Equals("No"))
                    { // Add to blacklist
                        positiveSum += 0;
                        is_blacklist = 1;
                    }
                    else if (question.Value.Equals("Don't know")) positiveSum += 5;
                    else positiveSum += System.Convert.ToInt32(question.Value);
                    positiveCnt += 1;
                }
                else { // Medical needs questions
                    if (question.Value.Equals("Yes")) medicalSum += 10;
                    else if (question.Value.Equals("No")) medicalSum += 0;
                    else if (question.Value.Equals("Don't know")) medicalSum += 5;
                    else medicalSum += (10 - System.Convert.ToInt32(question.Value));
                    medicalCnt += 1;

                }
            }
            // Calculate ranks
            ans[0] = medicalSum / medicalCnt;
            ans[1] = positiveSum / positiveCnt;
            ans[2] = ((positiveSum + medicalSum) / cnt);
            ans[3] = is_blacklist;
            return ans;
        }

        private async void SubmitFeedback(object sender, TappedRoutedEventArgs e) // Send feedback to server
        {
            HttpResponseMessage res = null;
            UsageUpdateRequest req;

            double[] ranks = new double[4];

            ranks = GetRanks(questionDictionary);

            try
            { // Build request for server
                progressRing.IsActive = true;
                GlobalContext.CurrentUser.UsageSessions.LastOrDefault().usageFeedback = questionDictionary;

                UsageData use = GlobalContext.CurrentUser.UsageSessions.LastOrDefault();
                string userId = GlobalContext.CurrentUser.Data.UserID;

                req = new UsageUpdateRequest(use.UsageStrain.Name, use.UsageStrain.ID,
                    userId, 
                    ((DateTimeOffset)use.StartTime).ToUnixTimeMilliseconds(),
                    ((DateTimeOffset)use.EndTime).ToUnixTimeMilliseconds(),
                    ranks[0], ranks[1], ranks[2], use.HeartRateMax, use.HeartRateMin, (int)use.HeartRateAverage, ranks[3],
                    questionDictionary);

                res = await HttpManager.Manager.Post(Constants.MakeUrl("usage"), req); // Send request

                if (res != null)
                { // Request sent successfully
                    if (res.IsSuccessStatusCode)
                    {
                        Status.Text = "Usage update Successful!";
                        var index = GlobalContext.CurrentUser.UsageSessions.Count - 1;
                        GlobalContext.CurrentUser.UsageSessions[index].UsageId = res.GetContent()["body"];
                        PagesUtilities.SleepSeconds(0.5);
                        Frame.Navigate(typeof(DashboardPage));
                    }
                    else
                    {
                        Status.Text = "Usage update failed! Status = " + res.StatusCode;
                    }
                }
                else
                {
                    Status.Text = "Usage update failed!\nPost operation failed";
                }

                Frame.Navigate(typeof(DashboardPage));
            }
            catch(Exception x)
            {
                AppDebug.Exception(x, "UsageUpdate");
            }
        }

        private void Answers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var check = sender as ComboBox;
            questionDictionary[check.Tag.ToString()] = check.SelectedValue.ToString();
        }
    }
}
