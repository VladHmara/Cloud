using CloudClient;
using DataCloud.Model;
using DataToCommunicate;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security;
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
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private async void Btn_SignUp_Click(object sender, RoutedEventArgs e)
        {
            string password1 = pb_Password1.Password;
            string password2 = pb_Password2.Password;

            if (password1.Equals(password2))
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:51769");
                HttpResponseMessage signUpResponse = await client.PostAsJsonAsync("/api/Cloud/SignUp", new RegistrationData { UserName = tb_UserName.Text, Password = password1 });
                if (signUpResponse.IsSuccessStatusCode)
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/Token");

                    var keyValues = new List<KeyValuePair<string, string>>();
                    keyValues.Add(new KeyValuePair<string, string>("grant_type", "password"));
                    keyValues.Add(new KeyValuePair<string, string>("username", tb_UserName.Text));
                    keyValues.Add(new KeyValuePair<string, string>("password", password1));

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
                else
                    MessageBox.Show(signUpResponse.StatusCode.ToString() + signUpResponse.RequestMessage.ToString(), "Ошибка запроса");
            }
            else
                MessageBox.Show("Не совпадают пароли", "Ошибка");
        }

        private async void Btn_SignInWindow_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow authorizationWindow = new AuthorizationWindow();
            authorizationWindow.Show();
            this.Close();
        }
    }
}
