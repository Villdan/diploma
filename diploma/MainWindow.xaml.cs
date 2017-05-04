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
        private IElement _movedElement = null;
        private Image _movedImage = null;

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
                _movedElement.Move(e.GetPosition(canvas).X,e.GetPosition(canvas).Y);
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
            img.MouseDown += newImage_PreviewMouseDown;
            Canvas.SetLeft(img, e.GetPosition((canvas)).X);
            Canvas.SetTop(img, e.GetPosition((canvas)).Y);
            ElementsList.Add(_elementFactory.CreateElement(createImage.Name, img));
            createImage.Visibility = Visibility.Hidden;
        }

        private IElement FindElementByImage(Image image)
        {
            return ElementsList.FirstOrDefault(element => element.Image.GetHashCode().Equals(image.GetHashCode()));
        }

        private void image_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _movedImage = createImage;
            createImage.Name = ((Image) sender).Name;
            createImage.Source = ((Image)sender).Source;
            createImage.Visibility = Visibility.Hidden;
            DragDrop.DoDragDrop((DependencyObject)sender, ((Image)sender).Source, DragDropEffects.Copy);
        }

        private void Canvas_DragEnter(object sender, DragEventArgs e)
        {
            if (!Equals(_movedImage, createImage)) return;
            createImage.Visibility = Visibility.Visible;
        }

        private void newImage_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _movedElement = FindElementByImage((Image) sender);
            _movedImage = (Image)_movedElement.Image;
            DragDrop.DoDragDrop((DependencyObject)sender, ((Image)sender).Source, DragDropEffects.Move);
        }

        private void Canvas_DragLeave(object sender, DragEventArgs e)
        {
            createImage.Visibility = Visibility.Hidden;
        }
        public static bool ExistOutPoint(Image point)
        {
            return MainWindow.Connectors.Any(element => element.HaveOutPoint(point));
        }
    }
}
