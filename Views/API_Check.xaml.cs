using System.Text;
using System.Windows;
using DotNetEnv;
using Str = child13.Resources.Localization.Strings;
using System.Globalization;
using System.Net.Http;
using child13.Models;
using Newtonsoft.Json;

namespace child13.Views
{
    /// <summary>
    /// Interaction logic for API_Check.xaml
    /// </summary>
    public partial class API_Check : Window
    {
        static readonly HttpClient client = new HttpClient();
        public CultureInfo _culture;
        private string serverUrl;
        public API_Check()
        {
            Env.Load(".env");
            InitializeComponent();
            UpdateUI();
            serverUrl = Environment.GetEnvironmentVariable("child13_API_KEY");

            if (string.IsNullOrEmpty(serverUrl))
            {
                MessageBox.Show("Server URL is not set. Please check your .env file.");
            }
            else if (!Uri.IsWellFormedUriString(serverUrl, UriKind.Absolute))
            {
                MessageBox.Show("Invalid server URL in .env file.");
            }
        }

        #region Localization
        private void UpdateUI()
        {
            _lblApi.Content = Str._lblApi;
            _txtAPICheck.Text = Str._txtAPICheck;
            _btnApi.Content = Str._btnApi;
        }
        #endregion

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var authRequest = new AuthRequest
            {
                Openai_key = _txtAPICheck.Text
            };
            var json = JsonConvert.SerializeObject(authRequest);
            var context = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                if (!Uri.IsWellFormedUriString(serverUrl, UriKind.Absolute))
                {
                    MessageBox.Show("Invalid server URL.");
                    return;
                }
                var response = await client.PostAsync(serverUrl + "/auth", context);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(responseString);
                    IdentityCode._message = result.access_key.ToString();
                    api_Trans._api = _txtAPICheck.Text;
                    this.Close();
                }
                else
                {
                    var errorData = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<dynamic>(errorData);
                    MessageBox.Show("GG");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }
    }
}
