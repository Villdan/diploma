using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using diploma;

namespace diploma
{
    public partial class MainWindow : Window
    {
        private readonly ElementFactory _elementFactory = new ElementFactory();
        public static readonly List<IElement> ElementsList = new List<IElement>();
        public static readonly List<Connector> Connectors = new List<Connector>();
        private static IElement _movedElement = null;
        private static Image _movedImage = null;
        private static double _distanceBetweenX = 0;
        private static double _distanceBetweenY = 0;

        public MainWindow()
        {
            InitializeComponent();
            _elementFactory.canvas = canvas;
        }
        //TODO при движении элемент не позиционировать к мыши
        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            if (_movedElement != null)
            {
                double coordinateX = (e.GetPosition(canvas).X - _distanceBetweenX)<0? 0: e.GetPosition(canvas).X - _distanceBetweenX;
                double coordinateY = (e.GetPosition(canvas).Y - _distanceBetweenY)<0? 0: e.GetPosition(canvas).Y - _distanceBetweenY;
                _movedElement.Move(coordinateX, coordinateY);
            }
            else
            {
                Canvas.SetLeft(_movedImage, e.GetPosition(canvas).X);
                Canvas.SetTop(_movedImage, e.GetPosition(canvas).Y);
            }
            
        }

        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            if (_movedElement != null)
            {
                _movedElement = null;
                return;
            }
            var img = new Image
            {
                Height = 50,
                Source = createImage.Source
            };
            img.MouseDown += newImage_MouseDown;
            Canvas.SetLeft(img, e.GetPosition((canvas)).X);
            Canvas.SetTop(img, e.GetPosition((canvas)).Y);
            ElementsList.Add(_elementFactory.CreateElement(createImage.Name, img));
            createImage.Visibility = Visibility.Hidden;
        }

        private static IElement FindElementByImage(Image image)
        {
            return ElementsList.FirstOrDefault(element => element.Image.GetHashCode().Equals(image.GetHashCode()));
        }

        private void image_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_elementFactory.createdLine != null) return;
            _movedImage = createImage;
            createImage.Name = ((Image) sender).Name;
            createImage.Source = ((Image) sender).Source;
            createImage.Visibility = Visibility.Hidden;
            DragDrop.DoDragDrop((DependencyObject) sender, ((Image) sender).Source, DragDropEffects.Copy);
        }

        private void Canvas_DragEnter(object sender, DragEventArgs e)
        {
            if (!Equals(_movedImage, createImage)) return;
            createImage.Visibility = Visibility.Visible;
        }

        private void newImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_elementFactory.createdLine != null) return;

            _movedElement = FindElementByImage((Image) sender);
            _movedImage = (Image) _movedElement.Image;
            _distanceBetweenX = e.GetPosition(canvas).X - Canvas.GetLeft((Image) sender);
            _distanceBetweenY = e.GetPosition(canvas).Y - Canvas.GetTop((Image)sender);
            DragDrop.DoDragDrop((DependencyObject) sender, ((Image) sender).Source, DragDropEffects.Move);
        }

        private void Canvas_DragLeave(object sender, DragEventArgs e)
        {
            createImage.Visibility = Visibility.Hidden;
        }
    }
}
