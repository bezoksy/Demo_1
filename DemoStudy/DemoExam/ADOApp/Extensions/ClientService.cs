using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DemoExam.ADOApp
{
    public partial class ClientService
    {
        public TimeSpan TimeToStart => StartTime - DateTime.Now;
        public string TimeToStartStr => $"{TimeToStart.Hours} часов {TimeToStart.Minutes} минут";
        public string StartTimeStr => StartTime.ToString("f");
        public Brush TimeToStartForeground => TimeToStart.TotalHours < 1 ? Brushes.Red : Brushes.Black;
    }
}
