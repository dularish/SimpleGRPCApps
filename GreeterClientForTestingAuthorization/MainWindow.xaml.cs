using GreeterServiceForTestingAuthorization;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GreeterClientForTestingAuthorization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Greeter.GreeterClient _client = null;
        private string _authToken;

        public Greeter.GreeterClient Client
        {
            get
            {
                if(_client == null)
                {
                    var channel = GrpcChannel.ForAddress("http://localhost:5000");
                    _client = new Greeter.GreeterClient(channel);
                }

                return _client;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }


        private async void safeHelloBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var response = await Client.SayEasyHelloAsync(new HelloRequest { Name = "WPF" });
                MessageBox.Show(response.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception : {ex.Message}");
            }
        }

        private async void authorizedHelloBtn_Click(object sender, RoutedEventArgs e)
        {
            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {_authToken}");
            try
            {
                var response = await Client.SayAuthorizedHelloAsync(new HelloRequest { Name = "WPF" }, headers);
                MessageBox.Show(response.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception : {ex.Message}");
            }
        }

        private async void getAuthTokenBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var response = await Client.GetAuthTokenAsync(new TokenRequest() { Username = "testUserId", Password = "testPassword" });
                if (response.Result)
                {
                    MessageBox.Show("Received a valid token");
                    _authToken = response.TokenString;
                }
                else
                {
                    MessageBox.Show("Did not receive a valid token", "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception : {ex.Message}");
            }
        }

        private async void getAdminAuthTokenBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var response = await Client.GetAuthTokenAsync(new TokenRequest() { Username = "testAdminUserId", Password = "testAdminPassword" });
                if (response.Result)
                {
                    MessageBox.Show("Received a valid token");
                    _authToken = response.TokenString;
                }
                else
                {
                    MessageBox.Show("Did not receive a valid token", "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception : {ex.Message}");
            }
        }

        private async void adminAuthorizedHelloBtn_Click(object sender, RoutedEventArgs e)
        {
            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {_authToken}");
            try
            {
                var response = await Client.SayAdminAuthorizedHelloAsync(new HelloRequest { Name = "WPF" }, headers);
                MessageBox.Show(response.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception : {ex.Message}");
            }
        }
    }
}
