using DemoExam.ADOApp;
using System.Windows;

namespace DemoExam
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly SessionOneEntities _connection = new SessionOneEntities();
        public static SessionOneEntities Connection => _connection;
        public static bool IsAdministratorMode { get; set; } = false;
        public static Visibility AdminVisibility => IsAdministratorMode ? Visibility.Visible : Visibility.Collapsed;
    }
}
