using System;
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
using System.Windows.Shapes;

namespace diploma
{
    /// <summary>
    /// Логика взаимодействия для QueueParametersWindow.xaml
    /// </summary>
    public partial class QueueParametersWindow : Window
    {
        private Queue _queue = null;
        public QueueParametersWindow(Queue queue)
        {
            InitializeComponent();
            _queue = queue;
            CapacityInput.Text = _queue.Capacity.ToString();
            throwOutByTimeout.IsChecked = _queue.TimeLimitThrowOut;
            throwOutCheck.IsChecked = _queue.ThrowOutWhenOverflow;

            if (throwOutByTimeout.IsChecked == false)
            {
                Timeout.Visibility = Visibility.Hidden;
                typeOfTime.Visibility = Visibility.Hidden;
                timeLabel.Visibility = Visibility.Hidden;
            }

            Timeout.Text = _queue.TimeLimit.ToString();
            if (_queue.DelayType == DelayType.Second)
            {
                typeOfTime.SelectedIndex = 0;
            }
            else if (_queue.DelayType == DelayType.Minute)
            {
                typeOfTime.SelectedIndex = 1;
            }
            else
            {
                typeOfTime.SelectedIndex = 2;
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(typeOfTime.SelectedIndex);
            _queue.Capacity = Int32.Parse(CapacityInput.Text);
            _queue.ThrowOutWhenOverflow = throwOutCheck.IsChecked.Value;
            _queue.TimeLimitThrowOut = throwOutByTimeout.IsChecked.Value;
            if (_queue.TimeLimitThrowOut)
            {
                _queue.TimeLimit = Int32.Parse(Timeout.Text);
                if (typeOfTime.SelectedIndex == 0)
                {
                    _queue.DelayType = DelayType.Second;
                }
                else if (typeOfTime.SelectedIndex == 1)
                {
                    _queue.DelayType = DelayType.Minute;
                }
                else
                {
                    _queue.DelayType = DelayType.Hour;
                }
            }
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void throwOutByTimeout_Checked(object sender, RoutedEventArgs e)
        {
           
                Timeout.Visibility = Visibility.Visible;
                typeOfTime.Visibility = Visibility.Visible;
                timeLabel.Visibility = Visibility.Visible;
            }

        private void throwOutByTimeout_Unchecked(object sender, RoutedEventArgs e)
        {
             Timeout.Visibility = Visibility.Hidden;
                typeOfTime.Visibility = Visibility.Hidden;
                timeLabel.Visibility = Visibility.Hidden;
        }
    }
}
