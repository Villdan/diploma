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
    class Delay : IElement
    {
        public Delay(UIElement image, InPoint inPoint, OutPoint outPoint)
        {
            Image = image;
            In = inPoint;
            Out = outPoint;
        }

        public InPoint In { get; set; }
        public OutPoint Out { get; set; }
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
            Canvas.SetTop(Out.Image, mouseY + 19);
            Canvas.SetLeft(Out.Image, mouseX + 50);
            Line line = null;
            foreach (var connector in MainWindow.Connectors)
            {
                if (connector.StartPoint.GetHashCode().Equals(Out.Image.GetHashCode()))
                {
                    line = (Line)connector.Image;
                    line.X1 = mouseX + 50 + 6;
                    line.Y1 = mouseY + 19 + 6;
                }
                if (connector.EndPoint.GetHashCode().Equals(In.Image.GetHashCode()))
                {
                    line = (Line)connector.Image;
                    line.X2 = mouseX + 6;
                    line.Y2 = mouseY + 19 + 6;
                }
            }
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
    }
}
