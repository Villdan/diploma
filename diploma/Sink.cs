using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Defaults;

namespace diploma
{
    class Sink : IElement
    {
        public Sink(UIElement image, InPoint inPoint)
        {
            Image = image;
            In = inPoint;
        }

        private const int InPointTopOffset = 19;
        private int _labelXOffset = 8;
        private int _labelYOffset = 8;
        private int _digitCount = 1;

        public InPoint In { get; set; }
        public UIElement Image { get; set; }

        public void Run()
        {
            In.Label.Dispatcher.BeginInvoke(
                (Action)(() =>
                {
                    In.Label.Visibility = Visibility.Visible;
                }));
            In.Label.Dispatcher.BeginInvoke(
               (Action)(() =>
               {
                   if (((int)Math.Log10(Double.Parse(In.Label.Content.ToString())) + 1)> _digitCount)
                   {
                       _digitCount++;
                       _labelXOffset += 8;
                       Move(Canvas.GetLeft(Image), Canvas.GetTop(Image));
                   }
                   In.Label.Visibility = Visibility.Visible;
               }));
             

        }

        public void Stop()
        {
            In.Label.Dispatcher.BeginInvoke(
                 (Action)(() =>
                 {
                     In.Label.Visibility = Visibility.Hidden;
                     In.Label.Content = "0";
                     _digitCount = 1;
                     _labelXOffset = 8;
                 }));
            MainWindow.ResultList.Clear();
        }

        public void Move(double mouseX, double mouseY)
        {
            Canvas.SetLeft(Image, mouseX);
            Canvas.SetTop(Image, mouseY);

            Canvas.SetTop(In.Image, mouseY + InPointTopOffset);
            Canvas.SetLeft(In.Image, mouseX);

            Canvas.SetTop(In.Label, mouseY + InPointTopOffset + _labelYOffset);
            Canvas.SetLeft(In.Label, mouseX - _labelXOffset);
            Line line = null;
            foreach (var connector in MainWindow.Connectors)
            {
                if (!connector.EndPoint.GetHashCode().Equals(In.Image.GetHashCode())) continue;
                line = (Line)connector.Image;
                line.X2 = mouseX + ElementFactory.PointMiddle;
                line.Y2 = mouseY + InPointTopOffset + ElementFactory.PointMiddle;
            }
        }

        public bool InRequest(double request, long time, long queueTime)
        {
            MainWindow.ResultList.Add((Tuple.Create(request, MainWindow.EmulationTime - time, queueTime)));
            In.Label.Dispatcher.BeginInvoke(
               (Action)(() =>
               {
                   In.Label.Content = (int.Parse(In.Label.Content.ToString()) + 1).ToString();
               }));
            ResultWindow.Complite2 = ResultWindow.Complite2 == null
                        ? "1"
                        : (int.Parse(ResultWindow.Complite2) + 1).ToString();
            return true;
        }

        public void OutRequest()
        {
            throw new NotImplementedException();
        }

        public bool HavePoint(Image point)
        {
            return In.Image.Equals(point);
        }

        public bool HaveInPoint(Image point)
        {
            return In.Image.Equals(point);
        }

        public bool HaveOutPoint(Image point)
        {
            return false;
        }

        public string GetElementType()
        {
            return "Sink";
        }

        public void Delete(Canvas canvas)
        {
            List<Connector> removedConnectors = new List<Connector>();
            foreach (var connector in MainWindow.Connectors)
            {
                if (connector.HaveInPoint(In.Image))
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
        }

        public int GetPointNumber(Image point)
        {
            if (point.GetHashCode() == In.Image.GetHashCode())
            {
                return 1;
            }
            return 0;
        }

        public Image GetPointByNumber(int number)
        {
            if (number == 1)
            {
                return (Image)In.Image;
            }
            return null;
        }
    }
}
