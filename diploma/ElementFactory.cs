using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace diploma
{
    class ElementFactory
    {
        public Canvas canvas = null;
        private Line _createdLine = null;
        private Connector _createdConnector = null;
        public IElement CreateElement(string name, UIElement image)
        {
            canvas.Children.Add(image);
            double mouseX = Canvas.GetLeft(image);
            double mouseY = Canvas.GetTop(image);
            InPoint inPoint;
            OutPoint outPoint;
            switch (name)
            {
                case "Source":
                    outPoint = CreateOutPoint();
                    outPoint.Image.MouseUp += OutPoint_MouseUp;
                    Source source = new Source(image, outPoint);
                    source.Move(mouseX,mouseY);
                    return source;
                case "Sink":
                    inPoint = CreateInPoint();
                    inPoint.Image.MouseUp += InPoint_MouseUp;
                    Sink sink = new Sink(image, inPoint);
                    sink.Move(mouseX, mouseY);
                    return sink;
                case "SelectOutput":
                    inPoint = CreateInPoint();
                    inPoint.Image.MouseUp += InPoint_MouseUp;
                    outPoint = CreateOutPoint();
                    outPoint.Image.MouseUp += OutPoint_MouseUp;
                    OutPoint outPoint2 = CreateOutPoint();
                    outPoint2.Image.MouseUp += OutPoint_MouseUp;
                    SelectOutput selectOutput = new SelectOutput(image, inPoint, outPoint, outPoint2);
                    selectOutput.Move(mouseX, mouseY);
                    return selectOutput;
                case "Queue":
                    inPoint = CreateInPoint();
                    inPoint.Image.MouseUp += InPoint_MouseUp;
                    outPoint = CreateOutPoint();
                    outPoint.Image.MouseUp += OutPoint_MouseUp;
                    OutPoint throwPoint1 = CreateOutPoint();
                    throwPoint1.Image.MouseUp += OutPoint_MouseUp;
                    OutPoint throwPoint2 = CreateOutPoint();
                    throwPoint2.Image.MouseUp += OutPoint_MouseUp;
                    Queue queue = new Queue(image, inPoint, outPoint, throwPoint1, throwPoint2);
                    queue.Move(mouseX, mouseY);
                    return queue;
                case "Delay":
                    inPoint = CreateInPoint();
                    inPoint.Image.MouseUp += InPoint_MouseUp;
                    outPoint = CreateOutPoint();
                    outPoint.Image.MouseUp += OutPoint_MouseUp;
                    Delay delay = new Delay(image, inPoint, outPoint);
                    delay.Move(mouseX, mouseY);
                    return delay;
            }
            return null;
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        private Image CreatePointImage()
        {
            Image pointImage = new Image
            {
                Height = 12,
                Width = 12,
                Source =
                    new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "images\\Point.png",
                        UriKind.Absolute))
            };
            return pointImage;
        }

        public InPoint CreateInPoint()
        {
            InPoint inPoint = new InPoint(CreatePointImage());
            canvas.Children.Add(inPoint.Image);
            return inPoint;
        }

        public OutPoint CreateOutPoint()
        {
            OutPoint outPoint = new OutPoint(CreatePointImage());
            canvas.Children.Add(outPoint.Image);
            return outPoint;
        }
        //TODO запретить соединение точки элемента со своей же точкой
        private void InPoint_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_createdConnector == null)
            {
                Line line = new Line();
                line.X1 = Canvas.GetLeft((Image)sender) + 6;
                line.Y1 = Canvas.GetTop((Image)sender) + 6;
                line.X2 = Canvas.GetLeft((Image)sender) + 6;
                line.Y2 = Canvas.GetTop((Image)sender) + 6;
                SolidColorBrush blackBrush = new SolidColorBrush();
                blackBrush.Color = Colors.Black;
                line.StrokeThickness = 4;
                line.Stroke = blackBrush;
                Connector connector = new Connector(line);
                connector.EndPoint = (Image)sender;
                canvas.Children.Add(connector.Image);
                _createdLine = line;
                _createdConnector = connector;
                canvas.MouseMove += canvas_MouseMove;
                canvas.MouseRightButtonUp += canvas_MouseRightButtonUp;
            }
            else if (_createdConnector.StartPoint != null)
            {
                //TODO Добавить вывод ошибки соединения: In to In
                _createdConnector.EndPoint = (Image)sender;
                Line line = (Line) _createdConnector.Image;
                line.X2 = Canvas.GetLeft((Image)sender) + 6;
                line.Y2 = Canvas.GetTop((Image)sender) + 6;
                MainWindow.Connectors.Add(_createdConnector);
                canvas.MouseMove -= canvas_MouseMove;
                canvas.MouseRightButtonUp -= canvas_MouseRightButtonUp;
                _createdLine = null;
                _createdConnector = null;
            }
        }
        
        private void OutPoint_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_createdConnector == null && !MainWindow.ExistOutPoint((Image)sender))
            {
                Line line = new Line();
                line.X1 = Canvas.GetLeft((Image) sender)+6;
                line.Y1 = Canvas.GetTop((Image)sender)+6;
                line.X2 = Canvas.GetLeft((Image)sender)+6;
                line.Y2 = Canvas.GetTop((Image)sender)+6;
                SolidColorBrush blackBrush = new SolidColorBrush();
                blackBrush.Color = Colors.Black;
                line.StrokeThickness = 4;
                line.Stroke = blackBrush;
                Connector connector = new Connector(line);
                connector.StartPoint = (Image) sender;
                canvas.Children.Add(connector.Image);
                _createdLine = line;
                _createdConnector = connector;
                canvas.MouseMove += canvas_MouseMove;
                canvas.MouseRightButtonUp += canvas_MouseRightButtonUp;
            }
            else if (_createdConnector?.EndPoint != null && !MainWindow.ExistOutPoint((Image)sender))
            {
                //TODO Добавить вывод ошибки соединения: Out to Out, Out already exist
                _createdConnector.StartPoint = (Image)sender;
                Line line = (Line)_createdConnector.Image;
                line.X1 = Canvas.GetLeft((Image)sender) + 6;
                line.Y1 = Canvas.GetTop((Image)sender) + 6;
                MainWindow.Connectors.Add(_createdConnector);
                _createdLine = null;
                _createdConnector = null;
                canvas.MouseMove -= canvas_MouseMove;
                canvas.MouseRightButtonUp -= canvas_MouseRightButtonUp;
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_createdConnector.StartPoint == null)
            {
                //TODO придумать как сделать линию по мышкой, что бы не мешала соединять и убрать отступы по 10
                _createdLine.X1 = e.GetPosition(canvas).X + 10;
                _createdLine.Y1 = e.GetPosition(canvas).Y + 10;
            }
            else
            {
                _createdLine.X2 = e.GetPosition(canvas).X + 10;
                _createdLine.Y2 = e.GetPosition(canvas).Y + 10;
            }
        }

        private void canvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            canvas.MouseMove -= canvas_MouseMove;
            canvas.Children.Remove(_createdConnector.Image);
            canvas.MouseRightButtonUp -= canvas_MouseRightButtonUp;
            _createdConnector = null;
            _createdLine = null;
        }

    }
}
