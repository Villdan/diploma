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
    public class SelectOutput : IElement
    {
        public SelectOutput(UIElement image, InPoint inPoint, OutPoint outPoint1, OutPoint outPoint2)
        {
            Image = image;
            In = inPoint;
            Out1 = outPoint1;
            Out2 = outPoint2;
        }

        public InPoint In { get; set; }
        public OutPoint Out1 { get; set; }
        public OutPoint Out2 { get; set; }
        public UIElement Image { get; set; }
        private const int InPointTopOffset = 20;
        private const int Out1PointTopOffset = 20;
        private const int Out1PointLeftOffset = 40;
        private const int Out2PointTopOffset = 40;
        private const int Out2PointLeftOffset = 20;
        private  int _labelXOffset = 7;
        private  int _labelYOffset = 7;

        private readonly List<Tuple<double, long, long>> _requests = new List<Tuple<double, long, long>>();
        public double Chance = 0.5;
        private int _digitCount = 1;
        private Random rand = new Random();

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
            Out1.Label.Dispatcher.BeginInvoke(
              (Action)(() =>
              {
                  Out1.Label.Visibility = Visibility.Visible;
              }));
            Out2.Label.Dispatcher.BeginInvoke(
              (Action)(() =>
              {
                  Out2.Label.Visibility = Visibility.Visible;
              }));
            if (rand.NextDouble() > Chance)
            {
                bool find = false;
                foreach (var connector in MainWindow.Connectors)
                {
                    if (_requests.Count == 0)
                    {
                        continue;
                    }
                    if (connector.HaveOutPoint((Image)Out1.Image))
                    {
                        if (!connector.EndElement.InRequest(_requests[0].Item1, _requests[0].Item2, _requests[0].Item3))
                        {
                            MainWindow.ErrorStop(this, Out1.Image);
                        }
                        _requests.RemoveAt(0);
                        OutRequest();
                        find = true;
                    }
                }
                if (!find)
                {
                    foreach (var connector in MainWindow.Connectors)
                    {
                        if (_requests.Count == 0)
                        {
                            continue;
                        }
                        if (connector.HaveOutPoint((Image)Out2.Image))
                        {
                            if (!connector.EndElement.InRequest(_requests[0].Item1, _requests[0].Item2, _requests[0].Item3))
                            {
                                MainWindow.ErrorStop(this, Out2.Image);
                            }
                            _requests.RemoveAt(0);
                           Out2Request();
                        }
                    }
                }
            }
            else
            {
                bool find = false;
                foreach (var connector in MainWindow.Connectors)
                {
                    if (_requests.Count == 0)
                    {
                        continue;
                    }
                    if (connector.HaveOutPoint((Image)Out2.Image))
                    {
                        if (!connector.EndElement.InRequest(_requests[0].Item1, _requests[0].Item2, _requests[0].Item3))
                        {
                            MainWindow.ErrorStop(this, Out2.Image);
                        }
                        _requests.RemoveAt(0);
                        Out2Request();
                        find = true;
                    }
                }
                if (!find)
                {
                    foreach (var connector in MainWindow.Connectors)
                    {
                        if (_requests.Count == 0)
                        {
                            continue;
                        }
                        if (connector.HaveOutPoint((Image)Out1.Image))
                        {
                            if (!connector.EndElement.InRequest(_requests[0].Item1, _requests[0].Item2, _requests[0].Item3))
                            {
                                MainWindow.ErrorStop(this, Out1.Image);
                            }
                            _requests.RemoveAt(0);
                            OutRequest();
                        }
                    }
                }
            }
            
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
            Out1.Label.Dispatcher.BeginInvoke(
              (Action)(() =>
              {
                  Out1.Label.Visibility = Visibility.Hidden;
                  Out1.Label.Content = "0";
              }));
            Out2.Label.Dispatcher.BeginInvoke(
              (Action)(() =>
              {
                  Out2.Label.Visibility = Visibility.Hidden;
                  Out2.Label.Content = "0";
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

            Canvas.SetTop(Out1.Image, mouseY + Out1PointTopOffset);
            Canvas.SetLeft(Out1.Image, mouseX + Out1PointLeftOffset);

            Canvas.SetTop(Out1.Label, mouseY + Out1PointTopOffset + _labelYOffset);
            Canvas.SetLeft(Out1.Label, mouseX + Out1PointLeftOffset + _labelYOffset);

            Canvas.SetTop(Out2.Image, mouseY + Out2PointTopOffset);
            Canvas.SetLeft(Out2.Image, mouseX + Out2PointLeftOffset);

            Canvas.SetTop(Out2.Label, mouseY + Out2PointTopOffset + _labelYOffset);
            Canvas.SetLeft(Out2.Label, mouseX + Out2PointLeftOffset + _labelYOffset);
            Line line = null;
            foreach (var connector in MainWindow.Connectors)
            {
                if (connector.StartPoint.GetHashCode().Equals(Out1.Image.GetHashCode()))
                {
                    line = (Line)connector.Image;
                    line.X1 = mouseX + Out1PointLeftOffset + ElementFactory.PointMiddle;
                    line.Y1 = mouseY + Out1PointTopOffset + ElementFactory.PointMiddle;
                }
                if (connector.StartPoint.GetHashCode().Equals(Out2.Image.GetHashCode()))
                {
                    line = (Line)connector.Image;
                    line.X1 = mouseX + Out2PointLeftOffset + ElementFactory.PointMiddle;
                    line.Y1 = mouseY + Out2PointTopOffset + ElementFactory.PointMiddle;
                }
                if (!connector.EndPoint.GetHashCode().Equals(In.Image.GetHashCode())) continue;
                line = (Line)connector.Image;
                line.X2 = mouseX + ElementFactory.PointMiddle;
                line.Y2 = mouseY + InPointTopOffset + ElementFactory.PointMiddle;
            }
        }

        public bool InRequest(double request, long time, long queueTime)
        {
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
            Out1.Label.Dispatcher.BeginInvoke(
                (Action)(() =>
                {
                    Out1.Label.Content = (int.Parse(Out1.Label.Content.ToString()) + 1).ToString();
                }));
        }

        private void Out2Request()
        {
            Out2.Label.Dispatcher.BeginInvoke(
               (Action)(() =>
               {
                   Out2.Label.Content = (int.Parse(Out2.Label.Content.ToString()) + 1).ToString();
               }));
        }

        public bool HavePoint(Image point)
        {
            return In.Image.Equals(point) || Out1.Image.Equals(point) || Out2.Image.Equals(point);
        }

        public bool HaveInPoint(Image point)
        {
            return In.Image.Equals(point);
        }

        public bool HaveOutPoint(Image point)
        {
            return Out1.Image.Equals(point) || Out2.Image.Equals(point);
        }

        public string GetElementType()
        {
            return "SelectOutput";
        }

        public void Delete(Canvas canvas)
        {
            List<Connector> removedConnectors = new List<Connector>();
            foreach (var connector in MainWindow.Connectors)
            {
                if (connector.HaveInPoint(In.Image) || connector.HaveOutPoint(Out1.Image) || connector.HaveOutPoint(Out2.Image))
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
            canvas.Children.Remove(Out1.Image);
            canvas.Children.Remove(Out1.Label);
            canvas.Children.Remove(Out2.Image);
            canvas.Children.Remove(Out2.Label);
        }

        public int GetPointNumber(Image point)
        {
            if (point.GetHashCode() == In.Image.GetHashCode())
            {
                return 1;
            }
            else if (point.GetHashCode() == Out1.Image.GetHashCode())
            {
                return 2;
            }
            else if (point.GetHashCode() == Out2.Image.GetHashCode())
            {
                return 3;
            }
            return 0;
        }

        public Image GetPointByNumber(int number)
        {
            switch (number)
            {
                case 1:
                    return (Image) In.Image;
                case 2:
                    return (Image) Out1.Image;
                case 3:
                    return (Image) Out2.Image;
            }
            return null;
        }
    }
}
