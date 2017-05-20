using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace diploma
{
    public class Source : IElement
    {
        public Source(UIElement image, OutPoint outPoint)
        {
            Image = image;
            Out = outPoint;
            TimeDistributions.min = 0;
            TimeDistributions.max = 1;
            ElementDistributions.min = 0;
            ElementDistributions.max = 1;
        }

        public OutPoint Out { get; set; }
        public UIElement Image { get; set; }

        private const int LabelOffset = 5;
        private const int OutPointTopOffset = 19;
        private const int OutPointLeftOffset = 39;
        public DelayType DelayType = DelayType.Second;
        public Distributions TimeDistributions = new Distributions();
        public Distributions ElementDistributions = new Distributions();

        public void Run()
        {
            Out.Label.Dispatcher.BeginInvoke(
                (Action) (() =>
                {
                    Out.Label.Visibility = Visibility.Visible;
                }));
            switch (DelayType)
            {
                case DelayType.Second:
                    SecondDelay(TimeDistributions.GetNextRand());
                    break;
                case DelayType.Minute:
                    MinuteDelay(TimeDistributions.GetNextRand());
                    break;
                case DelayType.Hour:
                    HourDelay(TimeDistributions.GetNextRand());
                    break;
            }
            foreach (var connector in MainWindow.Connectors)
            {
                if (connector.HaveOutPoint((Image) Out.Image))
                {
                    if (!connector.EndElement.InRequest(ElementDistributions.GetNextRand(),MainWindow.EmulationTime,0))
                    {
                        MainWindow.ErrorStop(this, Out.Image);
                        return;
                    }
                    OutRequest();
                }
            }
        }
        
        private static void SecondDelay(double delayTime)
        {
            double secondsToMilliseconds = TimeSpan.FromSeconds(Math.Truncate(delayTime)).TotalMilliseconds;
            double milliseconds = TimeSpan.FromMilliseconds((delayTime- Math.Truncate(delayTime)) * 1000).TotalMilliseconds;
            TimeSpan final = TimeSpan.FromMilliseconds((MainWindow.EmulationTime) * MainWindow.SynchronizationCoefficient);
            while ((TimeSpan.FromMilliseconds((MainWindow.EmulationTime) * MainWindow.SynchronizationCoefficient).TotalMilliseconds) < (final.TotalMilliseconds + secondsToMilliseconds+ milliseconds)) {}
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
            Out.Label.Dispatcher.BeginInvoke((Action) (() =>
            {
                Out.Label.Visibility = Visibility.Hidden;
                Out.Label.Content = "0";
            }));
        }

        public void Move(double mouseX, double mouseY)
        {
            Canvas.SetLeft(Image, mouseX);
            Canvas.SetTop(Image, mouseY);

            Canvas.SetTop(Out.Image, mouseY + OutPointTopOffset);
            Canvas.SetLeft(Out.Image, mouseX + OutPointLeftOffset);

            Canvas.SetTop(Out.Label, mouseY + OutPointTopOffset + LabelOffset);
            Canvas.SetLeft(Out.Label, mouseX + OutPointLeftOffset + LabelOffset);
            Line line = null;
            foreach (var connector in MainWindow.Connectors)
            {
                if (!connector.StartPoint.GetHashCode().Equals(Out.Image.GetHashCode())) continue;
                line = (Line) connector.Image;
                line.X1 = mouseX + OutPointLeftOffset + ElementFactory.PointMiddle;
                line.Y1 = mouseY + OutPointTopOffset + ElementFactory.PointMiddle;
            }
        }

        public bool InRequest(double request, long time, long queueTime)
        {
            return true;
        }

        public void OutRequest()
        {
            ResultWindow.Generated2 = ResultWindow.Generated2 == null
                        ? "1"
                        : (int.Parse(ResultWindow.Generated2) + 1).ToString();
            Out.Label.Dispatcher.BeginInvoke((Action) (() => { Out.Label.Content = (Int32.Parse(Out.Label.Content.ToString()) + 1).ToString(); }));
        }


        public bool HavePoint(Image point)
        {
            return Out.Image.Equals(point);
        }

        public bool HaveInPoint(Image point)
        {
            return false;
        }

        public bool HaveOutPoint(Image point)
        {
            return Out.Image.Equals(point);
        }

        public string GetElementType()
        {
            return "Source";
        }

        public void Delete(Canvas canvas)
        {
            List<Connector> removedConnectors = new List<Connector>();
            foreach (var connector in MainWindow.Connectors)
            {
                if (connector.HaveOutPoint(Out.Image))
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
            canvas.Children.Remove(Out.Image);
            canvas.Children.Remove(Out.Label);
        }

        public int GetPointNumber(Image point)
        {
            if (point.GetHashCode() == Out.Image.GetHashCode())
            {
                return 1;
            }
            return 0;
        }

        public Image GetPointByNumber(int number)
        {
            if (number == 1)
            {
                return (Image)Out.Image;
            }
            return null;
        }
        
    }
}
