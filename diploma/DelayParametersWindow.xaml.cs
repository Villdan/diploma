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
    /// Логика взаимодействия для DelayParametersWindow.xaml
    /// </summary>
    public partial class DelayParametersWindow : Window
    {
        //TODO Ввести проверки ввода во всех Input во всех окнах
        private Delay _delay = null;
        public DelayParametersWindow(Delay delay)
        {
            InitializeComponent();
            _delay = delay;
            CapacityInput.Text = _delay.Capacity.ToString();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            _delay.Capacity = Int32.Parse(CapacityInput.Text);
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
