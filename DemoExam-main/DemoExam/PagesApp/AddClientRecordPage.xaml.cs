using DemoExam.ADOApp;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace DemoExam.PagesApp
{
    /// <summary>
    /// Interaction logic for AddClientRecordPage.xaml
    /// </summary>
    public partial class AddClientRecordPage : Page
    {
        private Service _currentService = new Service();

        public AddClientRecordPage(Service service)
        {
            InitializeComponent();
            _currentService = service;

            this.DataContext = _currentService;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            
            if (tbTimeStart.Text == "" || dpDate.SelectedDate == null || cbClients.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            TimeSpan startTime;

            if (!TimeSpan.TryParse(tbTimeStart.Text, out startTime))
            {
                MessageBox.Show("Введите корректное время!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newClientService = new ClientService
            {
                Client = (Client)cbClients.SelectedItem,
                Service = _currentService,
                StartTime = dpDate.SelectedDate.Value + startTime,
                Comment = tbComment.Text
            };

            App.Connection.ClientService.Add(newClientService);
            App.Connection.SaveChanges();
            MessageBox.Show("Запись успешно добавлена!", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);

            NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateClients();
        }

        private void UpdateClients()
        {
            cbClients.ItemsSource = App.Connection.Client.ToList();
        }
    }
}
