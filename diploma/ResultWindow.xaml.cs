using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Timer = System.Threading.Timer;

namespace diploma
{
    /// <summary>
    /// Логика взаимодействия для ResultWindow.xaml
    /// </summary>
    
    public partial class ResultWindow : Window, INotifyPropertyChanged
    {
        private static string _generated;
        private static string _complite;

        public static string Generated2 { get; set; }
        public static string Complite2 { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public string Generated
        {
            get { return _generated; }
            set
            {
                _generated = Generated2;
                OnPropertyChanged("Generated");
            } 
        }
        public string Complite
        {
            get { return _complite; }
            set
            {
                _complite = Complite2;
                OnPropertyChanged("Complite");
            }
        }
        
        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> XFormatter { get; set; }
        public Func<double, string> YFormatter { get; set; }
        private Thread resultThread;
        private Thread result2Thread;
        private MainWindow mainWindow = null;

        public ResultWindow(MainWindow main)
        {
            InitializeComponent();
            XFormatter = val => TimeSpan.FromMilliseconds(val).ToString();
            YFormatter = val => val.ToString("N");
            DataContext = this;
            resultThread = new Thread(o =>
            {
                while (true)
                {
                    Thread.Sleep(10);
                    Generated = Generated == null
                        ? "1"
                        : (int.Parse(Generated) + 1).ToString();
                    Complite = Complite == null
                        ? "1"
                        : (int.Parse(Complite) + 1).ToString();
                }
            });
            result2Thread = new Thread(o =>
            {
                while (true)
                {
                    Thread.Sleep(2000);
                    MainWindow.SeriesCollection[0].Values.Add(new MainWindow.TimeSpanPoint(TimeSpan.FromMilliseconds(MainWindow.EmulationTime * 10),
                                 GetAverageTime()));
                    MainWindow.SeriesCollection[1].Values.Add(new MainWindow.TimeSpanPoint(TimeSpan.FromMilliseconds(MainWindow.EmulationTime * 10),
                                GetAverageQueueTime()));
                }
            });
            resultThread.Start();
            result2Thread.Start();
            mainWindow = main;
        }

        private double GetAverageTime()
        {
            List<Tuple<double,long,long>> list = new List<Tuple<double, long, long>>(MainWindow.ResultList.ToArray());
            if (list.Count == 0) return 0;
            return list.Select(req => req.Item2).Select(dummy => (double)dummy).ToList().Average();
        }
        private double GetAverageQueueTime()
        {
            List<Tuple<double, long, long>> list = new List<Tuple<double, long, long>>(MainWindow.ResultList.ToArray());
            if (list.Count == 0) return 0;
            return list.Select(req => req.Item3).Select(dummy => (double)dummy).ToList().Average();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            resultThread.Abort();
            result2Thread.Abort();
            mainWindow.StopEmulation();
        }
    }
}
