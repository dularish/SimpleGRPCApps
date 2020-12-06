using Grpc.Core;
using Grpc.Net.Client;
using ScoreUpdaterConsole.ViewModels;
using SimpleGRPCService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
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

namespace ScoreUpdaterConsole
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ScoreboardVM _dataContext;
        private GrpcChannel _channel;
        //private Greeter.GreeterClient _client2;

        private CricketScoreUpdater.CricketScoreUpdaterClient _client;

        public MainWindow()
        {
            InitializeComponent();
            _dataContext = new ScoreboardVM();
            DataContext = _dataContext;

            string host = "localhost";
            string port = "5001";
            var channelTarget = $"https://{host}:{port}";
            _channel = GrpcChannel.ForAddress(channelTarget);
            _client = new CricketScoreUpdater.CricketScoreUpdaterClient(_channel);
            //_client2 = new Greeter.GreeterClient(_channel);
        }

        private void _updateScoresBtn_Click(object sender, RoutedEventArgs e)
        {
            uint.TryParse(_runsScoredOffBallBox.Text, out uint runsOffBall);
            bool isWicketLost = _wicketLost.IsChecked.HasValue ? _wicketLost.IsChecked.Value : false;
            var updatedScores = _client.UpdateBallInfo(new BallUpdateRequest() { RunsOffBall = runsOffBall, Wickets = isWicketLost ? (uint)1 : 0 });
            _dataContext.BallsBowled = (int)updatedScores.TotalBallsBowled;
            _dataContext.RunsScored = (int)updatedScores.Runs;
            _dataContext.WicketsLost = (int)updatedScores.Wickets;
            //var response = _client2.SayHello(new HelloRequest() { Name = "Dularish" });
            _wicketLost.IsChecked = false;
            _runsScoredOffBallBox.Text = "0";
        }
    }
}
