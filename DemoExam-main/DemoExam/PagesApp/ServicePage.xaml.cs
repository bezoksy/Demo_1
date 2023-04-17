using DemoExam.ADOApp;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Globalization;
using System.IO;
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
    /// Interaction logic for ServicePage.xaml
    /// </summary>
    public partial class ServicePage : Page
    {
        private Service _service;
        private bool _isEdit;

        public ServicePage(Service service)
        {
            InitializeComponent();

            _service = service;

            if (_service != null)
            {
                _isEdit = true;
                lbTitle.Content = "Изменение услуги";
            }
            else
            {
                _service = new Service();
                lbTitle.Content = "Добавление услуги";
            }

            this.DataContext = _service;

            tblID.Visibility = _isEdit ? Visibility.Visible : Visibility.Collapsed;
            tbID.Visibility = _isEdit ? Visibility.Visible : Visibility.Collapsed;
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            int duration = 0;

            if (tbDuration.Text == "" || tbDiscount.Text == "" || tbName.Text == "" || tbCost.Text == "")
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(tbDuration.Text, out duration)
                || !double.TryParse(tbDiscount.Text, out double discount)
                || !decimal.TryParse(tbCost.Text.Replace('.', ','), out decimal cost))
            {
                MessageBox.Show("Введите корректные данные (скидка, стоимость, длительность)!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (duration * 1.0 / 3600 > 4)
            {
                MessageBox.Show("Длительность не может быть больше 4 часов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!_isEdit)
            {
                var existService = App.Connection.Service.Any(x => x.Title == _service.Title);

                if (existService)
                {
                    MessageBox.Show("Услуга с таким названием уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            App.Connection.Service.AddOrUpdate(_service);
            App.Connection.SaveChanges();
            MessageBox.Show($"Услуга успешно сохранена!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

            NavigationService.GoBack();
        }

        private void btnMainImage_Click(object sender, RoutedEventArgs e)
        {
            var window = new OpenFileDialog();

            if (window.ShowDialog() is true)
            {
                var byteImg = File.ReadAllBytes(window.FileName);
                _service.MainImage = byteImg;
            }
        }

        private void btnOtherImage_Click(object sender, RoutedEventArgs e)
        {
            var window = new OpenFileDialog();

            if (window.ShowDialog() is true)
            {
                var byteImg = File.ReadAllBytes(window.FileName);
                _service.ServicePhoto.Add(new ServicePhoto() { Image = byteImg, ServiceID = _service.ID, });
                lbImages.ItemsSource = _service.ServicePhoto.ToList();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            lbImages.ItemsSource = _service.ServicePhoto.ToList();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var image = (byte[])((Button)sender).Tag;

            var res = _service.ServicePhoto.FirstOrDefault(x => x.Image == image);

            _service.ServicePhoto.Remove(res);

            lbImages.ItemsSource = _service.ServicePhoto.ToList();

            if (_isEdit)
            {
                var dbImage = App.Connection.ServicePhoto.ToArray().FirstOrDefault(x => x.ID == res?.ID);

                if (dbImage != null)
                {
                    App.Connection.ServicePhoto.Remove(dbImage);
                    App.Connection.SaveChanges();
                }
            }
        }
    }
}
