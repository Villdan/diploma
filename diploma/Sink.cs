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
    class Sink : IElement
    {
        public Sink(UIElement image, InPoint inPoint)
        {
            Image = image;
            In = inPoint;
        }

        public InPoint In { get; set; }
        public UIElement Image { get; set; }

        public void Run()
        {
            throw new NotImplementedException();
        }

        public void Move(double mouseX, double mouseY)
        {
            //TODO убрать магические числа, ввести константы.
            Canvas.SetLeft(Image, mouseX);
            Canvas.SetTop(Image, mouseY);
            Canvas.SetTop(In.Image, mouseY + 19);
            Canvas.SetLeft(In.Image, mouseX);
            Line line = null;
            foreach (var connector in MainWindow.Connectors)
            {
                if (!connector.EndPoint.GetHashCode().Equals(In.Image.GetHashCode())) continue;
                line = (Line)connector.Image;
                line.X2 = mouseX + 6;
                line.Y2 = mouseY + 19 + 6;
            }
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
    }
}
