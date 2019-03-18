using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace eHealthWorkshopGroup4.Models
{
    /// <summary>
    /// This is the common code for the main page.
    /// </summary>
    class MSHelath
    { 
            private LiveIdCredentials creds = new LiveIdCredentials();
            private string responseText { get; set; }
            //
            // The ClientId and ClientSecret are obtained from:
            // https://account.live.com/developers/applications
            //
            // See the conceptual document for the Microsoft Health Service for more information
            // about this topic.
            //
            private const string ClientId = "a965a101-aa3c-479c-aa18-7a308a850315";
            //
            // Choose the minimum set of authorization scopes required by your application.
            //
        private const string Scopes = "mshealth.ReadActivityHistory mshealth.ReadProfile";

            private const string BaseHealthUri = "https://api.microsofthealth.net/v1/me/";
            private const string RedirectUri = "https://login.live.com/oauth20_desktop.srf";

            #region API requests

            private async void getProfileButton_Click(object sender, EventArgs e)
            {
                await this.PerformRequest("Profile");
            }

            private async void getDevicesButton_Click(object sender, EventArgs e)
            {
                await this.PerformRequest("Devices");
            }

            private async void getDailySummaryButton_Click(object sender, EventArgs e)
            {
                string startTime = Uri.EscapeDataString(DateTime.UtcNow.AddDays(-1).ToString("O"));
                string endtime = Uri.EscapeDataString(DateTime.UtcNow.ToString("O"));

                await this.PerformRequest(
                    "Summaries/Daily",
                    string.Format("startTime={0}&endTime={1}", startTime, endtime));
            }

            private async void getHourlySummaryButton_Click(object sender, EventArgs e)
            {
                string startTime = Uri.EscapeDataString(DateTime.UtcNow.AddDays(-1).ToString("O"));
                string endTime = Uri.EscapeDataString(DateTime.UtcNow.ToString("O"));

                await this.PerformRequest(
                    "Summaries/Hourly",
                    string.Format("startTime={0}&endTime={1}", startTime, endTime));
            }

            private async void getAcitvitiesButton_Click(object sender, EventArgs e)
            {
                string startTime = Uri.EscapeDataString(DateTime.UtcNow.AddDays(-29).ToString("O"));
                string endtime = Uri.EscapeDataString(DateTime.UtcNow.ToString("O"));

                await this.PerformRequest(
                    "Activities",
                    string.Format("startTime={0}&endTime={1}", startTime, endtime));
            }

            private async Task PerformRequest(string relativePath, string queryParams = null)
            {
                try
                {
                    this.responseText = string.Empty;

                    var uriBuilder = new UriBuilder(BaseHealthUri);

                    uriBuilder.Path += relativePath;

                    uriBuilder.Query = queryParams;

                    var request = HttpWebRequest.Create(uriBuilder.Uri);

                    request.Headers[HttpRequestHeader.Authorization] = string.Format("bearer {0}", this.creds.AccessToken);

                    using (var response = await request.GetResponseAsync())
                    {
                        using (var stream = response.GetResponseStream())
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                this.responseText = reader.ReadToEnd();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.responseText = string.Format("There was an error requesting the request. {0}", ex.Message);
                }
            }

            #endregion

            #region Sign-in and sign-out

            private void signinButton_Click(object sender, EventArgs e)
            {
                UriBuilder uri = new UriBuilder("https://login.live.com/oauth20_authorize.srf");
                var query = new StringBuilder();

                query.AppendFormat("redirect_uri={0}", Uri.EscapeDataString(RedirectUri));
                query.AppendFormat("&client_id={0}", Uri.EscapeDataString(ClientId));

                query.AppendFormat("&scope={0}", Uri.EscapeDataString(Scopes));
                query.Append("&response_type=code");

                uri.Query = query.ToString();

                this.webView.Visibility = Visibility.Visible;
                this.webView.Navigate(uri.Uri);
            }

            private void signoutButton_Click(object sender, EventArgs e)
            {
                UriBuilder uri = new UriBuilder("https://login.live.com/oauth20_logout.srf");
                var query = new StringBuilder();

                query.AppendFormat("redirect_uri={0}", Uri.EscapeDataString(RedirectUri));
                query.AppendFormat("&client_id={0}", Uri.EscapeDataString(ClientId));

                uri.Query = query.ToString();

                this.webView.Navigate(uri.Uri);
            }

            private async void WebView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
            {
                //
                // When the web view navigates to our redirect URI, extract the authorization code from
                // the URI and use it to fetch our access token. If no authorization code is present,
                // we're completing a sign-out flow.
                //
                if (args.Uri.LocalPath.StartsWith("/oauth20_desktop.srf", StringComparison.OrdinalIgnoreCase))
                {
                    WwwFormUrlDecoder decoder = new WwwFormUrlDecoder(args.Uri.Query);

                    var code = decoder.FirstOrDefault((entry) => entry.Name.Equals("code", StringComparison.OrdinalIgnoreCase));

                    var error = decoder.FirstOrDefault((entry) => entry.Name.Equals("error", StringComparison.OrdinalIgnoreCase));
                    var errorDesc = decoder.FirstOrDefault((entry) => entry.Name.Equals("error_description", StringComparison.OrdinalIgnoreCase));

                    // Check the code to see if this is sign-in or sign-out
                    if (code != null)
                    {
                        // Hide the browser again, no matter what happened...
                        sender.Visibility = Visibility.Collapsed;

                        if (error != null)
                        {
                            this.responseText.Text = string.Format("{0}\r\n{1}", error.Value, errorDesc.Value);
                            return;
                        }

                        var tokenError = await this.GetToken(code.Value, false);

                        if (string.IsNullOrEmpty(tokenError))
                        {
                            this.responseText.Text = "Successful sign-in!";
                            this.signoutButton.IsEnabled = true;
                            this.signinButton.IsEnabled = false;
                            this.getProfileButton.IsEnabled = true;
                            this.getDevicesButton.IsEnabled = true;
                            this.getActivitiesButton.IsEnabled = true;
                            this.getDailySummaryButton.IsEnabled = true;
                            this.getHourlySummaryButton.IsEnabled = true;
                        }
                        else
                        {
                            this.responseText.Text = tokenError;
                        }
                    }
                    else
                    {
                        this.responseText.Text = "Successful sign-out!";

                        this.signoutButton.IsEnabled = false;
                        this.signinButton.IsEnabled = true;
                        this.getProfileButton.IsEnabled = false;
                        this.getDevicesButton.IsEnabled = false;
                        this.getActivitiesButton.IsEnabled = false;
                        this.getDailySummaryButton.IsEnabled = false;
                        this.getHourlySummaryButton.IsEnabled = false;
                    }
                }
            }

            private async Task<string> GetToken(string code, bool isRefresh)
            {
                UriBuilder uri = new UriBuilder("https://login.live.com/oauth20_token.srf");
                var query = new StringBuilder();

                query.AppendFormat("redirect_uri={0}", Uri.EscapeDataString(RedirectUri));
                query.AppendFormat("&client_id={0}", Uri.EscapeDataString(ClientId));
                if (isRefresh)
                {
                    query.AppendFormat("&refresh_token={0}", Uri.EscapeDataString(code));
                    query.Append("&grant_type=refresh_token");
                }
                else
                {
                    query.AppendFormat("&code={0}", Uri.EscapeDataString(code));
                    query.Append("&grant_type=authorization_code");
                }

                uri.Query = query.ToString();

                var request = WebRequest.Create(uri.Uri);

                try
                {
                    using (var response = await request.GetResponseAsync())
                    {
                        using (var stream = response.GetResponseStream())
                        {
                            using (var streamReader = new StreamReader(stream))
                            {
                                var responseString = streamReader.ReadToEnd();
                                var jsonResponse = JObject.Parse(responseString);
                                this.creds.AccessToken = (string)jsonResponse["access_token"];
                                this.creds.ExpiresIn = (long)jsonResponse["expires_in"];
                                this.creds.RefreshToken = (string)jsonResponse["refresh_token"];
                                string error = (string)jsonResponse["error"];

                                return error;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }

            #endregion
        }
    }
