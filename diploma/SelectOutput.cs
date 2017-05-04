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
    class SelectOutput : IElement
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


        public void Run()
        {
            throw new NotImplementedException();
        }

        public void Move(double mouseX, double mouseY)
        {
            //TODO убрать магические числа, ввести константы.
            Canvas.SetLeft(Image, mouseX);
            Canvas.SetTop(Image, mouseY);
            Canvas.SetTop(In.Image, mouseY + 20);
            Canvas.SetLeft(In.Image, mouseX);
            Canvas.SetTop(Out1.Image, mouseY + 20);
            Canvas.SetLeft(Out1.Image, mouseX + 40);
            Canvas.SetTop(Out2.Image, mouseY + 40);
            Canvas.SetLeft(Out2.Image, mouseX + 20);
            Line line = null;
            foreach (var connector in MainWindow.Connectors)
            {
                if (connector.StartPoint.GetHashCode().Equals(Out1.Image.GetHashCode()))
                {
                    line = (Line)connector.Image;
                    line.X1 = mouseX + 40 + 6;
                    line.Y1 = mouseY + 20 + 6;
                }
                if (connector.StartPoint.GetHashCode().Equals(Out2.Image.GetHashCode()))
                {
                    line = (Line)connector.Image;
                    line.X1 = mouseX + 20 + 6;
                    line.Y1 = mouseY + 40 + 6;
                }
                if (connector.EndPoint.GetHashCode().Equals(In.Image.GetHashCode()))
                {
                    line = (Line)connector.Image;
                    line.X2 = mouseX + 6;
                    line.Y2 = mouseY + 20 + 6;
                }
            }
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
    }
}
