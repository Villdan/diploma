using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace diploma
{
    public class Delay : IElement
    {
        public Delay(UIElement image, InPoint inPoint, OutPoint outPoint)
        {
            Image = image;
            In = inPoint;
            Out = outPoint;
            DelayDistributions.min = 0;
            DelayDistributions.max = 1;
        }

        private const int InPointTopOffset = 19;
        private const int OutPointTopOffset = 19;
        private const int OutPointLeftOffset = 50;
        private int _labelXOffset = 7;
        private int _labelYOffset = 7;

        public DelayType DelayType = DelayType.Second;

        public InPoint In { get; set; }
        public OutPoint Out { get; set; }
        public UIElement Image { get; set; }
        private readonly List<Tuple<double, long, long>> _requests = new List<Tuple<double, long, long>>();
        public int Capacity = 1;
        public  Distributions DelayDistributions = new Distributions();
        private int _digitCount = 1;

        public void Run()
        {
            In.Label.Dispatcher.BeginInvoke(
               (Action)(() =>
               {
                   if (((int)Math.Log10(Double.Parse(In.Label.Content.ToString())) + 1) > _digitCount)
                   {
                       _digitCount++;
                       _labelXOffset += 7;
                       Move(Canvas.GetLeft(Image), Canvas.GetTop(Image));
                   }
                   In.Label.Visibility = Visibility.Visible;
               }));
            Out.Label.Dispatcher.BeginInvoke(
              (Action)(() =>
              {
                  Out.Label.Visibility = Visibility.Visible;
              }));
            switch (DelayType)
            {
                case DelayType.Second:
                    SecondDelay(DelayDistributions.GetNextRand());
                    break;
                case DelayType.Minute:
                    MinuteDelay(DelayDistributions.GetNextRand());
                    break;
                case DelayType.Hour:
                    HourDelay(DelayDistributions.GetNextRand());
                    break;
            }
            foreach (var connector in MainWindow.Connectors)
            {
                if (_requests.Count == 0)
                {
                    continue;
                }
                if (connector.HaveOutPoint((Image)Out.Image))
                {
                    if (_requests[0] != null)
                    {
                        if (!connector.EndElement.InRequest(_requests[0].Item1, _requests[0].Item2, _requests[0].Item3))
                        {
                            if (_requests.Count >= Capacity)
                            {
                                MainWindow.ErrorStop(this, Out.Image);
                            }
                        }
                        else
                        {
                            _requests.RemoveAt(0);
                            OutRequest();
                        }
                    }
                    else
                    {
                        _requests.RemoveAt(0);
                    }
                }
            }
        }

        private static void SecondDelay(double delayTime)
        {
            double secondsToMilliseconds = TimeSpan.FromSeconds(Math.Truncate(delayTime)).TotalMilliseconds;
            double milliseconds = TimeSpan.FromMilliseconds((delayTime - Math.Truncate(delayTime)) * 1000).TotalMilliseconds;
            TimeSpan final = TimeSpan.FromMilliseconds((MainWindow.EmulationTime) * MainWindow.SynchronizationCoefficient);
            while ((TimeSpan.FromMilliseconds((MainWindow.EmulationTime) * MainWindow.SynchronizationCoefficient).TotalMilliseconds) < (final.TotalMilliseconds + secondsToMilliseconds + milliseconds)) { }
        }

        private static void MinuteDelay(double delayTime)
        {
            double minuteToMilliseconds = TimeSpan.FromMinutes(Math.Truncate(delayTime)).TotalMilliseconds;
            double secondsToMilliseconds = TimeSpan.FromSeconds((delayTime - Math.Truncate(delayTime)) * 100).TotalMilliseconds;
            TimeSpan final = TimeSpan.FromMilliseconds((MainWindow.EmulationTime) * MainWindow.SynchronizationCoefficient);
            while ((TimeSpan.FromMilliseconds((MainWindow.EmulationTime) * MainWindow.SynchronizationCoefficient).TotalMilliseconds) < (final.TotalMilliseconds + minuteToMilliseconds + secondsToMilliseconds)) { }
        }

        private static void HourDelay(double delayTime)
        {
            double houreToMilliseconds = TimeSpan.FromHours(Math.Truncate(delayTime)).TotalMilliseconds;
            double minuteToMilliseconds = TimeSpan.FromSeconds((delayTime - Math.Truncate(delayTime)) * 100).TotalMilliseconds;
            TimeSpan final = TimeSpan.FromMilliseconds((MainWindow.EmulationTime) * MainWindow.SynchronizationCoefficient);
            while ((TimeSpan.FromMilliseconds((MainWindow.EmulationTime) * MainWindow.SynchronizationCoefficient).TotalMilliseconds) < (final.TotalMilliseconds + houreToMilliseconds + minuteToMilliseconds)) { }
        }

        public void Stop()
        {
            In.Label.Dispatcher.BeginInvoke(
                (Action) (() =>
                {
                    In.Label.Visibility = Visibility.Hidden;
                    In.Label.Content = "0";
                    _digitCount = 1;
                    _labelXOffset = 7;
                }));
            Out.Label.Dispatcher.BeginInvoke(
              (Action)(() =>
              {
                  Out.Label.Visibility = Visibility.Hidden;
                  Out.Label.Content = "0";
              }));
            _requests.Clear();
        }

        public void Move(double mouseX, double mouseY)
        {
            Canvas.SetLeft(Image, mouseX);
            Canvas.SetTop(Image, mouseY);

            Canvas.SetTop(In.Image, mouseY + InPointTopOffset);
            Canvas.SetLeft(In.Image, mouseX);

            Canvas.SetTop(In.Label, mouseY + InPointTopOffset + _labelYOffset);
            Canvas.SetLeft(In.Label, mouseX - _labelXOffset);

            Canvas.SetTop(Out.Image, mouseY + OutPointTopOffset);
            Canvas.SetLeft(Out.Image, mouseX + OutPointLeftOffset);

            Canvas.SetTop(Out.Label, mouseY + OutPointTopOffset + _labelYOffset);
            Canvas.SetLeft(Out.Label, mouseX + OutPointLeftOffset + _labelYOffset);

            Line line = null;
            foreach (var connector in MainWindow.Connectors)
            {
                if (connector.StartPoint.GetHashCode().Equals(Out.Image.GetHashCode()))
                {
                    line = (Line)connector.Image;
                    line.X1 = mouseX + OutPointLeftOffset + ElementFactory.PointMiddle;
                    line.Y1 = mouseY + OutPointTopOffset + ElementFactory.PointMiddle;
                }
                if (!connector.EndPoint.GetHashCode().Equals(In.Image.GetHashCode())) continue;
                line = (Line)connector.Image;
                line.X2 = mouseX + ElementFactory.PointMiddle;
                line.Y2 = mouseY + InPointTopOffset + ElementFactory.PointMiddle;
            }
        }

        public bool InRequest(double request, long time, long queueTime)
        {
            if (_requests.Count == Capacity)
            {
                return false;
            }
            In.Label.Dispatcher.BeginInvoke(
                (Action) (() =>
                {
                    In.Label.Content = (int.Parse(In.Label.Content.ToString()) + 1).ToString();
                }));
            _requests.Add((Tuple.Create(request, time,queueTime)));
            return true;
        }

        public void OutRequest()
        {
            Out.Label.Dispatcher.BeginInvoke(
               (Action)(() =>
               {
                   Out.Label.Content = (int.Parse(Out.Label.Content.ToString()) + 1).ToString();
               }));
        }

        public bool HavePoint(Image point)
        {
            return In.Image.Equals(point) || Out.Image.Equals(point);
        }

        public bool HaveInPoint(Image point)
        {
            return In.Image.Equals(point);
        }

        public bool HaveOutPoint(Image point)
        {
            return Out.Image.Equals(point);
        }

        public string GetElementType()
        {
            return "Delay";
        }

        public void Delete(Canvas canvas)
        {
            List<Connector> removedConnectors = new List<Connector>();
            foreach (var connector in MainWindow.Connectors)
            {
                if (connector.HaveInPoint(In.Image) || connector.HaveOutPoint(Out.Image))
                {
                    removedConnectors.Add(connector);
                }
            }
            foreach (var connector in removedConnectors)
            {
                canvas.Children.Remove(connector.Image);
                MainWindow.Connectors.Remove(connector);
            }
            canvas.Children.Remove(Image);
            canvas.Children.Remove(In.Image);
            canvas.Children.Remove(In.Label);
            canvas.Children.Remove(Out.Image);
            canvas.Children.Remove(Out.Label);
        }

        public int GetPointNumber(Image point)
        {
            if (point.GetHashCode() == In.Image.GetHashCode())
            {
                return 1;
            }
            else if (point.GetHashCode() == Out.Image.GetHashCode())
            {
                return 2;
            }
            return 0;
        }

        public Image GetPointByNumber(int number)
        {
            if (number == 1)
            {
                return (Image)In.Image;
            }
            else if (number == 2)
            {
                return (Image)Out.Image;
            }
            return null;
        }

    }
}
