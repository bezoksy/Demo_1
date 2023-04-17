using DemoExam.ADOApp;
using DemoExam.Components;
using DemoExam.WindowsApp;
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

namespace DemoExam.PagesApp
{
    /// <summary>
    /// Interaction logic for ServicesPage.xaml
    /// </summary>
    public partial class ServicesPage : Page
    {
        private List<Service> _services;
        private List<Service> _sorted;

        private Predicate<Service> _filterQuery = x => true;
        private Func<Service, object> _sortQuery = x => x.ID;

        public ServicesPage()
        {
            InitializeComponent();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortingMethodChange();
        }

        private void tbSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search();
        }

        private void FilterAndSort()
        {
            _sorted = _services.Where(x => _filterQuery(x)).OrderBy(x => _sortQuery(x)).ToList();
            lvServices.ItemsSource = _sorted;

            if (tbSearchBar.Text != "")
            {
                Search();
            }

            UpdateRecordsCount();
        }

        private void Search()
        {
            lvServices.ItemsSource = _sorted
                .Where(x => x.Title.ToLower()
                .Contains(tbSearchBar.Text.ToLower()))
                .ToList();
            UpdateRecordsCount();
        }

        private void SortingMethodChange()
        {
            switch (cbSort.SelectedIndex)
            {
                case 0:
                    _sortQuery = x => x.ID;
                    break;
                case 1:
                    _sortQuery = x => x.CostWithDiscount;
                    break;
                case 2:
                    _sortQuery = x => -x.CostWithDiscount;
                    break;
            }
            FilterAndSort();
        }

        private void FilterMethodChange()
        {
            switch (cbFilter.SelectedIndex)
            {
                case 0:
                    _filterQuery = x => true;
                    break;
                case 1:
                    _filterQuery = x => x.Discount >= 0 && x.Discount < 5;
                    break;
                case 2:
                    _filterQuery = x => x.Discount >= 5 && x.Discount < 15;
                    break;
                case 3:
                    _filterQuery = x => x.Discount >= 15 && x.Discount < 30;
                    break;
                case 4:
                    _filterQuery = x => x.Discount >= 30 && x.Discount < 70;
                    break;
                case 5:
                    _filterQuery = x => x.Discount >= 70 && x.Discount < 100;
                    break;
                default:
                    _filterQuery = x => true;
                    break;
            }

            FilterAndSort();
        }

        private void UpdateRecordsCount()
        {
            tbRecordsCount.Text = $"{lvServices.Items.Count} из {_services.Count()}";
        }

        private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterMethodChange();
        }

        private void btnDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            int serviceId = (int)((Button)sender).Tag;

            var res = App.Connection.ClientService.Any(x => x.ServiceID == serviceId);

            if (res)
            {
                MessageBox.Show("Удаление не возможно, так как существуют записи", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("Успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            int serviceId = (int)((Button)sender).Tag;
            var editService = App.Connection.Service.FirstOrDefault(x => x.ID == serviceId);

            NavigationService.Navigate(new ServicePage(editService));
            UpdateServices();
        }

        private void UpdateServices()
        {
            _services = App.Connection.Service.ToList();
            lvServices.ItemsSource = _services;
            FilterAndSort();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ServicePage(null));
        }

        private void btnAdminMode_Click(object sender, RoutedEventArgs e)
        {
            if (App.IsAdministratorMode)
            {
                App.IsAdministratorMode = false;
            }
            else
            {
                var window = new PasswordWindow();
                window.ShowDialog();
            }
            
            btnCreate.Visibility = App.IsAdministratorMode ? Visibility.Visible : Visibility.Collapsed;
            btnRecordsList.Visibility = App.IsAdministratorMode ? Visibility.Visible : Visibility.Collapsed;

            UpdateServices();
            ChangeAdminModeBtn();
        }

        private void ChangeAdminModeBtn()
        {
            btnAdminMode.Content = App.IsAdministratorMode ? "Выйти" : "Режим администратора";
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _services = App.Connection.Service.ToList();
            lvServices.ItemsSource = _services;

            cbSort.ItemsSource = SortingMethods.Methods;
            cbFilter.ItemsSource = FilterMethods.Methods;
            cbSort.SelectedIndex = 0;
            cbFilter.SelectedIndex = 0;

            btnCreate.Visibility = App.IsAdministratorMode ? Visibility.Visible : Visibility.Collapsed;
            btnRecordsList.Visibility = App.IsAdministratorMode ? Visibility.Visible : Visibility.Collapsed;
            UpdateRecordsCount();
            UpdateServices();
        }

        private void btnRecordsList_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RecordsPage());
        }

        private void lvServices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (App.IsAdministratorMode)
            {
                NavigationService.Navigate(new AddClientRecordPage((Service) lvServices.SelectedItem));
            }
        }
    }
}
