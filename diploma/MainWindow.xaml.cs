using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;
using diploma;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace diploma
{
    public enum EmulationStatus : int
    {
        On,
        Pause,
        Off
    }

    public enum DelayType : int
    {
        Second,
        Minute,
        Hour
    }

    public enum MouseAction : int
    {
        Move,
        Delete
    }

    public enum DistributionType : int
    {
        Bernoulli,
        Beta,
        Binominal,
        Weibull,
        Gamma,
        Geometric,
        Cauchy,
        Lognormal,
        Normal,
        Pareto,
        Poisson,
        Uniform,
        Chi2,
        Exponential,
        Erlang
    }

    public partial class MainWindow : Window
    {
        public static SeriesCollection SeriesCollection { get; set; }
        public static List<Tuple<double,long,long>> ResultList = new List<Tuple<double, long, long>>();
        private readonly ElementFactory _elementFactory = new ElementFactory();
        public static readonly List<IElement> ElementsList = new List<IElement>();
        public static readonly List<Connector> Connectors = new List<Connector>();
        public static EmulationStatus Emulation = EmulationStatus.Off;
        private static IElement _movedElement = null;
        private static Image _movedImage = null;
        private static double _distanceBetweenX = 0;
        private static double _distanceBetweenY = 0;
        private const double ElementsHeight = 50;
        private readonly List<Thread> _runnedElementsThreads = new List<Thread>();
        public static long EmulationTime = 0;
        public const int SynchronizationCoefficient = 10;
        private int speedCoefficient = 1;
        private ResultWindow resultWindow = null;
        public static MouseAction MouseAction = MouseAction.Move;
        private static bool errorShow = false;

        public class TimeSpanPoint : IObservableChartPoint
        {
            private TimeSpan _dateTime;
            private double _value;
            
            public TimeSpanPoint()
            {

            }
            
            public TimeSpanPoint(TimeSpan dateTime, double value)
            {
                _dateTime = dateTime;
                _value = value;
            }
            
            public TimeSpan DateTime
            {
                get { return _dateTime; }
                set
                {
                    _dateTime = value;
                    OnPointChanged();
                }
            }
            
            public double Value
            {
                get { return _value; }
                set
                {
                    _value = value;
                    OnPointChanged();
                }
            }
            
            public event Action PointChanged;
            
            protected virtual void OnPointChanged()
            {
                if (PointChanged != null) PointChanged.Invoke();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            _elementFactory.canvas = canvas;
            var mapper = Mappers.Xy<TimeSpanPoint>()
                .X(model => model.DateTime.TotalMilliseconds)   //use DateTime.Ticks as X
                .Y(model => model.Value);           //use the value property as Y

            //lets save the mapper globally.
            Charting.For<TimeSpanPoint>(mapper);
            SeriesCollection = new SeriesCollection
            {
                new LineSeries()
                {
                    Title = "Время отклика",
                    Values = new ChartValues<TimeSpanPoint>(),
                    LineSmoothness = 0
                },
                new LineSeries()
                {
                    Title = "Время в очереди",
                    Values = new ChartValues<TimeSpanPoint>(),
                    LineSmoothness = 0
                }
            };
        }

        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            if (_movedElement != null)
            {
                double coordinateX = (e.GetPosition(canvas).X - _distanceBetweenX)<0? 0: e.GetPosition(canvas).X - _distanceBetweenX;
                double coordinateY = (e.GetPosition(canvas).Y - _distanceBetweenY)<0? 0: e.GetPosition(canvas).Y - _distanceBetweenY;

                coordinateX = (e.GetPosition(canvas).X + _distanceBetweenX) > canvas.MinWidth ? canvas.MinWidth - 50 : e.GetPosition(canvas).X - _distanceBetweenX;
                coordinateY = (e.GetPosition(canvas).Y + _distanceBetweenY) > canvas.MinHeight ? canvas.MinHeight - 50 : e.GetPosition(canvas).Y - _distanceBetweenY;
                _movedElement.Move(coordinateX, coordinateY);
            }
            else
            {
                Canvas.SetLeft(_movedImage, e.GetPosition(canvas).X);
                Canvas.SetTop(_movedImage, e.GetPosition(canvas).Y);
            }
            
        }

        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            if (_movedElement != null)
            {
                _movedElement = null;
                return;
            }
            var img = new Image
            {
                Height = ElementsHeight,
                Source = createImage.Source
            };
            _distanceBetweenX = e.GetPosition(canvas).X - Canvas.GetLeft(_movedImage);
            _distanceBetweenY = e.GetPosition(canvas).Y - Canvas.GetTop(_movedImage);

            double coordinateX = (e.GetPosition(canvas).X - _distanceBetweenX) < 0 ? 0 : e.GetPosition(canvas).X - _distanceBetweenX;
            double coordinateY = (e.GetPosition(canvas).Y - _distanceBetweenY) < 0 ? 0 : e.GetPosition(canvas).Y - _distanceBetweenY;

            coordinateX = (e.GetPosition(canvas).X + _distanceBetweenX) > canvas.MinWidth  - 50 ? canvas.MinWidth - 50 : e.GetPosition(canvas).X - _distanceBetweenX;
            coordinateY = (e.GetPosition(canvas).Y + _distanceBetweenY) > canvas.MinHeight - 50 ? canvas.MinHeight - 50 : e.GetPosition(canvas).Y - _distanceBetweenY;

            img.MouseLeftButtonDown += newImage_MouseDown;
            img.MouseRightButtonUp += Image_MouseRightButtonUp;
            Canvas.SetLeft(img, coordinateX);
            Canvas.SetTop(img, coordinateY);
            ElementsList.Add(_elementFactory.CreateElement(createImage.Name, img));
            createImage.Visibility = Visibility.Hidden;
        }

        private static IElement FindElementByImage(Image image)
        {
            return ElementsList.FirstOrDefault(element => element.Image.GetHashCode().Equals(image.GetHashCode()));
        }

        private void image_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_elementFactory.createdLine != null || Emulation != EmulationStatus.Off) return;
            
                _movedImage = createImage;
                createImage.Name = ((Image) sender).Name;
                createImage.Source = ((Image) sender).Source;
                createImage.Visibility = Visibility.Hidden;
                DragDrop.DoDragDrop((DependencyObject) sender, ((Image) sender).Source, DragDropEffects.Copy);
           
             
        }

        private void Canvas_DragEnter(object sender, DragEventArgs e)
        {
            if (!Equals(_movedImage, createImage)) return;
            createImage.Visibility = Visibility.Visible;
        }

        private void newImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_elementFactory.createdLine != null || Emulation != EmulationStatus.Off) return;
            if (MouseAction == MouseAction.Move)
            {
                _movedElement = FindElementByImage((Image) sender);
                _movedImage = (Image) _movedElement.Image;
                _distanceBetweenX = e.GetPosition(canvas).X - Canvas.GetLeft((Image) sender);
                _distanceBetweenY = e.GetPosition(canvas).Y - Canvas.GetTop((Image) sender);
                DragDrop.DoDragDrop((DependencyObject) sender, ((Image) sender).Source, DragDropEffects.Move);
            }
            else
            {
                IElement elem = FindElementByImage((Image) sender);
                elem.Delete(canvas);
            }
        }

        private void Canvas_DragLeave(object sender, DragEventArgs e)
        {
            createImage.Visibility = Visibility.Hidden;
        }
 
        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            Emulation = EmulationStatus.On;
            startButton.IsEnabled = false;
            pauseButton.IsEnabled = true;
            stopButton.IsEnabled = true;

            foreach (var element in ElementsList)
            {
                Thread newThread = new Thread(
                    o =>
                    {
                        while (true)
                        {
                            while (Emulation == EmulationStatus.Pause) { Thread.Sleep(1); }
                            Thread.Sleep(SynchronizationCoefficient);
                            element.Run();
                        }
                    });
                _runnedElementsThreads.Add(newThread);
            }
            Thread timerThread = new Thread(
                o =>
                {
                    while (true)
                    {
                        while (Emulation == EmulationStatus.Pause)
                        {
                            if (errorShow == true)
                            {
                                pauseButton.Dispatcher.Invoke(
                                    (Action)(() =>
                                    {
                                        pauseButton.IsEnabled = false;
                                    }
                                        ));
                            }
                            Thread.Sleep(1);
                        }
                        if (speedCoefficient < 0)
                        {
                           
                            Thread.Sleep(SynchronizationCoefficient * Math.Abs(speedCoefficient));
                        }
                        else
                        {
                            
                            Thread.Sleep(SynchronizationCoefficient);
                        }
                        timerLabel.Dispatcher.Invoke(
                            (Action) (() =>
                            {
                                if (MainWindow.Emulation == EmulationStatus.Off)
                                {
                                    return;
                                }
                                if (speedCoefficient > 0)
                                {
                                    timerLabel.Content =
                                    TimeSpan.FromMilliseconds((MainWindow.EmulationTime = (MainWindow.EmulationTime + (1 * speedCoefficient))) * SynchronizationCoefficient)
                                        .ToString("g");
                                }
                                else
                                {
                                    timerLabel.Content =
                                    TimeSpan.FromMilliseconds((MainWindow.EmulationTime++) * SynchronizationCoefficient)
                                        .ToString("g");
                                }
                            }));
                    }
                });
            _runnedElementsThreads.Add(timerThread);
            foreach (var thread in _runnedElementsThreads)
            {
                thread.Start();
            }
            resultWindow = new ResultWindow(this);
            resultWindow.SeriesCollection = SeriesCollection;
            resultWindow.Show();
        }

        private void pauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (Emulation == EmulationStatus.Pause)
            {
                Emulation = EmulationStatus.On;
                pauseButton.Content = "Pause";
            }
            else
            {
                Emulation = EmulationStatus.Pause;
                pauseButton.Content = "Resume";
            }
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            StopEmulation();
        }

        public void StopEmulation()
        {
            Emulation = EmulationStatus.Off;
            foreach (var thread in _runnedElementsThreads)
            {
                thread.Abort();
            }
            _runnedElementsThreads.Clear();
            foreach (var element in ElementsList)
            {
                element.Stop();
            }
            EmulationTime = 0;
            timerLabel.Content = "0:00:00,00";
            startButton.IsEnabled = true;
            pauseButton.IsEnabled = false;
            stopButton.IsEnabled = false;
            pauseButton.Content = "Pause";
            errorShow = false;
            foreach (var connect in Connectors)
            {
                connect.MakePointGreen();
            }
            ResultWindow.Generated2 = "0";
            ResultWindow.Complite2 = "0";
            MainWindow.SeriesCollection[0].Values.Clear();
            MainWindow.SeriesCollection[1].Values.Clear();
            MainWindow.ResultList.Clear();
            resultWindow.Hide();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Emulation != EmulationStatus.Off)
            {
                foreach (var thread in _runnedElementsThreads)
                {
                    thread.Abort();
                }
                _runnedElementsThreads.Clear();

                foreach (var element in ElementsList)
                {
                    element.Stop();
                }
            }
        }
 

        private void increaseSpeed_Click(object sender, RoutedEventArgs e)
        {
            switch (speedCoefficient)
            {
                case 1:
                    speedCoefficient = 2;
                    break;
                case 2:
                    speedCoefficient = 5;
                    break;
                case 5:
                    speedCoefficient = 10;
                    break;
                case 10:
                    speedCoefficient = 25;
                    break;
                case 25:
                    speedCoefficient = 50;
                    break;
                case 50:
                    speedCoefficient = 100;
                    break;
                case -2:
                    speedCoefficient = 1;
                    break;
                case -5:
                    speedCoefficient = -2;
                    break;
                case -10:
                    speedCoefficient = -5;
                    break;
                case -25:
                    speedCoefficient = -10;
                    break;
                case -50:
                    speedCoefficient = -25;
                    break;
                case -100:
                    speedCoefficient = -50;
                    break;
            }

            if (speedCoefficient > 0)
            {
                speed.Content = "x" + speedCoefficient;
            }
            else 
            {
                speed.Content = "x1/" + Math.Abs(speedCoefficient);
            }
        }

        private void decreaseSpeed_Click(object sender, RoutedEventArgs e)
        {
            switch (speedCoefficient)
            {
                case 1:
                    speedCoefficient = -2;
                    break;
                case -2:
                    speedCoefficient = -5;
                    break;
                case -5:
                    speedCoefficient = -10;
                    break;
                case -10:
                    speedCoefficient = -25;
                    break;
                case -25:
                    speedCoefficient = -50;
                    break;
                case -50:
                    speedCoefficient = -100;
                    break;
                case 2:
                    speedCoefficient = 1;
                    break;
                case 5:
                    speedCoefficient = 2;
                    break;
                case 10:
                    speedCoefficient = 5;
                    break;
                case 25:
                    speedCoefficient = 10;
                    break;
                case 50:
                    speedCoefficient = 25;
                    break;
                case 100:
                    speedCoefficient = 50;
                    break;
            }

            if (speedCoefficient > 0)
            {
                speed.Content = "x" + speedCoefficient;
            }
            else
            {
                speed.Content = "x1/" + Math.Abs(speedCoefficient);
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SMO");
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SMO";
            saveFileDialog.Filter = "File (*.dat)|*.dat";
            if (saveFileDialog.ShowDialog() != true) return;
            int pointCount = 1;
            foreach (var elem in ElementsList)
            {
                JObject jObject = new JObject();
                if (elem.GetType() == typeof(Source))
                {
                    Source source = (Source) elem;
                    jObject["DelayType"] = source.DelayType.ToString();
                    jObject["ElementDistributions"] = JsonConvert.SerializeObject(source.ElementDistributions);
                    jObject["TimeDistributions"] = JsonConvert.SerializeObject(source.TimeDistributions);
                }
                else if (elem.GetType() == typeof(Queue))
                {
                    Queue queue = (Queue) elem;
                    jObject["DelayType"] = queue.DelayType.ToString();
                    jObject["Distribution"] = JsonConvert.SerializeObject(queue.TimeLimitDistributions);
                    jObject["Capacity"] = queue.Capacity;
                    jObject["TimeLimitThrowOut"] = queue.TimeLimitThrowOut;
                    jObject["ThrowOutWhenOverflow"] = queue.ThrowOutWhenOverflow;
                }
                else if (elem.GetType() == typeof(Delay))
                {
                    Delay delay = (Delay) elem;
                    jObject["DelayType"] = delay.DelayType.ToString();
                    jObject["DelayDistributions"] = JsonConvert.SerializeObject(delay.DelayDistributions);
                    jObject["Capacity"] = delay.Capacity;
                }
                else if (elem.GetType() == typeof(SelectOutput))
                {
                    SelectOutput selectOutput = (SelectOutput) elem;
                    jObject["Chance"] = selectOutput.Chance;
                }
                else
                {
                    jObject["Name"] = "Sink";
                }
                ((Image) elem.Image).Tag = jObject.ToString();
            }
            foreach (var connector in MainWindow.Connectors)
            {
                JObject jObject = new JObject();
                jObject["StartElement"] = ((Image)connector.StartElement.Image).Name;
                jObject["StartPointNumber"] = connector.StartElement.GetPointNumber((Image)connector.StartPoint);
                jObject["EndElement"] = ((Image)connector.EndElement.Image).Name;
                jObject["EndPointNumber"] = connector.EndElement.GetPointNumber((Image)connector.EndPoint);
                ((Line)connector.Image).Tag = jObject.ToString();
            }
            string savedCanvas = XamlWriter.Save(canvas);
            File.WriteAllText(saveFileDialog.FileName, savedCanvas);

        }


        private static DelayType DelayTypeParseFromString(string delayType)
        {
            switch (delayType)
            {
                case "Second":
                    return DelayType.Second;
                case "Minute":
                    return DelayType.Minute;
                default:
                    return DelayType.Hour;
            }
        }

        private IElement FindElementByImageName(string name)
        {
            return ElementsList.FirstOrDefault(elem => ((Image) elem.Image).Name == name);
        }

        private void loadbutton_Click(object sender, RoutedEventArgs e)
        {
             OpenFileDialog openFileDialog = new OpenFileDialog();
            string filename = null;
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SMO");
              openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SMO";
              openFileDialog.Filter = "File (*.dat)|*.dat";
            if (openFileDialog.ShowDialog() == true)
                filename = openFileDialog.FileName;
            else return;
            MainWindow.Connectors.Clear();
            MainWindow.ElementsList.Clear();
            _elementFactory.Rewind();
            using (FileStream SourceStream = File.Open(filename, FileMode.Open))
            {
               Canvas canv = (Canvas)XamlReader.Load(SourceStream);
                canvas.Children.RemoveRange(1, canvas.Children.Count);
                canv.Children.RemoveAt(0);
               
                List<UIElement> list = new List<UIElement>();
                foreach (UIElement child in canv.Children)
                {
                    list.Add(child);
                }
                canv.Children.Clear();
                foreach (var child in list)
                {
                    if (child.GetType() == typeof(Image))
                    {
                        if (((Image)child).Tag != null)
                        {
                            IElement element = null;
                            JObject jObject =
                                JObject.Parse(JsonConvert.DeserializeObject(((Image) child).Tag.ToString()).ToString());
                            if (((Image) child).Name.Contains("Source"))
                            {
                                element = _elementFactory.CreateElement("Source", (UIElement) child);

                                ((Source) element).DelayType = DelayTypeParseFromString(jObject["DelayType"].ToString());
                                ((Source) element).ElementDistributions = JsonConvert.DeserializeObject<Distributions>(jObject["ElementDistributions"].ToString()); ;
                                ((Source) element).TimeDistributions = JsonConvert.DeserializeObject<Distributions>(jObject["TimeDistributions"].ToString()); ;
                            }
                            else if (((Image) child).Name.Contains("Queue"))
                            {
                                element = _elementFactory.CreateElement("Queue", (UIElement) child);
                                ((Queue) element).DelayType = DelayTypeParseFromString(jObject["DelayType"].ToString());
                                ((Queue) element).Capacity = int.Parse(jObject["Capacity"].ToString());
                                ((Queue)element).TimeLimitDistributions = JsonConvert.DeserializeObject<Distributions>(jObject["Distribution"].ToString()); 
                                ((Queue) element).TimeLimitThrowOut =
                                    bool.Parse(jObject["TimeLimitThrowOut"].ToString());
                                ((Queue) element).ThrowOutWhenOverflow =
                                    bool.Parse(jObject["ThrowOutWhenOverflow"].ToString());
                            }
                            else if (((Image) child).Name.Contains("Delay"))
                            {
                                element = _elementFactory.CreateElement("Delay", (UIElement) child);
                                ((Delay) element).DelayType = DelayTypeParseFromString(jObject["DelayType"].ToString());
                                ((Delay)element).DelayDistributions = JsonConvert.DeserializeObject<Distributions>(jObject["DelayDistributions"].ToString());
                                ((Delay) element).Capacity = int.Parse(jObject["Capacity"].ToString());
                            }
                            else if (((Image) child).Name.Contains("SelectOutput"))
                            {
                                element = _elementFactory.CreateElement("SelectOutput", (UIElement) child);
                                ((SelectOutput) element).Chance = double.Parse(jObject["Chance"].ToString());
                            }
                            else if (((Image) child).Name.Contains("Sink"))
                            {
                                element = _elementFactory.CreateElement("Sink", (UIElement) child);
                            }
                            ElementsList.Add(element);

                        }
                    }
                    if (child is Line)
                    {
                        JObject jObject =
                            JObject.Parse(JsonConvert.DeserializeObject(((Line) child).Tag.ToString()).ToString());
                        Connector connector = new Connector(child);
                        connector.StartElement = FindElementByImageName(jObject["StartElement"].ToString());
                        connector.StartPoint =
                            connector.StartElement.GetPointByNumber(Int32.Parse(jObject["StartPointNumber"].ToString()));
                        connector.EndElement = FindElementByImageName(jObject["EndElement"].ToString());
                        connector.EndPoint =
                            connector.EndElement.GetPointByNumber(Int32.Parse(jObject["EndPointNumber"].ToString()));
                        connector.Image.MouseUp += connector_MouseUp;
                        Connectors.Add(connector);
                    }
                }
                
                canv.Children.Clear();
                foreach (var child in list)
                {
                    if (child.GetType() == typeof(Image))
                    {
                        
                        if (((Image)child).Tag != null)
                        {

                            child.MouseRightButtonUp += Image_MouseRightButtonUp;
                            child.MouseLeftButtonDown += newImage_MouseDown;
                        }
                    }
                    else
                    {
                        canvas.Children.Add(child);
                    }
                }
            }
        }
        private void connector_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Emulation != EmulationStatus.Off) return;
            if (MouseAction == MouseAction.Delete)
            {
                Connector deletedConnector = FindConnectorByImage(((Line)sender));
                canvas.Children.Remove(deletedConnector.Image);
                Connectors.Remove(deletedConnector);
            }
        }

        private static void Image_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(Emulation != EmulationStatus.Off)  return;
            var elem = FindElementByImage((Image) sender);
            if (elem.GetType() == typeof(Source))
            {
                SourceParametersWindow sourceParametersWindow = new SourceParametersWindow((Source)elem);
                sourceParametersWindow.Show();
            }
            else if (elem.GetType() == typeof(Queue))
            {
                QueueParametersWindow queueParametersWindow = new QueueParametersWindow((Queue)elem);
                queueParametersWindow.Show();
            }
            else if (elem.GetType() == typeof(Delay))
            {
                DelayParametersWindow delayParametersWindow = new DelayParametersWindow((Delay)elem);
                delayParametersWindow.Show();
            }
            else if (elem.GetType() == typeof(SelectOutput))
            {
                SelectOutputParametersWindow selectOutputParametersWindow = new SelectOutputParametersWindow((SelectOutput)elem);
                selectOutputParametersWindow.Show();
            }
        }

        private void mouseAction_Click(object sender, RoutedEventArgs e)
        {
            if ((string) mouseAction.Content == "Move")
            {
                mouseAction.Content = "Delete";
                MouseAction = MouseAction.Delete;
            }
            else
            {
                mouseAction.Content = "Move";
                MouseAction = MouseAction.Move;
            }
        }
        private static Connector FindConnectorByImage(Line connector)
        {
            foreach (var connect in MainWindow.Connectors)
            {
                if (connect.Image.GetHashCode() == connector.GetHashCode())
                {
                    return connect;
                }
            }
            return null;
        }
        public static void ErrorStop(IElement element, Image errorPoint)
        {
            Emulation = EmulationStatus.Pause;
            errorShow = true;
            string name = "";
            (element.Image).Dispatcher.Invoke((Action) (() =>
            {
                 name = ((Image)element.Image).Name;
            }));
            foreach (var connect in Connectors)
            {
                if (connect.HavePoint(errorPoint))
                {
                    connect.MakePointRed();
                }

            }
            MessageBox.Show("Следующий элемент переполнен" , "Error in " + name, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
