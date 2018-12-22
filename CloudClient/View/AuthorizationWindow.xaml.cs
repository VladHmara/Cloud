using CloudClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DataCloud.View
{
    /// <summary>
    /// Interaction logic for AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        private async void Btn_SignIn_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:51769");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/Token");

            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("grant_type", "password"));
            keyValues.Add(new KeyValuePair<string, string>("username", tb_UserName.Text));
            keyValues.Add(new KeyValuePair<string, string>("password", pb_Password.Password));

            request.Content = new FormUrlEncodedContent(keyValues);
            HttpResponseMessage signInResponse = await client.SendAsync(request);
            if (signInResponse.IsSuccessStatusCode)
            {
                string responseJson = await signInResponse.Content.ReadAsStringAsync();
                dynamic responseData = JsonConvert.DeserializeObject(responseJson);
                string token = responseData.access_token;

                MainWindow mainWindow = new MainWindow(token);
                mainWindow.Show();
                this.Close();
            }
            else
                MessageBox.Show(signInResponse.StatusCode.ToString() + signInResponse.RequestMessage.ToString(), "Ошибка запроса");
        }

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
            this.Close();
        }
    }
}
