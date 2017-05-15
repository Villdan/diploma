using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using diploma.Properties;

namespace diploma
{
    class ElementFactory
    {
        public Canvas canvas = null;
        private const int PointSize = 12;
        public const int PointMiddle = PointSize/2;
        public Line createdLine = null;
        private Connector _createdConnector = null;
        private int _sourceCount = 1;
        private int _sinkCount = 1;
        private int _queueCount = 1;
        private int _delayCount = 1;
        private int _selectOutputCount = 1;

        public void Rewind()
        {
            _sourceCount = 1;
            _sinkCount = 1;
            _queueCount = 1;
            _delayCount = 1;
        }

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
                    ((Image) source.Image).Name = "Source" + _sourceCount;
                    _sourceCount++;
                    source.Move(mouseX,mouseY);
                    return source;
                case "Sink":
                    inPoint = CreateInPoint();
                    inPoint.Image.MouseUp += InPoint_MouseUp;
                    Sink sink = new Sink(image, inPoint);
                    ((Image)sink.Image).Name = "Sink" + _sinkCount;
                    _sinkCount++;
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
                    ((Image)selectOutput.Image).Name = "SelectOutput" + _selectOutputCount;
                    _selectOutputCount++;
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
                    ((Image)queue.Image).Name = "Queue" + _queueCount;
                    _queueCount++;
                    queue.Move(mouseX, mouseY);
                    return queue;
                case "Delay":
                    inPoint = CreateInPoint();
                    inPoint.Image.MouseUp += InPoint_MouseUp;
                    outPoint = CreateOutPoint();
                    outPoint.Image.MouseUp += OutPoint_MouseUp;
                    Delay delay = new Delay(image, inPoint, outPoint);
                    ((Image)delay.Image).Name = "Delay" + _delayCount;
                    _delayCount++;
                    delay.Move(mouseX, mouseY);
                    return delay;
            }
            return null;
        }

        private static Image CreatePointImage()
        {
            ImageSourceConverter source = new ImageSourceConverter();
            return new Image
            {
                Height = PointSize,
                Width = PointSize,
                Source = (ImageSource)source.ConvertFrom("pack://application:,,,/diploma;component/Images/Point.png")
            };
        }

        private static Label CreatePointLable()
        {
            return new Label()
            {
                Content = "0",
                Visibility = Visibility.Hidden,
                Foreground = Brushes.CornflowerBlue
            };
        }

        private InPoint CreateInPoint()
        {
            InPoint inPoint = new InPoint(CreatePointImage(), CreatePointLable());
            canvas.Children.Add(inPoint.Image);
            canvas.Children.Add(inPoint.Label);
            return inPoint;
        }

        private OutPoint CreateOutPoint()
        {
            OutPoint outPoint = new OutPoint(CreatePointImage(), CreatePointLable());
            canvas.Children.Add(outPoint.Image);
            canvas.Children.Add(outPoint.Label);
            return outPoint;
        }

        private IElement FindElementByPointImage(Image point)
        {
            return MainWindow.ElementsList.FirstOrDefault(element => element.HavePoint(point));
        }

        private Line CreateLine(Image start)
        {
            Line line = new Line
            {
                X1 = Canvas.GetLeft(start) + PointMiddle,
                Y1 = Canvas.GetTop(start) + PointMiddle,
                X2 = Canvas.GetLeft(start) + PointMiddle,
                Y2 = Canvas.GetTop(start) + PointMiddle
            };
            SolidColorBrush blackBrush = new SolidColorBrush();
            blackBrush.Color = Colors.Black;
            line.StrokeThickness = 1;
            line.Stroke = blackBrush;
            line.MouseLeftButtonUp += connector_MouseUp;
            return line;
        }

        private void InPoint_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.Emulation != EmulationStatus.Off) return;

            if (_createdConnector == null)
            {
                Line line = CreateLine((Image) sender);
                Connector connector = new Connector(line);
                connector.EndPoint = (Image)sender;
                connector.EndElement = FindElementByPointImage((Image) sender);
                canvas.Children.Add(connector.Image);
                createdLine = line;
                _createdConnector = connector;
                canvas.MouseMove += canvas_MouseMove;
                canvas.MouseRightButtonUp += canvas_MouseRightButtonUp;
            }
            else if (_createdConnector.StartPoint != null && !_createdConnector.StartElement.HavePoint((Image)sender))
            {
                //TODO Добавить вывод ошибки соединения: In to In
                _createdConnector.EndPoint = (Image)sender;
                _createdConnector.EndElement = FindElementByPointImage((Image)sender);
                Line line = (Line) _createdConnector.Image;
                line.X2 = Canvas.GetLeft((Image)sender) + PointMiddle;
                line.Y2 = Canvas.GetTop((Image)sender) + PointMiddle;
                MainWindow.Connectors.Add(_createdConnector);
                canvas.MouseMove -= canvas_MouseMove;
                canvas.MouseRightButtonUp -= canvas_MouseRightButtonUp;
                createdLine = null;
                _createdConnector = null;
            }
        }
        
        private void OutPoint_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.Emulation != EmulationStatus.Off) return;

            if (_createdConnector == null && !ExistOutPoint((Image)sender))
            {
                Line line = CreateLine((Image)sender);
                Connector connector = new Connector(line);
                connector.StartPoint = (Image) sender;
                connector.StartElement = FindElementByPointImage((Image)sender);
                canvas.Children.Add(connector.Image);
                createdLine = line;
                _createdConnector = connector;
                canvas.MouseMove += canvas_MouseMove;
                canvas.MouseRightButtonUp += canvas_MouseRightButtonUp;
            }
            else if (_createdConnector?.EndPoint != null && !ExistOutPoint((Image)sender) && !_createdConnector.EndElement.HavePoint((Image)sender))
            {
                //TODO Добавить вывод ошибки соединения: Out to Out, Out already exist
                _createdConnector.StartPoint = (Image)sender;
                _createdConnector.StartElement = FindElementByPointImage((Image)sender);
                Line line = (Line)_createdConnector.Image;
                line.X1 = Canvas.GetLeft((Image)sender) + PointMiddle;
                line.Y1 = Canvas.GetTop((Image)sender) + PointMiddle;
                MainWindow.Connectors.Add(_createdConnector);
                createdLine = null;
                _createdConnector = null;
                canvas.MouseMove -= canvas_MouseMove;
                canvas.MouseRightButtonUp -= canvas_MouseRightButtonUp;
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_createdConnector.StartPoint == null)
            {
                createdLine.X1 = e.GetPosition(canvas).X + PointMiddle;
                createdLine.Y1 = e.GetPosition(canvas).Y + PointMiddle;
            }
            else
            {
                createdLine.X2 = e.GetPosition(canvas).X + PointMiddle;
                createdLine.Y2 = e.GetPosition(canvas).Y + PointMiddle;
            }
        }

        private void canvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            canvas.MouseMove -= canvas_MouseMove;
            canvas.Children.Remove(_createdConnector.Image);
            canvas.MouseRightButtonUp -= canvas_MouseRightButtonUp;
            _createdConnector = null;
            createdLine = null;
        }

        private bool ExistOutPoint(Image point)
        {
            return MainWindow.Connectors.Any(element => element.HaveOutPoint(point));
        }

        private Connector FindConnectorByImage(Line connector)
        {
            foreach (var connect in MainWindow.Connectors)
            {
                if (connect.Image.GetHashCode() == connector.GetHashCode())
                {
                    return connect;
                }
            }
            return null;
        }

        private void connector_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.Emulation != EmulationStatus.Off) return;
            if (MainWindow.MouseAction == MouseAction.Delete)
            {
                Connector deletedConnector = FindConnectorByImage(((Line)sender));
                canvas.Children.Remove(deletedConnector.Image);
                MainWindow.Connectors.Remove(deletedConnector);
            }
        }

    }
}
