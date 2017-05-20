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
            _delay = delay;
            InitializeComponent();
            CapacityInput.Text = _delay.Capacity.ToString();
            SetDistributionTypeBox(_delay.DelayDistributions.DistributionType);
            switch (_delay.DelayType)
            {
                case DelayType.Second:
                    typeOfTime.SelectedIndex = 0;
                    break;
                case DelayType.Minute:
                    typeOfTime.SelectedIndex = 1;
                    break;
                default:
                    typeOfTime.SelectedIndex = 2;
                    break;
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            _delay.Capacity = Int32.Parse(CapacityInput.Text);
            _delay.DelayDistributions.DistributionType = GetDistributionType();
            SetDistributionTypeToElement();
            switch (typeOfTime.SelectedIndex)
            {
                case 0:
                    _delay.DelayType = DelayType.Second;
                    break;
                case 1:
                    _delay.DelayType = DelayType.Minute;
                    break;
                default:
                    _delay.DelayType = DelayType.Hour;
                    break;
            }
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SetDistributionTypeToElement()
        {
            _delay.DelayDistributions.DistributionType = GetDistributionType();
            _delay.DelayDistributions.RewindParameters();
            switch (DistributionComboBox.SelectedIndex)
            {
                case 0:
                    _delay.DelayDistributions.p = double.Parse(Param1.Text);
                    return;
                case 1:
                    _delay.DelayDistributions.p = double.Parse(Param1.Text);
                    _delay.DelayDistributions.q = double.Parse(Param2.Text);
                    _delay.DelayDistributions.min = double.Parse(Param3.Text);
                    _delay.DelayDistributions.max = double.Parse(Param4.Text);
                    return;
                case 2:
                    _delay.DelayDistributions.n = int.Parse(Param1.Text);
                    _delay.DelayDistributions.p = double.Parse(Param2.Text);
                    return;
                case 3:
                    _delay.DelayDistributions.beta = double.Parse(Param1.Text);
                    _delay.DelayDistributions.alpha = double.Parse(Param2.Text);
                    return;
                case 4:
                    _delay.DelayDistributions.shape = double.Parse(Param1.Text);
                    _delay.DelayDistributions.scale = double.Parse(Param2.Text);
                    return;
                case 5:
                    _delay.DelayDistributions.p = double.Parse(Param1.Text);
                    return;
                case 6:
                    _delay.DelayDistributions.lambda = double.Parse(Param1.Text);
                    _delay.DelayDistributions.theta = double.Parse(Param2.Text);
                    return;
                case 7:
                    _delay.DelayDistributions.mean = double.Parse(Param1.Text);
                    _delay.DelayDistributions.sigma = double.Parse(Param2.Text);
                    return;
                case 8:
                    _delay.DelayDistributions.mean = double.Parse(Param1.Text);
                    _delay.DelayDistributions.sigma = double.Parse(Param2.Text);
                    return;
                case 9:
                    _delay.DelayDistributions.alpha = double.Parse(Param1.Text);
                    _delay.DelayDistributions.min = double.Parse(Param2.Text);
                    return;
                case 10:
                    _delay.DelayDistributions.lambda = double.Parse(Param1.Text);
                    return;
                case 11:
                    _delay.DelayDistributions.min = double.Parse(Param1.Text);
                    _delay.DelayDistributions.max = double.Parse(Param2.Text);
                    return;
                case 12:
                    _delay.DelayDistributions.nu = int.Parse(Param1.Text);
                    return;
                case 13:
                    _delay.DelayDistributions.lambda = double.Parse(Param1.Text);
                    return;
                case 14:
                    _delay.DelayDistributions.beta = double.Parse(Param1.Text);
                    _delay.DelayDistributions.m = int.Parse(Param2.Text);
                    return;
                default:
                    return;
            }
        }

        private void SetDistributionTypeBox(DistributionType distribution)
        {
            Param1.Text = "0";
            Param2.Text = "0";
            Param3.Text = "0";
            Param4.Text = "0";
            CheckParam1();
            CheckParam2();
            CheckParam3and4();
            switch (distribution)
            {
                case DistributionType.Bernoulli:
                    DistributionComboBox.SelectedIndex = 0;
                    Param1label.Content = "p";
                    if (_delay.DelayDistributions.DistributionType == distribution)
                    {
                         Param1.Text = _delay.DelayDistributions.p.ToString();
                    }
                    Param1label.Visibility = Visibility.Visible;
                    Param1.Visibility = Visibility.Visible;
                    Param2label.Visibility = Visibility.Hidden;
                    Param2.Visibility = Visibility.Hidden;
                    Param3label.Visibility = Visibility.Hidden;
                    Param3.Visibility = Visibility.Hidden;
                    Param4label.Visibility = Visibility.Hidden;
                    Param4.Visibility = Visibility.Hidden;
                    return;
                case DistributionType.Beta:
                    DistributionComboBox.SelectedIndex = 1;
                    Param1label.Content = "p";
                    Param2label.Content = "q";
                    Param3label.Content = "min";
                    Param4label.Content = "max";
                    if (_delay.DelayDistributions.DistributionType == distribution)
                    {
                        Param1.Text = _delay.DelayDistributions.p.ToString();
                        Param2.Text = _delay.DelayDistributions.q.ToString();
                        Param3.Text = _delay.DelayDistributions.min.ToString();
                        Param4.Text = _delay.DelayDistributions.max.ToString();
                    }

                    Param1label.Visibility = Visibility.Visible;
                    Param1.Visibility = Visibility.Visible;
                    Param2label.Visibility = Visibility.Visible;
                    Param2.Visibility = Visibility.Visible;
                    Param3label.Visibility = Visibility.Visible;
                    Param3.Visibility = Visibility.Visible;
                    Param4label.Visibility = Visibility.Visible;
                    Param4.Visibility = Visibility.Visible;

                    return;
                case DistributionType.Binominal:
                    DistributionComboBox.SelectedIndex = 2;
                    Param1label.Content = "n";
                    Param2label.Content = "p";
                    if (_delay.DelayDistributions.DistributionType == distribution)
                    {
                        Param1.Text = _delay.DelayDistributions.n.ToString();
                        Param2.Text = _delay.DelayDistributions.p.ToString();
                    }

                    Param1label.Visibility = Visibility.Visible; 
                    Param1.Visibility = Visibility.Visible; 
                    Param2label.Visibility = Visibility.Visible; 
                    Param2.Visibility = Visibility.Visible; 
                    
                    Param3label.Visibility = Visibility.Hidden; 
                    Param3.Visibility = Visibility.Hidden; 
                    Param4label.Visibility = Visibility.Hidden; 
                    Param4.Visibility = Visibility.Hidden; 
                    return;
                case DistributionType.Weibull:
                    DistributionComboBox.SelectedIndex = 3;
                    Param1label.Content = "beta";
                    Param2label.Content = "alpha";

                    if (_delay.DelayDistributions.DistributionType == distribution)
                    {
                        Param1.Text = _delay.DelayDistributions.beta.ToString();
                        Param2.Text = _delay.DelayDistributions.alpha.ToString();
                    }

                    Param1label.Visibility = Visibility.Visible;
                    Param1.Visibility = Visibility.Visible;
                    Param2label.Visibility = Visibility.Visible;
                    Param2.Visibility = Visibility.Visible;

                    Param3label.Visibility = Visibility.Hidden; 
                    Param3.Visibility = Visibility.Hidden; 
                    Param4label.Visibility = Visibility.Hidden; 
                    Param4.Visibility = Visibility.Hidden; 
                    return;
                case DistributionType.Gamma:
                    DistributionComboBox.SelectedIndex = 4;
                    Param1label.Content = "shape";
                    Param2label.Content = "scala";

                    if (_delay.DelayDistributions.DistributionType == distribution)
                    {
                        Param1.Text = _delay.DelayDistributions.shape.ToString();
                        Param2.Text = _delay.DelayDistributions.scale.ToString();
                    }

                    Param1label.Visibility = Visibility.Visible;
                    Param1.Visibility = Visibility.Visible;
                    Param2label.Visibility = Visibility.Visible;
                    Param2.Visibility = Visibility.Visible;

                    Param3label.Visibility = Visibility.Hidden; 
                    Param3.Visibility = Visibility.Hidden; 
                    Param4label.Visibility = Visibility.Hidden; 
                    Param4.Visibility = Visibility.Hidden; 
                    return;
                case DistributionType.Geometric:
                    DistributionComboBox.SelectedIndex = 5;
                    Param1label.Content = "p";

                    if (_delay.DelayDistributions.DistributionType == distribution)
                    {
                        Param1.Text = _delay.DelayDistributions.p.ToString();
                    }

                    Param1label.Visibility = Visibility.Visible;
                    Param1.Visibility = Visibility.Visible;

                    Param2label.Visibility = Visibility.Hidden; 
                    Param2.Visibility = Visibility.Hidden; 
                    Param3label.Visibility = Visibility.Hidden; 
                    Param3.Visibility = Visibility.Hidden; 
                    Param4label.Visibility = Visibility.Hidden; 
                    Param4.Visibility = Visibility.Hidden; 
                    return;
                case DistributionType.Cauchy:
                    DistributionComboBox.SelectedIndex = 6;
                    Param1label.Content = "lambda";
                    Param2label.Content = "theta";

                    if (_delay.DelayDistributions.DistributionType == distribution)
                    {
                        Param1.Text = _delay.DelayDistributions.lambda.ToString();
                        Param2.Text = _delay.DelayDistributions.theta.ToString();
                    }

                    Param1label.Visibility = Visibility.Visible;
                    Param1.Visibility = Visibility.Visible;
                    Param2label.Visibility = Visibility.Visible;
                    Param2.Visibility = Visibility.Visible;

                    Param3label.Visibility = Visibility.Hidden; 
                    Param3.Visibility = Visibility.Hidden; 
                    Param4label.Visibility = Visibility.Hidden; 
                    Param4.Visibility = Visibility.Hidden; 
                    return;
                case DistributionType.Lognormal:
                    DistributionComboBox.SelectedIndex = 7;
                    Param1label.Content = "mean";
                    Param2label.Content = "sigma";

                    if (_delay.DelayDistributions.DistributionType == distribution)
                    {
                        Param1.Text = _delay.DelayDistributions.mean.ToString();
                        Param2.Text = _delay.DelayDistributions.sigma.ToString();
                    }

                    Param1label.Visibility = Visibility.Visible;
                    Param1.Visibility = Visibility.Visible;
                    Param2label.Visibility = Visibility.Visible;
                    Param2.Visibility = Visibility.Visible;

                    Param3label.Visibility = Visibility.Hidden; 
                    Param3.Visibility = Visibility.Hidden;
                    Param4label.Visibility = Visibility.Hidden; 
                    Param4.Visibility = Visibility.Hidden; 
                    return;
                case DistributionType.Normal:
                    DistributionComboBox.SelectedIndex = 8;
                    Param1label.Content = "mean";
                    Param2label.Content = "sigma";

                    if (_delay.DelayDistributions.DistributionType == distribution)
                    {
                        Param1.Text = _delay.DelayDistributions.mean.ToString();
                        Param2.Text = _delay.DelayDistributions.sigma.ToString();
                    }

                    Param1label.Visibility = Visibility.Visible;
                    Param1.Visibility = Visibility.Visible;
                    Param2label.Visibility = Visibility.Visible;
                    Param2.Visibility = Visibility.Visible;

                    Param3label.Visibility = Visibility.Hidden; 
                    Param3.Visibility = Visibility.Hidden; 
                    Param4label.Visibility = Visibility.Hidden; 
                    Param4.Visibility = Visibility.Hidden; 
                    return;
                case DistributionType.Pareto:
                    DistributionComboBox.SelectedIndex = 9;
                    Param1label.Content = "alpha";
                    Param2label.Content = "min";

                    if (_delay.DelayDistributions.DistributionType == distribution)
                    {
                        Param1.Text = _delay.DelayDistributions.alpha.ToString();
                        Param2.Text = _delay.DelayDistributions.min.ToString();
                    }

                    Param1label.Visibility = Visibility.Visible;
                    Param1.Visibility = Visibility.Visible;
                    Param2label.Visibility = Visibility.Visible;
                    Param2.Visibility = Visibility.Visible;

                    Param3label.Visibility = Visibility.Hidden; 
                    Param3.Visibility = Visibility.Hidden; 
                    Param4label.Visibility = Visibility.Hidden; 
                    Param4.Visibility = Visibility.Hidden; 
                    return;
                case DistributionType.Poisson:
                    DistributionComboBox.SelectedIndex = 10;
                    Param1label.Content = "lambda";

                    if (_delay.DelayDistributions.DistributionType == distribution)
                    {
                        Param1.Text = _delay.DelayDistributions.lambda.ToString();
                    }

                    Param1label.Visibility = Visibility.Visible;
                    Param1.Visibility = Visibility.Visible;

                    Param2label.Visibility = Visibility.Hidden; 
                    Param2.Visibility = Visibility.Hidden; 
                    Param3label.Visibility = Visibility.Hidden; 
                    Param3.Visibility = Visibility.Hidden; 
                    Param4label.Visibility = Visibility.Hidden; 
                    Param4.Visibility = Visibility.Hidden; 
                    return;
                case DistributionType.Uniform:
                    DistributionComboBox.SelectedIndex = 11;
                    Param1label.Content = "min";
                    Param2label.Content = "max";

                    if (_delay.DelayDistributions.DistributionType == distribution)
                    {
                        Param1.Text = _delay.DelayDistributions.min.ToString();
                        Param2.Text = _delay.DelayDistributions.max.ToString();
                    }

                    Param1label.Visibility = Visibility.Visible;
                    Param1.Visibility = Visibility.Visible;
                    Param2label.Visibility = Visibility.Visible;
                    Param2.Visibility = Visibility.Visible;
                    
                    Param3label.Visibility = Visibility.Hidden; 
                    Param3.Visibility = Visibility.Hidden; 
                    Param4label.Visibility = Visibility.Hidden; 
                    Param4.Visibility = Visibility.Hidden; 
                    return;
                case DistributionType.Chi2:
                    DistributionComboBox.SelectedIndex = 12;
                    Param1label.Content = "nu";

                    if (_delay.DelayDistributions.DistributionType == distribution)
                    {
                        Param1.Text = _delay.DelayDistributions.nu.ToString();
                    }

                    Param1label.Visibility = Visibility.Visible;
                    Param1.Visibility = Visibility.Visible;

                    Param2label.Visibility = Visibility.Hidden; 
                    Param2.Visibility = Visibility.Hidden; 
                    Param3label.Visibility = Visibility.Hidden; 
                    Param3.Visibility = Visibility.Hidden; 
                    Param4label.Visibility = Visibility.Hidden; 
                    Param4.Visibility = Visibility.Hidden; 
                    return;
                case DistributionType.Exponential:
                    DistributionComboBox.SelectedIndex = 13;
                    Param1label.Content = "lambda";
                    Param1.Text = _delay.DelayDistributions.lambda.ToString();

                    if (_delay.DelayDistributions.DistributionType == distribution)
                    {
                        Param1.Text = _delay.DelayDistributions.lambda.ToString();
                    }

                    Param1label.Visibility = Visibility.Visible;
                    Param1.Visibility = Visibility.Visible;

                    Param2label.Visibility = Visibility.Hidden; 
                    Param2.Visibility = Visibility.Hidden; 
                    Param3label.Visibility = Visibility.Hidden; 
                    Param3.Visibility = Visibility.Hidden; 
                    Param4label.Visibility = Visibility.Hidden; 
                    Param4.Visibility = Visibility.Hidden; 
                    return;
                case DistributionType.Erlang:
                    DistributionComboBox.SelectedIndex = 14;
                    Param1label.Content = "beta";
                    Param2label.Content = "m";

                    if (_delay.DelayDistributions.DistributionType == distribution)
                    {
                        Param1.Text = _delay.DelayDistributions.beta.ToString();
                        Param2.Text = _delay.DelayDistributions.m.ToString();
                    }

                    Param1label.Visibility = Visibility.Visible;
                    Param1.Visibility = Visibility.Visible;
                    Param2label.Visibility = Visibility.Visible;
                    Param2.Visibility = Visibility.Visible;

                    Param3label.Visibility = Visibility.Hidden; 
                    Param3.Visibility = Visibility.Hidden; 
                    Param4label.Visibility = Visibility.Hidden; 
                    Param4.Visibility = Visibility.Hidden; 
                    return;
                default:
                    return;
            }
        }
        private DistributionType GetDistributionType()
        {
            switch (DistributionComboBox.SelectedIndex)
            {
                case 0:
                    return DistributionType.Bernoulli;
                case 1:
                    return DistributionType.Beta;
                case 2:
                    return DistributionType.Binominal;
                case 3:
                    return DistributionType.Weibull;
                case 4:
                    return DistributionType.Gamma;
                case 5:
                    return DistributionType.Geometric;
                case 6:
                    return DistributionType.Cauchy;
                case 7:
                    return DistributionType.Lognormal;
                case 8:
                    return DistributionType.Normal;
                case 9:
                    return DistributionType.Pareto;
                case 10:
                    return DistributionType.Poisson;
                case 11:
                    return DistributionType.Uniform;
                case 12:
                    return DistributionType.Chi2;
                case 13:
                    return DistributionType.Exponential;
                case 14:
                    return DistributionType.Erlang;
                default:
                    return 0;
            }
        }

        private void DistributionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetDistributionTypeBox(GetDistributionType());
        }
        private void CapacityInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            int capacity;
            if (int.TryParse(CapacityInput.Text, out capacity))
            {
                if (capacity > 0)
                {
                    _delay.Capacity = capacity;
                    CapacityInput.ToolTip = null;
                    CapacityInput.Background = Brushes.White;
                    return;
                }
            }
            CapacityInput.ToolTip = "Должно быть больше 0";
            CapacityInput.Background = Brushes.Red;
        }

        private void Param1_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckParam1();
        }

        private void Param2_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckParam2();
        }

        private void Param3_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckParam3and4();
        }

        private void Param4_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckParam3and4();
        }

        private void CheckParam1()
        {
            double param1, param2;
            if (DistributionComboBox == null)
            {
                return; ;
            }
            switch (DistributionComboBox.SelectedIndex)
            {
                case 0:
                    {
                        if (double.TryParse(Param1.Text, out param1) && param1 >= 0 && param1 <= 1)
                        {
                            Param1.Background = Brushes.White;
                            Param1.ToolTip = null;
                        }
                        else
                        {
                            Param1.ToolTip = "0 < p < 1";
                            Param1.Background = Brushes.Red;
                        }
                        break;
                    }
                case 1:
                    if (double.TryParse(Param1.Text, out param1) && param1 > 0)
                    {
                        Param1.Background = Brushes.White;
                        Param1.ToolTip = null;
                    }
                    else
                    {
                        Param1.ToolTip = "p > 0";
                        Param1.Background = Brushes.Red;
                    }
                    break;
                case 2:
                    if (double.TryParse(Param1.Text, out param1) && param1 >= 1)
                    {
                        Param1.Background = Brushes.White;
                        Param1.ToolTip = null;
                    }
                    else
                    {
                        Param1.ToolTip = "n >= 0";
                        Param1.Background = Brushes.Red;
                    }
                    break;
                case 3:
                    if (double.TryParse(Param1.Text, out param1) && param1 >= 0)
                    {
                        Param1.Background = Brushes.White;
                        Param1.ToolTip = null;
                    }
                    else
                    {
                        Param1.ToolTip = "alpha >= 0";
                        Param1.Background = Brushes.Red;
                    }
                    break;
                case 4:
                    if (double.TryParse(Param1.Text, out param1) && param1 > 0)
                    {
                        Param1.Background = Brushes.White;
                        Param1.ToolTip = null;
                    }
                    else
                    {
                        Param1.ToolTip = "shape > 0";
                        Param1.Background = Brushes.Red;
                    }
                    break;
                case 5:
                    if (double.TryParse(Param1.Text, out param1) && param1 >= 0 && param1 <= 1)
                    {
                        Param1.Background = Brushes.White;
                        Param1.ToolTip = null;
                    }
                    else
                    {
                        Param1.ToolTip = "0 < p < 1";
                        Param1.Background = Brushes.Red;
                    }
                    break;
                case 6:
                    if (double.TryParse(Param1.Text, out param1))
                    {
                        Param1.Background = Brushes.White;
                        Param1.ToolTip = null;
                    }
                    else
                    {
                        Param1.ToolTip = "Должно быть число";
                        Param1.Background = Brushes.Red;
                    }
                    break;
                case 7:
                    if (double.TryParse(Param1.Text, out param1))
                    {
                        Param1.Background = Brushes.White;
                        Param1.ToolTip = null;
                    }
                    else
                    {
                        Param1.ToolTip = "Должно быть число";
                        Param1.Background = Brushes.Red;
                    }
                    break;
                case 8:
                    if (double.TryParse(Param1.Text, out param1))
                    {
                        Param1.Background = Brushes.White;
                        Param1.ToolTip = null;
                    }
                    else
                    {
                        Param1.ToolTip = "Должно быть число";
                        Param1.Background = Brushes.Red;
                    }
                    break;
                case 9:
                    if (double.TryParse(Param1.Text, out param1))
                    {
                        Param1.Background = Brushes.White;
                        Param1.ToolTip = null;
                    }
                    else
                    {
                        Param1.ToolTip = "Должно быть число";
                        Param1.Background = Brushes.Red;
                    }
                    break;
                case 10:
                    if (double.TryParse(Param1.Text, out param1) && param1 >= 0 && param1 <= Math.Log(double.MaxValue))
                    {
                        Param1.Background = Brushes.White;
                        Param1.ToolTip = null;
                    }
                    else
                    {
                        Param1.ToolTip = "0 <= lambda <= Math.Log(Double.MaxValue)";
                        Param1.Background = Brushes.Red;
                    }
                    break;
                case 11:
                    if (double.TryParse(Param1.Text, out param1) && param1 >= 0 && double.TryParse(Param2.Text, out param2) && param2 > param1)
                    {
                        Param1.Background = Brushes.White;
                        Param1.ToolTip = null;
                        Param2.Background = Brushes.White;
                        Param2.ToolTip = null;
                    }
                    else
                    {
                        Param1.Background = Brushes.Red;
                        Param1.ToolTip = "min < max";
                        Param2.Background = Brushes.Red;
                        Param2.ToolTip = "min < max";
                    }
                    break;
                case 12:
                    if (double.TryParse(Param1.Text, out param1))
                    {
                        Param1.Background = Brushes.White;
                        Param1.ToolTip = null;
                    }
                    else
                    {
                        Param1.ToolTip = "Должно быть число";
                        Param1.Background = Brushes.Red;
                    }
                    break;
                case 13:
                    if (double.TryParse(Param1.Text, out param1) && param1 > 0)
                    {
                        Param1.Background = Brushes.White;
                        Param1.ToolTip = null;
                    }
                    else
                    {
                        Param1.ToolTip = "lambda > 0";
                        Param1.Background = Brushes.Red;
                    }
                    break;
                case 14:
                    if (double.TryParse(Param1.Text, out param1))
                    {
                        Param1.Background = Brushes.White;
                        Param1.ToolTip = null;
                    }
                    else
                    {
                        Param1.ToolTip = "Должно быть число";
                        Param1.Background = Brushes.Red;
                    }
                    break;
                default:
                    break;
            }
        }
        private void CheckParam2()
        {
            double param2, param1;
            if (DistributionComboBox == null)
            {
                return; ;
            }
            switch (DistributionComboBox.SelectedIndex)
            {
                case 1:
                    if (double.TryParse(Param2.Text, out param2) && param2 > 0)
                    {
                        Param2.Background = Brushes.White;
                        Param2.ToolTip = null;
                    }
                    else
                    {
                        Param2.ToolTip = "q > 0";
                        Param2.Background = Brushes.Red;
                    }
                    break;
                case 2:
                    if (double.TryParse(Param2.Text, out param2) && param2 >= 0 && param2 <= 1)
                    {
                        Param2.Background = Brushes.White;
                        Param2.ToolTip = null;
                    }
                    else
                    {
                        Param2.ToolTip = "0 < p < 1";
                        Param2.Background = Brushes.Red;
                    }
                    break;
                case 3:
                    if (double.TryParse(Param2.Text, out param2) && param2 >= 0)
                    {
                        Param2.Background = Brushes.White;
                        Param2.ToolTip = null;
                    }
                    else
                    {
                        Param2.ToolTip = "beta >= 0";
                        Param2.Background = Brushes.Red;
                    }
                    break;
                case 4:
                    if (double.TryParse(Param2.Text, out param2) && param2 > 0)
                    {
                        Param2.Background = Brushes.White;
                        Param2.ToolTip = null;
                    }
                    else
                    {
                        Param2.ToolTip = "scala > 0";
                        Param2.Background = Brushes.Red;
                    }
                    break;
                case 6:
                    if (double.TryParse(Param2.Text, out param2))
                    {
                        Param2.Background = Brushes.White;
                        Param2.ToolTip = null;
                    }
                    else
                    {
                        Param2.ToolTip = "Должно быть числом";
                        Param2.Background = Brushes.Red;
                    }
                    break;
                case 7:
                    if (double.TryParse(Param2.Text, out param2) && param2 > 0)
                    {
                        Param2.Background = Brushes.White;
                        Param2.ToolTip = null;
                    }
                    else
                    {
                        Param2.ToolTip = "sigma > 0";
                        Param2.Background = Brushes.Red;
                    }
                    break;
                case 8:
                    if (double.TryParse(Param2.Text, out param2))
                    {
                        Param2.Background = Brushes.White;
                        Param2.ToolTip = null;
                    }
                    else
                    {
                        Param2.ToolTip = "Должно быть числом";
                        Param2.Background = Brushes.Red;
                    }
                    break;
                case 9:
                    if (double.TryParse(Param2.Text, out param2))
                    {
                        Param2.Background = Brushes.White;
                        Param2.ToolTip = null;
                    }
                    else
                    {
                        Param2.ToolTip = "Должно быть числом";
                        Param2.Background = Brushes.Red;
                    }
                    break;
                case 11:
                    if (double.TryParse(Param1.Text, out param1) && param1 >= 0 && double.TryParse(Param2.Text, out param2) && param2 > param1)
                    {
                        Param1.Background = Brushes.White;
                        Param1.ToolTip = null;
                        Param2.Background = Brushes.White;
                        Param2.ToolTip = null;

                    }
                    else
                    {
                        Param1.Background = Brushes.Red;
                        Param1.ToolTip = "min < max";
                        Param2.Background = Brushes.Red;
                        Param2.ToolTip = "min < max";
                    }
                    break;
                case 14:
                    if (double.TryParse(Param2.Text, out param2) && param2 > 0)
                    {
                        Param2.Background = Brushes.White;
                        Param2.ToolTip = null;
                    }
                    else
                    {
                        Param2.ToolTip = "m > 0";
                        Param2.Background = Brushes.Red;
                    }
                    break;
                default:
                    break;
            }
        }
        private void CheckParam3and4()
        {
            double param3, param4;
            if (DistributionComboBox == null)
            {
                return; ;
            }
            if (DistributionComboBox.SelectedIndex == 1)
            {
                if (double.TryParse(Param3.Text, out param3) && double.TryParse(Param4.Text, out param4) && param4 > param3)
                {
                    Param3.Background = Brushes.White;
                    Param3.ToolTip = null;
                    Param4.Background = Brushes.White;
                    Param4.ToolTip = null;
                }
                else
                {
                    Param3.ToolTip = "min < max";
                    Param3.Background = Brushes.Red;
                    Param4.ToolTip = "min < max";
                    Param4.Background = Brushes.Red;
                }
            }
        }
    }
}
