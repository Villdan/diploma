using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace diploma
{
    public class Connector : IElement
    {
        public Connector(UIElement image)
        {
            Image = image;
        }

        public Image StartPoint { get; set; }
        public IElement StartElement { get; set; }
        public Image EndPoint { get; set; }
        public IElement EndElement { get; set; }
        public UIElement Image { get; set; }

        public void Run()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void Move(double mouseX, double mouseY)
        {
            Canvas.SetLeft(Image, mouseX);
            Canvas.SetTop(Image, mouseY);
        }

        public void MakePointRed()
        {
            ImageSourceConverter source = new ImageSourceConverter();
            StartPoint.Dispatcher.Invoke((Action) (() =>
            {
                StartPoint.Source =
                    (ImageSource) source.ConvertFrom("pack://application:,,,/diploma;component/Images/RedPoint.png");
            }));
            EndPoint.Dispatcher.Invoke((Action)(() =>
            {
                EndPoint.Source =
                    (ImageSource) source.ConvertFrom("pack://application:,,,/diploma;component/Images/RedPoint.png");
            }));
        }
        public void MakePointGreen()
        {
            ImageSourceConverter source = new ImageSourceConverter();
            StartPoint.Dispatcher.Invoke((Action)(() =>
            {
                StartPoint.Source =
                    (ImageSource) source.ConvertFrom("pack://application:,,,/diploma;component/Images/Point.png");
            }));
            EndPoint.Dispatcher.Invoke((Action)(() =>
            {
                EndPoint.Source =
                    (ImageSource) source.ConvertFrom("pack://application:,,,/diploma;component/Images/Point.png");
            }));
        }

        public bool InRequest(double request)
        {
            throw new NotImplementedException();
        }

        public void OutRequest()
        {
            throw new NotImplementedException();
        }

        public bool HavePoint(Image point)
        {
            return StartPoint.Equals(point) || EndPoint.Equals(point);
        }

        public bool HaveInPoint(Image point)
        {
            return EndPoint.Equals(point);
        }

        public bool HaveOutPoint(Image point)
        {
            return StartPoint.Equals(point);
        }

        public string GetElementType()
        {
            return "Connector";
        }

        public void Delete(Canvas canvas)
        {
            throw new NotImplementedException();
        }

        public int GetPointNumber(Image point)
        {
            throw new NotImplementedException();
        }

        public Image GetPointByNumber(int number)
        {
            throw new NotImplementedException();
        }
    }
}
