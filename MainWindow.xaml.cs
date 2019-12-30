using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;
using MingweiSamuel.Camille;
using MingweiSamuel.Camille.SummonerV4;
using MingweiSamuel.Camille.SpectatorV4;
using MingweiSamuel.Camille.Enums;

namespace NoGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        TimeSpan time;
        public MainWindow()
        {
            InitializeComponent();
        }

        String path;
        String summoner;
        double hours;
        String key = "RGAPI-1a15a384-ba51-464a-9a31-3c1f5a526ed2";

        void OpenSettings(object sender, RoutedEventArgs e)
        {
            Settings set = new Settings();
            set.Owner = this;

            if (set.ShowDialog() == true)
            {
                path = set.Path;
                hours = set.Hours;
                summoner = set.Summoner;
            }
        }


        void Play(object sender, RoutedEventArgs e)
        {

            // Run game
            Process.Start(path);

            var riotApi = RiotApi.NewInstance(key);
            var player = riotApi.SummonerV4.GetBySummonerName(Region.NA, summoner);
            var game = riotApi.SpectatorV4.GetCurrentGameInfoBySummoner(Region.NA, player.Id);


            time = TimeSpan.FromSeconds(30);

            timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                lblTime.Content = time.ToString("c");

                if ((time == TimeSpan.Zero) && (game == null))
                {
                    timer.Stop();


                    KillLeague();
                }
                time = time.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);
            timer.Start();


        }

        public static void KillLeague()
        {
            System.Diagnostics.Process[] procs = null;
            try
            {
                procs = Process.GetProcessesByName("LeagueClientUx");

                Process LeagueClientUxProc = procs[0]; 

                if (!LeagueClientUxProc.HasExited)
                {
                    LeagueClientUxProc.Kill();
                }
            }
            finally
            {
                if (procs != null)
                {
                    foreach (Process p in procs)
                        p.Dispose();
                }

            }




        }
    }
}

