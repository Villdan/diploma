using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace diploma
{
    public interface IElement
    {
        UIElement Image { get; set; }
        void Run();
        void Move(double mouseX, double mouseY);
        bool HavePoint(Image point);
        bool HaveInPoint(Image point);
        bool HaveOutPoint(Image point);

    }
}
