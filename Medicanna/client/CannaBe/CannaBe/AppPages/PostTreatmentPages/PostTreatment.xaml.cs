using CannaBe.Enums;
using System;
using System.Collections.Generic;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace CannaBe.AppPages.PostTreatmentPages
{
    public sealed partial class PostTreatment : Page
    {
        Dictionary<string, string> questionDictionary = new Dictionary<string, string>();
        public PostTreatment()
        {
            this.InitializeComponent();
            this.FixPageSize();
            PagesUtilities.AddBackButtonHandler((object sender, BackRequestedEventArgs e) => { });
        }

        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {

            PagesUtilities.DontFocusOnAnythingOnLoaded(sender, e);
            try
            { // Load questions for user needs
                foreach (var medicalNeed in GlobalContext.CurrentUser.Data.MedicalNeeds)
                {
                    var info = medicalNeed.GetAttribute<EnumDescriptions>();
                    PostQuestions.Items.Add(info); // Add to display
                    questionDictionary[info.q1] = "Don't know";
                }
            }

            catch (Exception x)
            {
                AppDebug.Exception(x, "PostTreatment => OnPageLoaded");
            }
        }

        private void GoToDashboard(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(DashboardPage));
        }

        private int[] GetRanks(Dictionary<string, string> questionDictionary)
        { // Calculate score from answers
            int positiveSum = 0, medicalSum = 0;
            int positiveCnt = 0, medicalCnt = 0;
            int cnt = questionDictionary.Count;
            int is_blacklist = 0;
            int[] ans = new int[4];

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



        private void ContinuePostFeedback(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(PostTreatment2), questionDictionary);
        }

        private void Answers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var check = sender as ComboBox;
            questionDictionary[check.Tag.ToString()] = check.SelectedValue.ToString();
        }
    }
}
