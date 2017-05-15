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
    /// Логика взаимодействия для SelectOutputParametersWindow.xaml
    /// </summary>
    public partial class SelectOutputParametersWindow : Window
    {
        private SelectOutput _selectOutput = null;
        public SelectOutputParametersWindow(SelectOutput selectOutput)
        {
            InitializeComponent();
            _selectOutput = selectOutput;
            Chance.Text = _selectOutput.Chance.ToString();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            _selectOutput.Chance = Double.Parse(Chance.Text);
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
