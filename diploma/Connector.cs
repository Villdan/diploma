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

        public void Move(double mouseX, double mouseY)
        {
            Canvas.SetLeft(Image, mouseX);
            Canvas.SetTop(Image, mouseY);
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
    }
}
