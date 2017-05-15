using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
        void Stop();
        void Move(double mouseX, double mouseY);
        bool InRequest(double request);
        void OutRequest();
        bool HavePoint(Image point);
        bool HaveInPoint(Image point);
        bool HaveOutPoint(Image point);
        string GetElementType();
        void Delete(Canvas canvas);
        int GetPointNumber(Image point);
        Image GetPointByNumber(int number);

    }
}
