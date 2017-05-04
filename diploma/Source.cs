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
    class Source : IElement
    {
        public Source(UIElement image, OutPoint outPoint)
        {
            Image = image;
            Out = outPoint;
        }

        public OutPoint Out { get; set; }
        public UIElement Image { get; set; }

        public void Run()
        {
            throw new NotImplementedException();
        }

        public void Move(double mouseX, double mouseY)
        {
            Canvas.SetLeft(Image, mouseX);
            Canvas.SetTop(Image, mouseY);
            Canvas.SetTop(Out.Image, mouseY + 19);
            Canvas.SetLeft(Out.Image, mouseX + 39);
            Line line = null;
            foreach (var connector in MainWindow.Connectors)
            {
                if (!connector.StartPoint.GetHashCode().Equals(Out.Image.GetHashCode())) continue;
                line = (Line) connector.Image;
                //TODO убрать магические числа, ввести константы, 6 - середина точки соединения, т.к. он круг 12x12.
                line.X1 = mouseX + 39 + 6;
                line.Y1 = mouseY + 19 + 6;
            }
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
    }
}
