using GreeterClientForTestingTLSCertificates.Properties;
using GreeterServiceForTestingTLSCertificates;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
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

namespace GreeterClientForTestingTLSCertificates
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Greeter.GreeterClient _client = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        public Greeter.GreeterClient Client
        {
            get
            {
                if(_client == null)
                {
                    var cert = new X509Certificate2(Settings.Default.CertFileName, Settings.Default.CertPassword);

                    var httpClientHandler = new HttpClientHandler();
                    httpClientHandler.ClientCertificates.Add(cert);//TLS HTTPS connection works without even this line, have to know more about this

                    var httpClient = new HttpClient(httpClientHandler);

                    var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions() { HttpClient = httpClient });
                    _client = new Greeter.GreeterClient(channel);
                }
                return _client;
            }
        }

        private async void sayHelloToService_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var response = await Client.SayHelloAsync(new HelloRequest() { Name = "WPF testing for TLS certificates" });
                MessageBox.Show(response.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception : {ex.Message}");
            }
        }
    }
}
