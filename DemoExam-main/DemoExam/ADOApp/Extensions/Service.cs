using System.Windows;
using System.Windows.Media;

namespace DemoExam.ADOApp
{
    public partial class Service
    {
        public Visibility IsHaveDiscount => Discount > 0 ? Visibility.Visible : Visibility.Collapsed;
        public decimal CostWithDiscount => Discount > 0 ? Cost - Cost * (decimal)Discount / 100 : Cost;
        public string DurationInMinutes
        {
            get => DurationInSeconds == 0 ? "" : $"{DurationInSeconds / 60} минут";
            set => DurationInSeconds = int.Parse(value) * 60;
        }
        public string DiscountDisplay => Discount == null || Discount == 0 ? "" : $"* скидка {Discount}%";
        public string ImagePath => $@"/{MainImagePath}";
        public Visibility AdminVisibility => App.IsAdministratorMode ? Visibility.Visible : Visibility.Collapsed;
        public Brush ServiceBackgroundColor => Discount > 0 ? Brushes.LightGreen : Brushes.Transparent;
    }
}
