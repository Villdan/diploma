using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Shapes;

namespace diploma
{
    public class Queue : IElement
    {
        public Queue(UIElement image, InPoint inPoint, OutPoint outPoint, OutPoint throwOut, OutPoint throwOut2)
        {
            Image = image;
            In = inPoint;
            Out = outPoint;
            ThrowOut = throwOut;
            ThrowOut2 = throwOut2;
        }
        public InPoint In { get; set; }
        public OutPoint Out { get; set; }
        public OutPoint ThrowOut { get; set; }
        public OutPoint ThrowOut2 { get; set; }
        public UIElement Image { get; set; }

        private const int InPointTopOffset = 19;
        private const int OutPointTopOffset = 19;
        private const int OutPointLeftOffset = 60;
        private const int ThrowOutPointTopOffset = 5;
        private const int ThrowOutPointLeftOffset = 30;
        private const int ThrowOut2PointTopOffset = 5;
        private const int ThrowOut2PointLeftOffset = 45;
        private int _labelXOffset = 9;
        private int _labelYOffset = 5;
        private int _digitCount = 1;
        private const int ThrowPointLabelOffset = 20;

        private readonly List<Tuple<double,long>> _requests = new List<Tuple<double, long>>();
        public bool TimeLimitThrowOut = true;
        public bool ThrowOutWhenOverflow = false;
        public DelayType DelayType = DelayType.Second;
        public long TimeLimit = 1;
        public int Capacity = 10000;



        public void Run()
        {
            In.Label.Dispatcher.BeginInvoke(
              (Action)(() =>
              {
                  if (((int)Math.Log10(Double.Parse(In.Label.Content.ToString())) + 1) > _digitCount)
                  {
                      _digitCount++;
                      _labelXOffset += 9;
                      Move(Canvas.GetLeft(Image), Canvas.GetTop(Image));
                  }
                  In.Label.Visibility = Visibility.Visible;
              }));
            Out.Label.Dispatcher.BeginInvoke(
              (Action)(() =>
              {
                  Out.Label.Visibility = Visibility.Visible;
              }));
            ThrowOut.Label.Dispatcher.BeginInvoke(
             (Action)(() =>
             {
                 ThrowOut.Label.Visibility = Visibility.Visible;
             }));
            ThrowOut2.Label.Dispatcher.BeginInvoke(
             (Action)(() =>
             {
                 ThrowOut2.Label.Visibility = Visibility.Visible;
             }));
            List<Tuple<double, long>> removeList = new List<Tuple<double, long>>();
            if (TimeLimitThrowOut)
            {
                foreach (var req in _requests)
                {
                    if (req.Item2 < MainWindow.EmulationTime)
                    {
                        removeList.Add(req);
                    }
                }
                foreach (var connector in MainWindow.Connectors)
                {
                    if (_requests.Count == 0)
                    {
                        continue;
                    }
                    if (connector.HaveOutPoint((Image)ThrowOut.Image))
                    {
                        foreach (var req in removeList)
                        {
                            if (!connector.EndElement.InRequest(req.Item1) || (_requests.Count  == Capacity))
                            {
                                MainWindow.ErrorStop(this,ThrowOut.Image);
                            }
                            else
                            {
                                _requests.Remove(req);
                                ThrowOutRequest();
                            }
                        }
                        removeList.Clear();
                    }
                }
            }
            foreach (var connector in MainWindow.Connectors)
            {
                if (_requests.Count == 0)
                {
                    continue;
                }
                if (connector.HaveOutPoint((Image)Out.Image))
                {
                    if (!connector.EndElement.InRequest(_requests[0].Item1))
                    {
                        if ((_requests.Count >= Capacity))
                        {
                            if (ThrowOutWhenOverflow)
                            {
                                
                                    foreach (var throwConnector in MainWindow.Connectors)
                                    {
                                        if (throwConnector.HaveOutPoint((Image)ThrowOut2.Image))
                                        {
                                            if (!throwConnector.EndElement.InRequest(_requests[0].Item1))
                                            {
                                                MainWindow.ErrorStop(this, ThrowOut2.Image);
                                            }
                                            else
                                            {
                                                _requests.Remove(_requests[0]);
                                                ThrowOut2Request();
                                            }
                                        }
                                    }
                            }
                            else
                            {
                                MainWindow.ErrorStop(this, Out.Image);
                            }
                        }
                           
                    }
                    else
                    {
                        _requests.RemoveAt(0);
                        OutRequest();
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
                    _labelXOffset = 9;
                    _digitCount = 1;
                }));
            Out.Label.Dispatcher.BeginInvoke(
              (Action)(() =>
              {
                  Out.Label.Visibility = Visibility.Hidden;
                  Out.Label.Content = "0";
              }));
            ThrowOut.Label.Dispatcher.BeginInvoke(
             (Action)(() =>
             {
                 ThrowOut.Label.Visibility = Visibility.Hidden;
                 ThrowOut.Label.Content = "0";
             }));
            ThrowOut2.Label.Dispatcher.BeginInvoke(
             (Action)(() =>
             {
                 ThrowOut2.Label.Visibility = Visibility.Hidden;
                 ThrowOut2.Label.Content = "0";
             }));
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

            Canvas.SetTop(ThrowOut.Image, mouseY + ThrowOutPointTopOffset);
            Canvas.SetLeft(ThrowOut.Image, mouseX + ThrowOutPointLeftOffset);

            Canvas.SetTop(ThrowOut.Label, mouseY + ThrowOutPointTopOffset - ThrowPointLabelOffset);
            Canvas.SetLeft(ThrowOut.Label, mouseX + ThrowOutPointLeftOffset - _labelYOffset);

            Canvas.SetTop(ThrowOut2.Image, mouseY + ThrowOut2PointTopOffset);
            Canvas.SetLeft(ThrowOut2.Image, mouseX + ThrowOut2PointLeftOffset);

            Canvas.SetTop(ThrowOut2.Label, mouseY + ThrowOut2PointTopOffset - ThrowPointLabelOffset);
            Canvas.SetLeft(ThrowOut2.Label, mouseX + ThrowOut2PointLeftOffset + _labelYOffset);

            Line line = null;
            foreach (var connector in MainWindow.Connectors)
            {
                if (connector.StartPoint.GetHashCode().Equals(Out.Image.GetHashCode()))
                {
                    line = (Line)connector.Image;
                    line.X1 = mouseX + OutPointLeftOffset + ElementFactory.PointMiddle;
                    line.Y1 = mouseY + OutPointTopOffset + ElementFactory.PointMiddle;
                }
                if (connector.StartPoint.GetHashCode().Equals(ThrowOut.Image.GetHashCode()))
                {
                    line = (Line)connector.Image;
                    line.X1 = mouseX + ThrowOutPointLeftOffset + ElementFactory.PointMiddle;
                    line.Y1 = mouseY + ThrowOutPointTopOffset + ElementFactory.PointMiddle;
                }
                if (connector.StartPoint.GetHashCode().Equals(ThrowOut2.Image.GetHashCode()))
                {
                    line = (Line)connector.Image;
                    line.X1 = mouseX + ThrowOut2PointLeftOffset + ElementFactory.PointMiddle;
                    line.Y1 = mouseY + ThrowOut2PointTopOffset + ElementFactory.PointMiddle;
                }
                if (!connector.EndPoint.GetHashCode().Equals(In.Image.GetHashCode())) continue;
                line = (Line)connector.Image;
                line.X2 = mouseX + ElementFactory.PointMiddle;
                line.Y2 = mouseY + InPointTopOffset + ElementFactory.PointMiddle;
            }
        }

        public bool InRequest(double request)
        {
            if (_requests.Count == Capacity)
            {
                return false;
            }
            In.Label.Dispatcher.BeginInvoke(
               (Action)(() =>
               {
                   In.Label.Content = (Int32.Parse(In.Label.Content.ToString()) + 1).ToString();
               }));
            _requests.Add(Tuple.Create(request, MainWindow.EmulationTime + TimeLimit));
            return true;
        }

        public void OutRequest()
        {
            Out.Label.Dispatcher.BeginInvoke(
                (Action)(() =>
                {
                    Out.Label.Content = (Int32.Parse(Out.Label.Content.ToString()) + 1).ToString();
                }));
        }

        private void ThrowOutRequest()
        {
            ThrowOut.Label.Dispatcher.BeginInvoke(
                (Action)(() =>
                {
                    ThrowOut.Label.Content = (Int32.Parse(ThrowOut.Label.Content.ToString()) + 1).ToString();
                }));
        }

        private void ThrowOut2Request()
        {
            ThrowOut2.Label.Dispatcher.BeginInvoke(
                (Action)(() =>
                {
                    ThrowOut2.Label.Content = (Int32.Parse(ThrowOut2.Label.Content.ToString()) + 1).ToString();
                }));
        }

        public bool HavePoint(Image point)
        {
            return In.Image.Equals(point) || Out.Image.Equals(point) || ThrowOut.Image.Equals(point) || ThrowOut2.Image.Equals(point);
        }

        public bool HaveInPoint(Image point)
        {
            return In.Image.Equals(point);
        }

        public bool HaveOutPoint(Image point)
        {
            return Out.Image.Equals(point) || ThrowOut.Image.Equals(point) || ThrowOut2.Image.Equals(point);
        }

        public string GetElementType()
        {
            return "Queue";
        }

        public void Delete(Canvas canvas)
        {
            List<Connector> removedConnectors = new List<Connector>();
            foreach (var connector in MainWindow.Connectors)
            {
                if (connector.HaveInPoint(In.Image) || connector.HaveOutPoint(Out.Image) || connector.HaveOutPoint(ThrowOut.Image) || connector.HaveOutPoint(ThrowOut2.Image))
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
            canvas.Children.Remove(ThrowOut.Image);
            canvas.Children.Remove(ThrowOut.Label);
            canvas.Children.Remove(ThrowOut2.Image);
            canvas.Children.Remove(ThrowOut2.Label);
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
            else if(point.GetHashCode() == ThrowOut.Image.GetHashCode())
            {
                return 3;
            }
            else if(point.GetHashCode() == ThrowOut2.Image.GetHashCode())
            {
                return 4;
            }
            return 0;
        }

        public Image GetPointByNumber(int number)
        {
            if (number == 1)
            {
                return (Image)In.Image;
            }
            else if(number == 2)
            {
                return (Image)Out.Image;
            }
            else if(number == 3)
            {
                return (Image)ThrowOut.Image;
            }
            else if(number == 4)
            {
                return (Image)ThrowOut2.Image;
            }
            return null;
        }
    }
}
