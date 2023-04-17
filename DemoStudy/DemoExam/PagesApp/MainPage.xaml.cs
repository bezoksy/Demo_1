using System;
using System.Collections.Generic;
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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            pageMainFrame.Navigate(new ServicesPage());
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (pageMainFrame.NavigationService.CanGoBack)
            {
                pageMainFrame.NavigationService.GoBack();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var path = @"C:\Users\Пользователь\source\repos\DemoExam\DemoExam\";
            foreach(var item in App.Connection.Service.ToArray().Where(i => !string.IsNullOrWhiteSpace(i.MainImagePath)))
            {
                var full = path + item.MainImagePath;
                var byteImg = File.ReadAllBytes(full);
                item.MainImage = byteImg;
            }

            App.Connection.SaveChanges();
        }
    }
}
