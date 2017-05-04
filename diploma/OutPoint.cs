using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace diploma
{
    public class OutPoint
    {
        public OutPoint(UIElement image)
        {
            Image = image;
        }

        public InPoint StartPoint { get; set; }
        public UIElement Image { get; set; }

        public void Move(double mouseX, double mouseY)
        {
            Canvas.SetLeft(Image, mouseX);
            Canvas.SetTop(Image, mouseY);
        }
    }
}
