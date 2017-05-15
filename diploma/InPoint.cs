using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace diploma
{
    public class InPoint
    {
        public InPoint(Image image, Label label)
        {
            Image = image;
            Label = label;
        }

        public Label Label { get; set; }
        public Image Image { get; set; }

        public void Move(double mouseX, double mouseY)
        {
            Canvas.SetLeft(Image, mouseX);
            Canvas.SetTop(Image, mouseY);
        }
    }
}
