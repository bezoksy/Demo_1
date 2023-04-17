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
using System.Windows.Threading;

namespace DemoExam.PagesApp
{
    /// <summary>
    /// Interaction logic for RecordsPage.xaml
    /// </summary>
    public partial class RecordsPage : Page
    {
        private DispatcherTimer _dispatcherTimer;

        public RecordsPage()
        {
            InitializeComponent();
        }

        private void UpdateRecords()
        {
            lvRecords.ItemsSource = App.Connection
                .ClientService
                .ToList()
                .Where(x => x.StartTime >= DateTime.Now && x.StartTime <= DateTime.Now.AddDays(2))
                .ToList();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateRecords();
            StartTimer();
        }

        private void StartTimer()
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromSeconds(30);
            _dispatcherTimer.Tick += TimerTick;
            _dispatcherTimer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            UpdateRecords();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            TimerStop();
        }

        private void TimerStop()
        {
            _dispatcherTimer?.Stop();
            _dispatcherTimer = null;
        }
    }
}
