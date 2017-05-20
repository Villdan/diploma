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
    /// Логика взаимодействия для SourceParametersWindow.xaml
    /// </summary>
    public partial class SourceParametersWindow : Window
    {
        private Source _source = null; 

        public SourceParametersWindow(Source source)
        {
            _source = source;
            InitializeComponent();

            SetDistributionTypeBox(_source.ElementDistributions.DistributionType);
            SetDistributionTimeTypeBox(_source.TimeDistributions.DistributionType);
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            _source.ElementDistributions.DistributionType = GetDistributionType();
            SetDistributionTypeToElement();
            _source.TimeDistributions.DistributionType = GetDistributionTimeType();
            SetDistributionTypeToTime();
            switch (typeOfTime.SelectedIndex)
            {
                case 0:
                    _source.DelayType = DelayType.Second;
                    break;
                case 1:
                    _source.DelayType = DelayType.Minute;
                    break;
                default:
                    _source.DelayType = DelayType.Hour;
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
            _source.ElementDistributions.DistributionType = GetDistributionType();
            _source.ElementDistributions.RewindParameters();
            switch (DistributionComboBox_Elem.SelectedIndex)
            {
                case 0:
                    _source.ElementDistributions.p = double.Parse(Param1_Elem.Text);
                    return;
                case 1:
                    _source.ElementDistributions.p = double.Parse(Param1_Elem.Text);
                    _source.ElementDistributions.q = double.Parse(Param2_Elem.Text);
                    _source.ElementDistributions.min = double.Parse(Param3_Elem.Text);
                    _source.ElementDistributions.max = double.Parse(Param4_Elem.Text);
                    return;
                case 2:
                    _source.ElementDistributions.n = int.Parse(Param1_Elem.Text);
                    _source.ElementDistributions.p = double.Parse(Param2_Elem.Text);
                    return;
                case 3:
                    _source.ElementDistributions.beta = double.Parse(Param1_Elem.Text);
                    _source.ElementDistributions.alpha = double.Parse(Param2_Elem.Text);
                    return;
                case 4:
                    _source.ElementDistributions.shape = double.Parse(Param1_Elem.Text);
                    _source.ElementDistributions.scale = double.Parse(Param2_Elem.Text);
                    return;
                case 5:
                    _source.ElementDistributions.p = double.Parse(Param1_Elem.Text);
                    return;
                case 6:
                    _source.ElementDistributions.lambda = double.Parse(Param1_Elem.Text);
                    _source.ElementDistributions.theta = double.Parse(Param2_Elem.Text);
                    return;
                case 7:
                    _source.ElementDistributions.mean = double.Parse(Param1_Elem.Text);
                    _source.ElementDistributions.sigma = double.Parse(Param2_Elem.Text);
                    return;
                case 8:
                    _source.ElementDistributions.mean = double.Parse(Param1_Elem.Text);
                    _source.ElementDistributions.sigma = double.Parse(Param2_Elem.Text);
                    return;
                case 9:
                    _source.ElementDistributions.alpha = double.Parse(Param1_Elem.Text);
                    _source.ElementDistributions.min = double.Parse(Param2_Elem.Text);
                    return;
                case 10:
                    _source.ElementDistributions.lambda = double.Parse(Param1_Elem.Text);
                    return;
                case 11:
                    _source.ElementDistributions.min = double.Parse(Param1_Elem.Text);
                    _source.ElementDistributions.max = double.Parse(Param2_Elem.Text);
                    return;
                case 12:
                    _source.ElementDistributions.nu = int.Parse(Param1_Elem.Text);
                    return;
                case 13:
                    _source.ElementDistributions.lambda = double.Parse(Param1_Elem.Text);
                    return;
                case 14:
                    _source.ElementDistributions.beta = double.Parse(Param1_Elem.Text);
                    _source.ElementDistributions.m = int.Parse(Param2_Elem.Text);
                    return;
                default:
                    return;
            }
        }

        private void SetDistributionTypeBox(DistributionType distribution)
        {
            Param1_Elem.Text = "0";
            Param2_Elem.Text = "0";
            Param3_Elem.Text = "0";
            Param4_Elem.Text = "0";
            CheckElemParam1();
            CheckElemParam2();
            CheckElemParam3and4();
            switch (distribution)
            {
                case DistributionType.Bernoulli:
                    DistributionComboBox_Elem.SelectedIndex = 0;
                    Param1label_Elem.Content = "p";
                    if (_source.ElementDistributions.DistributionType == distribution)
                    {
                        Param1_Elem.Text = _source.ElementDistributions.p.ToString();
                    }
                    Param1label_Elem.Visibility = Visibility.Visible;
                    Param1_Elem.Visibility = Visibility.Visible;
                    Param2label_Elem.Visibility = Visibility.Hidden;
                    Param2_Elem.Visibility = Visibility.Hidden;
                    Param3label_Elem.Visibility = Visibility.Hidden;
                    Param3_Elem.Visibility = Visibility.Hidden;
                    Param4label_Elem.Visibility = Visibility.Hidden;
                    Param4_Elem.Visibility = Visibility.Hidden;
                    return;
                case DistributionType.Beta:
                    DistributionComboBox_Elem.SelectedIndex = 1;
                    Param1label_Elem.Content = "p";
                    Param2label_Elem.Content = "q";
                    Param3label_Elem.Content = "min";
                    Param4label_Elem.Content = "max";
                    if (_source.ElementDistributions.DistributionType == distribution)
                    {
                        Param1_Elem.Text = _source.ElementDistributions.p.ToString();
                        Param2_Elem.Text = _source.ElementDistributions.q.ToString();
                        Param3_Elem.Text = _source.ElementDistributions.min.ToString();
                        Param4_Elem.Text = _source.ElementDistributions.max.ToString();
                    }

                    Param1label_Elem.Visibility = Visibility.Visible;
                    Param1_Elem.Visibility = Visibility.Visible;
                    Param2label_Elem.Visibility = Visibility.Visible;
                    Param2_Elem.Visibility = Visibility.Visible;
                    Param3label_Elem.Visibility = Visibility.Visible;
                    Param3_Elem.Visibility = Visibility.Visible;
                    Param4label_Elem.Visibility = Visibility.Visible;
                    Param4_Elem.Visibility = Visibility.Visible;

                    return;
                case DistributionType.Binominal:
                    DistributionComboBox_Elem.SelectedIndex = 2;
                    Param1label_Elem.Content = "n";
                    Param2label_Elem.Content = "p";
                    if (_source.ElementDistributions.DistributionType == distribution)
                    {
                        Param1_Elem.Text = _source.ElementDistributions.n.ToString();
                        Param2_Elem.Text = _source.ElementDistributions.p.ToString();
                    }

                    Param1label_Elem.Visibility = Visibility.Visible;
                    Param1_Elem.Visibility = Visibility.Visible;
                    Param2label_Elem.Visibility = Visibility.Visible;
                    Param2_Elem.Visibility = Visibility.Visible;

                    Param3label_Elem.Visibility = Visibility.Hidden;
                    Param3_Elem.Visibility = Visibility.Hidden;
                    Param4label_Elem.Visibility = Visibility.Hidden;
                    Param4_Elem.Visibility = Visibility.Hidden;
                    return;
                case DistributionType.Weibull:
                    DistributionComboBox_Elem.SelectedIndex = 3;
                    Param1label_Elem.Content = "beta";
                    Param2label_Elem.Content = "alpha";

                    if (_source.ElementDistributions.DistributionType == distribution)
                    {
                        Param1_Elem.Text = _source.ElementDistributions.beta.ToString();
                        Param2_Elem.Text = _source.ElementDistributions.alpha.ToString();
                    }

                    Param1label_Elem.Visibility = Visibility.Visible;
                    Param1_Elem.Visibility = Visibility.Visible;
                    Param2label_Elem.Visibility = Visibility.Visible;
                    Param2_Elem.Visibility = Visibility.Visible;

                    Param3label_Elem.Visibility = Visibility.Hidden;
                    Param3_Elem.Visibility = Visibility.Hidden;
                    Param4label_Elem.Visibility = Visibility.Hidden;
                    Param4_Elem.Visibility = Visibility.Hidden;
                    return;
                case DistributionType.Gamma:
                    DistributionComboBox_Elem.SelectedIndex = 4;
                    Param1label_Elem.Content = "shape";
                    Param2label_Elem.Content = "scala";

                    if (_source.ElementDistributions.DistributionType == distribution)
                    {
                        Param1_Elem.Text = _source.ElementDistributions.shape.ToString();
                        Param2_Elem.Text = _source.ElementDistributions.scale.ToString();
                    }

                    Param1label_Elem.Visibility = Visibility.Visible;
                    Param1_Elem.Visibility = Visibility.Visible;
                    Param2label_Elem.Visibility = Visibility.Visible;
                    Param2_Elem.Visibility = Visibility.Visible;

                    Param3label_Elem.Visibility = Visibility.Hidden;
                    Param3_Elem.Visibility = Visibility.Hidden;
                    Param4label_Elem.Visibility = Visibility.Hidden;
                    Param4_Elem.Visibility = Visibility.Hidden;
                    return;
                case DistributionType.Geometric:
                    DistributionComboBox_Elem.SelectedIndex = 5;
                    Param1label_Elem.Content = "p";

                    if (_source.ElementDistributions.DistributionType == distribution)
                    {
                        Param1_Elem.Text = _source.ElementDistributions.p.ToString();
                    }

                    Param1label_Elem.Visibility = Visibility.Visible;
                    Param1_Elem.Visibility = Visibility.Visible;

                    Param2label_Elem.Visibility = Visibility.Hidden;
                    Param2_Elem.Visibility = Visibility.Hidden;
                    Param3label_Elem.Visibility = Visibility.Hidden;
                    Param3_Elem.Visibility = Visibility.Hidden;
                    Param4label_Elem.Visibility = Visibility.Hidden;
                    Param4_Elem.Visibility = Visibility.Hidden;
                    return;
                case DistributionType.Cauchy:
                    DistributionComboBox_Elem.SelectedIndex = 6;
                    Param1label_Elem.Content = "lambda";
                    Param2label_Elem.Content = "theta";

                    if (_source.ElementDistributions.DistributionType == distribution)
                    {
                        Param1_Elem.Text = _source.ElementDistributions.lambda.ToString();
                        Param2_Elem.Text = _source.ElementDistributions.theta.ToString();
                    }

                    Param1label_Elem.Visibility = Visibility.Visible;
                    Param1_Elem.Visibility = Visibility.Visible;
                    Param2label_Elem.Visibility = Visibility.Visible;
                    Param2_Elem.Visibility = Visibility.Visible;

                    Param3label_Elem.Visibility = Visibility.Hidden;
                    Param3_Elem.Visibility = Visibility.Hidden;
                    Param4label_Elem.Visibility = Visibility.Hidden;
                    Param4_Elem.Visibility = Visibility.Hidden;
                    return;
                case DistributionType.Lognormal:
                    DistributionComboBox_Elem.SelectedIndex = 7;
                    Param1label_Elem.Content = "mean";
                    Param2label_Elem.Content = "sigma";

                    if (_source.ElementDistributions.DistributionType == distribution)
                    {
                        Param1_Elem.Text = _source.ElementDistributions.mean.ToString();
                        Param2_Elem.Text = _source.ElementDistributions.sigma.ToString();
                    }

                    Param1label_Elem.Visibility = Visibility.Visible;
                    Param1_Elem.Visibility = Visibility.Visible;
                    Param2label_Elem.Visibility = Visibility.Visible;
                    Param2_Elem.Visibility = Visibility.Visible;

                    Param3label_Elem.Visibility = Visibility.Hidden;
                    Param3_Elem.Visibility = Visibility.Hidden;
                    Param4label_Elem.Visibility = Visibility.Hidden;
                    Param4_Elem.Visibility = Visibility.Hidden;
                    return;
                case DistributionType.Normal:
                    DistributionComboBox_Elem.SelectedIndex = 8;
                    Param1label_Elem.Content = "mean";
                    Param2label_Elem.Content = "sigma";

                    if (_source.ElementDistributions.DistributionType == distribution)
                    {
                        Param1_Elem.Text = _source.ElementDistributions.mean.ToString();
                        Param2_Elem.Text = _source.ElementDistributions.sigma.ToString();
                    }

                    Param1label_Elem.Visibility = Visibility.Visible;
                    Param1_Elem.Visibility = Visibility.Visible;
                    Param2label_Elem.Visibility = Visibility.Visible;
                    Param2_Elem.Visibility = Visibility.Visible;

                    Param3label_Elem.Visibility = Visibility.Hidden;
                    Param3_Elem.Visibility = Visibility.Hidden;
                    Param4label_Elem.Visibility = Visibility.Hidden;
                    Param4_Elem.Visibility = Visibility.Hidden;
                    return;
                case DistributionType.Pareto:
                    DistributionComboBox_Elem.SelectedIndex = 9;
                    Param1label_Elem.Content = "alpha";
                    Param2label_Elem.Content = "min";

                    if (_source.ElementDistributions.DistributionType == distribution)
                    {
                        Param1_Elem.Text = _source.ElementDistributions.alpha.ToString();
                        Param2_Elem.Text = _source.ElementDistributions.min.ToString();
                    }

                    Param1label_Elem.Visibility = Visibility.Visible;
                    Param1_Elem.Visibility = Visibility.Visible;
                    Param2label_Elem.Visibility = Visibility.Visible;
                    Param2_Elem.Visibility = Visibility.Visible;

                    Param3label_Elem.Visibility = Visibility.Hidden;
                    Param3_Elem.Visibility = Visibility.Hidden;
                    Param4label_Elem.Visibility = Visibility.Hidden;
                    Param4_Elem.Visibility = Visibility.Hidden;
                    return;
                case DistributionType.Poisson:
                    DistributionComboBox_Elem.SelectedIndex = 10;
                    Param1label_Elem.Content = "lambda";

                    if (_source.ElementDistributions.DistributionType == distribution)
                    {
                        Param1_Elem.Text = _source.ElementDistributions.lambda.ToString();
                    }

                    Param1label_Elem.Visibility = Visibility.Visible;
                    Param1_Elem.Visibility = Visibility.Visible;

                    Param2label_Elem.Visibility = Visibility.Hidden;
                    Param2_Elem.Visibility = Visibility.Hidden;
                    Param3label_Elem.Visibility = Visibility.Hidden;
                    Param3_Elem.Visibility = Visibility.Hidden;
                    Param4label_Elem.Visibility = Visibility.Hidden;
                    Param4_Elem.Visibility = Visibility.Hidden;
                    return;
                case DistributionType.Uniform:
                    DistributionComboBox_Elem.SelectedIndex = 11;
                    Param1label_Elem.Content = "min";
                    Param2label_Elem.Content = "max";

                    if (_source.ElementDistributions.DistributionType == distribution)
                    {
                        Param1_Elem.Text = _source.ElementDistributions.min.ToString();
                        Param2_Elem.Text = _source.ElementDistributions.max.ToString();
                    }

                    Param1label_Elem.Visibility = Visibility.Visible;
                    Param1_Elem.Visibility = Visibility.Visible;
                    Param2label_Elem.Visibility = Visibility.Visible;
                    Param2_Elem.Visibility = Visibility.Visible;

                    Param3label_Elem.Visibility = Visibility.Hidden;
                    Param3_Elem.Visibility = Visibility.Hidden;
                    Param4label_Elem.Visibility = Visibility.Hidden;
                    Param4_Elem.Visibility = Visibility.Hidden;
                    return;
                case DistributionType.Chi2:
                    DistributionComboBox_Elem.SelectedIndex = 12;
                    Param1label_Elem.Content = "nu";

                    if (_source.ElementDistributions.DistributionType == distribution)
                    {
                        Param1_Elem.Text = _source.ElementDistributions.nu.ToString();
                    }

                    Param1label_Elem.Visibility = Visibility.Visible;
                    Param1_Elem.Visibility = Visibility.Visible;

                    Param2label_Elem.Visibility = Visibility.Hidden;
                    Param2_Elem.Visibility = Visibility.Hidden;
                    Param3label_Elem.Visibility = Visibility.Hidden;
                    Param3_Elem.Visibility = Visibility.Hidden;
                    Param4label_Elem.Visibility = Visibility.Hidden;
                    Param4_Elem.Visibility = Visibility.Hidden;
                    return;
                case DistributionType.Exponential:
                    DistributionComboBox_Elem.SelectedIndex = 13;
                    Param1label_Elem.Content = "lambda";
                    Param1_Elem.Text = _source.ElementDistributions.lambda.ToString();

                    if (_source.ElementDistributions.DistributionType == distribution)
                    {
                        Param1_Elem.Text = _source.ElementDistributions.lambda.ToString();
                    }

                    Param1label_Elem.Visibility = Visibility.Visible;
                    Param1_Elem.Visibility = Visibility.Visible;

                    Param2label_Elem.Visibility = Visibility.Hidden;
                    Param2_Elem.Visibility = Visibility.Hidden;
                    Param3label_Elem.Visibility = Visibility.Hidden;
                    Param3_Elem.Visibility = Visibility.Hidden;
                    Param4label_Elem.Visibility = Visibility.Hidden;
                    Param4_Elem.Visibility = Visibility.Hidden;
                    return;
                case DistributionType.Erlang:
                    DistributionComboBox_Elem.SelectedIndex = 14;
                    Param1label_Elem.Content = "beta";
                    Param2label_Elem.Content = "m";

                    if (_source.ElementDistributions.DistributionType == distribution)
                    {
                        Param1_Elem.Text = _source.ElementDistributions.beta.ToString();
                        Param2_Elem.Text = _source.ElementDistributions.m.ToString();
                    }

                    Param1label_Elem.Visibility = Visibility.Visible;
                    Param1_Elem.Visibility = Visibility.Visible;
                    Param2label_Elem.Visibility = Visibility.Visible;
                    Param2_Elem.Visibility = Visibility.Visible;

                    Param3label_Elem.Visibility = Visibility.Hidden;
                    Param3_Elem.Visibility = Visibility.Hidden;
                    Param4label_Elem.Visibility = Visibility.Hidden;
                    Param4_Elem.Visibility = Visibility.Hidden;
                    return;
                default:
                    return;
            }
        }
        private DistributionType GetDistributionType()
        {
            switch (DistributionComboBox_Elem.SelectedIndex)
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

        private void SetDistributionTypeToTime()
        {
            _source.TimeDistributions.DistributionType = GetDistributionTimeType();
            _source.TimeDistributions.RewindParameters();
            switch (DistributionComboBox_Time.SelectedIndex)
            {
                case 0:
                    _source.TimeDistributions.p = double.Parse(Param1_Time.Text);
                    return;
                case 1:
                    _source.TimeDistributions.p = double.Parse(Param1_Time.Text);
                    _source.TimeDistributions.q = double.Parse(Param2_Time.Text);
                    _source.TimeDistributions.min = double.Parse(Param3_Time.Text);
                    _source.TimeDistributions.max = double.Parse(Param4_Time.Text);
                    return;
                case 2:
                    _source.TimeDistributions.n = int.Parse(Param1_Time.Text);
                    _source.TimeDistributions.p = double.Parse(Param2_Time.Text);
                    return;
                case 3:
                    _source.TimeDistributions.beta = double.Parse(Param1_Time.Text);
                    _source.TimeDistributions.alpha = double.Parse(Param2_Time.Text);
                    return;
                case 4:
                    _source.TimeDistributions.shape = double.Parse(Param1_Time.Text);
                    _source.TimeDistributions.scale = double.Parse(Param2_Time.Text);
                    return;
                case 5:
                    _source.TimeDistributions.p = double.Parse(Param1_Time.Text);
                    return;
                case 6:
                    _source.TimeDistributions.lambda = double.Parse(Param1_Time.Text);
                    _source.TimeDistributions.theta = double.Parse(Param2_Time.Text);
                    return;
                case 7:
                    _source.TimeDistributions.mean = double.Parse(Param1_Time.Text);
                    _source.TimeDistributions.sigma = double.Parse(Param2_Time.Text);
                    return;
                case 8:
                    _source.TimeDistributions.mean = double.Parse(Param1_Time.Text);
                    _source.TimeDistributions.sigma = double.Parse(Param2_Time.Text);
                    return;
                case 9:
                    _source.TimeDistributions.alpha = double.Parse(Param1_Time.Text);
                    _source.TimeDistributions.min = double.Parse(Param2_Time.Text);
                    return;
                case 10:
                    _source.TimeDistributions.lambda = double.Parse(Param1_Time.Text);
                    return;
                case 11:
                    _source.TimeDistributions.min = double.Parse(Param1_Time.Text);
                    _source.TimeDistributions.max = double.Parse(Param2_Time.Text);
                    return;
                case 12:
                    _source.TimeDistributions.nu = int.Parse(Param1_Time.Text);
                    return;
                case 13:
                    _source.TimeDistributions.lambda = double.Parse(Param1_Time.Text);
                    return;
                case 14:
                    _source.TimeDistributions.beta = double.Parse(Param1_Time.Text);
                    _source.TimeDistributions.m = int.Parse(Param2_Time.Text);
                    return;
                default:
                    return;
            }
        }

        private void SetDistributionTimeTypeBox(DistributionType distribution)
        {
            Param1_Time.Text = "0";
            Param2_Time.Text = "0";
            Param3_Time.Text = "0";
            Param4_Time.Text = "0";
            CheckTimeParam1();
            CheckTimeParam2();
            CheckTimeParam3and4();
            switch (distribution)
            {
                case DistributionType.Bernoulli:
                    DistributionComboBox_Time.SelectedIndex = 0;
                    Param1label_Time.Content = "p";
                    if (_source.TimeDistributions.DistributionType == distribution)
                    {
                        Param1_Time.Text = _source.TimeDistributions.p.ToString();
                    }
                    Param1label_Time.Visibility = Visibility.Visible;
                    Param1_Time.Visibility = Visibility.Visible;
                    Param2label_Time.Visibility = Visibility.Hidden;
                    Param2_Time.Visibility = Visibility.Hidden;
                    Param3label_Time.Visibility = Visibility.Hidden;
                    Param3_Time.Visibility = Visibility.Hidden;
                    Param4label_Time.Visibility = Visibility.Hidden;
                    Param4_Time.Visibility = Visibility.Hidden;
                    return;
                case DistributionType.Beta:
                    DistributionComboBox_Time.SelectedIndex = 1;
                    Param1label_Time.Content = "p";
                    Param2label_Time.Content = "q";
                    Param3label_Time.Content = "min";
                    Param4label_Time.Content = "max";
                    if (_source.TimeDistributions.DistributionType == distribution)
                    {
                        Param1_Time.Text = _source.TimeDistributions.p.ToString();
                        Param2_Time.Text = _source.TimeDistributions.q.ToString();
                        Param3_Time.Text = _source.TimeDistributions.min.ToString();
                        Param4_Time.Text = _source.TimeDistributions.max.ToString();
                    }

                    Param1label_Time.Visibility = Visibility.Visible;
                    Param1_Time.Visibility = Visibility.Visible;
                    Param2label_Time.Visibility = Visibility.Visible;
                    Param2_Time.Visibility = Visibility.Visible;
                    Param3label_Time.Visibility = Visibility.Visible;
                    Param3_Time.Visibility = Visibility.Visible;
                    Param4label_Time.Visibility = Visibility.Visible;
                    Param4_Time.Visibility = Visibility.Visible;

                    return;
                case DistributionType.Binominal:
                    DistributionComboBox_Time.SelectedIndex = 2;
                    Param1label_Time.Content = "n";
                    Param2label_Time.Content = "p";
                    if (_source.TimeDistributions.DistributionType == distribution)
                    {
                        Param1_Time.Text = _source.TimeDistributions.n.ToString();
                        Param2_Time.Text = _source.TimeDistributions.p.ToString();
                    }

                    Param1label_Time.Visibility = Visibility.Visible;
                    Param1_Time.Visibility = Visibility.Visible;
                    Param2label_Time.Visibility = Visibility.Visible;
                    Param2_Time.Visibility = Visibility.Visible;

                    Param3label_Time.Visibility = Visibility.Hidden;
                    Param3_Time.Visibility = Visibility.Hidden;
                    Param4label_Time.Visibility = Visibility.Hidden;
                    Param4_Time.Visibility = Visibility.Hidden;
                    return;
                case DistributionType.Weibull:
                    DistributionComboBox_Time.SelectedIndex = 3;
                    Param1label_Time.Content = "beta";
                    Param2label_Time.Content = "alpha";

                    if (_source.TimeDistributions.DistributionType == distribution)
                    {
                        Param1_Time.Text = _source.TimeDistributions.beta.ToString();
                        Param2_Time.Text = _source.TimeDistributions.alpha.ToString();
                    }

                    Param1label_Time.Visibility = Visibility.Visible;
                    Param1_Time.Visibility = Visibility.Visible;
                    Param2label_Time.Visibility = Visibility.Visible;
                    Param2_Time.Visibility = Visibility.Visible;

                    Param3label_Time.Visibility = Visibility.Hidden;
                    Param3_Time.Visibility = Visibility.Hidden;
                    Param4label_Time.Visibility = Visibility.Hidden;
                    Param4_Time.Visibility = Visibility.Hidden;
                    return;
                case DistributionType.Gamma:
                    DistributionComboBox_Time.SelectedIndex = 4;
                    Param1label_Time.Content = "shape";
                    Param2label_Time.Content = "scala";

                    if (_source.TimeDistributions.DistributionType == distribution)
                    {
                        Param1_Time.Text = _source.TimeDistributions.shape.ToString();
                        Param2_Time.Text = _source.TimeDistributions.scale.ToString();
                    }

                    Param1label_Time.Visibility = Visibility.Visible;
                    Param1_Time.Visibility = Visibility.Visible;
                    Param2label_Time.Visibility = Visibility.Visible;
                    Param2_Time.Visibility = Visibility.Visible;

                    Param3label_Time.Visibility = Visibility.Hidden;
                    Param3_Time.Visibility = Visibility.Hidden;
                    Param4label_Time.Visibility = Visibility.Hidden;
                    Param4_Time.Visibility = Visibility.Hidden;
                    return;
                case DistributionType.Geometric:
                    DistributionComboBox_Time.SelectedIndex = 5;
                    Param1label_Time.Content = "p";

                    if (_source.TimeDistributions.DistributionType == distribution)
                    {
                        Param1_Time.Text = _source.TimeDistributions.p.ToString();
                    }

                    Param1label_Time.Visibility = Visibility.Visible;
                    Param1_Time.Visibility = Visibility.Visible;

                    Param2label_Time.Visibility = Visibility.Hidden;
                    Param2_Time.Visibility = Visibility.Hidden;
                    Param3label_Time.Visibility = Visibility.Hidden;
                    Param3_Time.Visibility = Visibility.Hidden;
                    Param4label_Time.Visibility = Visibility.Hidden;
                    Param4_Time.Visibility = Visibility.Hidden;
                    return;
                case DistributionType.Cauchy:
                    DistributionComboBox_Time.SelectedIndex = 6;
                    Param1label_Time.Content = "lambda";
                    Param2label_Time.Content = "theta";

                    if (_source.TimeDistributions.DistributionType == distribution)
                    {
                        Param1_Time.Text = _source.TimeDistributions.lambda.ToString();
                        Param2_Time.Text = _source.TimeDistributions.theta.ToString();
                    }

                    Param1label_Time.Visibility = Visibility.Visible;
                    Param1_Time.Visibility = Visibility.Visible;
                    Param2label_Time.Visibility = Visibility.Visible;
                    Param2_Time.Visibility = Visibility.Visible;

                    Param3label_Time.Visibility = Visibility.Hidden;
                    Param3_Time.Visibility = Visibility.Hidden;
                    Param4label_Time.Visibility = Visibility.Hidden;
                    Param4_Time.Visibility = Visibility.Hidden;
                    return;
                case DistributionType.Lognormal:
                    DistributionComboBox_Time.SelectedIndex = 7;
                    Param1label_Time.Content = "mean";
                    Param2label_Time.Content = "sigma";

                    if (_source.TimeDistributions.DistributionType == distribution)
                    {
                        Param1_Time.Text = _source.TimeDistributions.mean.ToString();
                        Param2_Time.Text = _source.TimeDistributions.sigma.ToString();
                    }

                    Param1label_Time.Visibility = Visibility.Visible;
                    Param1_Time.Visibility = Visibility.Visible;
                    Param2label_Time.Visibility = Visibility.Visible;
                    Param2_Time.Visibility = Visibility.Visible;

                    Param3label_Time.Visibility = Visibility.Hidden;
                    Param3_Time.Visibility = Visibility.Hidden;
                    Param4label_Time.Visibility = Visibility.Hidden;
                    Param4_Time.Visibility = Visibility.Hidden;
                    return;
                case DistributionType.Normal:
                    DistributionComboBox_Time.SelectedIndex = 8;
                    Param1label_Time.Content = "mean";
                    Param2label_Time.Content = "sigma";

                    if (_source.TimeDistributions.DistributionType == distribution)
                    {
                        Param1_Time.Text = _source.TimeDistributions.mean.ToString();
                        Param2_Time.Text = _source.TimeDistributions.sigma.ToString();
                    }

                    Param1label_Time.Visibility = Visibility.Visible;
                    Param1_Time.Visibility = Visibility.Visible;
                    Param2label_Time.Visibility = Visibility.Visible;
                    Param2_Time.Visibility = Visibility.Visible;

                    Param3label_Time.Visibility = Visibility.Hidden;
                    Param3_Time.Visibility = Visibility.Hidden;
                    Param4label_Time.Visibility = Visibility.Hidden;
                    Param4_Time.Visibility = Visibility.Hidden;
                    return;
                case DistributionType.Pareto:
                    DistributionComboBox_Time.SelectedIndex = 9;
                    Param1label_Time.Content = "alpha";
                    Param2label_Time.Content = "min";

                    if (_source.TimeDistributions.DistributionType == distribution)
                    {
                        Param1_Time.Text = _source.TimeDistributions.alpha.ToString();
                        Param2_Time.Text = _source.TimeDistributions.min.ToString();
                    }

                    Param1label_Time.Visibility = Visibility.Visible;
                    Param1_Time.Visibility = Visibility.Visible;
                    Param2label_Time.Visibility = Visibility.Visible;
                    Param2_Time.Visibility = Visibility.Visible;

                    Param3label_Time.Visibility = Visibility.Hidden;
                    Param3_Time.Visibility = Visibility.Hidden;
                    Param4label_Time.Visibility = Visibility.Hidden;
                    Param4_Time.Visibility = Visibility.Hidden;
                    return;
                case DistributionType.Poisson:
                    DistributionComboBox_Time.SelectedIndex = 10;
                    Param1label_Time.Content = "lambda";

                    if (_source.TimeDistributions.DistributionType == distribution)
                    {
                        Param1_Time.Text = _source.TimeDistributions.lambda.ToString();
                    }

                    Param1label_Time.Visibility = Visibility.Visible;
                    Param1_Time.Visibility = Visibility.Visible;

                    Param2label_Time.Visibility = Visibility.Hidden;
                    Param2_Time.Visibility = Visibility.Hidden;
                    Param3label_Time.Visibility = Visibility.Hidden;
                    Param3_Time.Visibility = Visibility.Hidden;
                    Param4label_Time.Visibility = Visibility.Hidden;
                    Param4_Time.Visibility = Visibility.Hidden;
                    return;
                case DistributionType.Uniform:
                    DistributionComboBox_Time.SelectedIndex = 11;
                    Param1label_Time.Content = "min";
                    Param2label_Time.Content = "max";

                    if (_source.TimeDistributions.DistributionType == distribution)
                    {
                        Param1_Time.Text = _source.TimeDistributions.min.ToString();
                        Param2_Time.Text = _source.TimeDistributions.max.ToString();
                    }

                    Param1label_Time.Visibility = Visibility.Visible;
                    Param1_Time.Visibility = Visibility.Visible;
                    Param2label_Time.Visibility = Visibility.Visible;
                    Param2_Time.Visibility = Visibility.Visible;

                    Param3label_Time.Visibility = Visibility.Hidden;
                    Param3_Time.Visibility = Visibility.Hidden;
                    Param4label_Time.Visibility = Visibility.Hidden;
                    Param4_Time.Visibility = Visibility.Hidden;
                    return;
                case DistributionType.Chi2:
                    DistributionComboBox_Time.SelectedIndex = 12;
                    Param1label_Time.Content = "nu";

                    if (_source.TimeDistributions.DistributionType == distribution)
                    {
                        Param1_Time.Text = _source.TimeDistributions.nu.ToString();
                    }

                    Param1label_Time.Visibility = Visibility.Visible;
                    Param1_Time.Visibility = Visibility.Visible;

                    Param2label_Time.Visibility = Visibility.Hidden;
                    Param2_Time.Visibility = Visibility.Hidden;
                    Param3label_Time.Visibility = Visibility.Hidden;
                    Param3_Time.Visibility = Visibility.Hidden;
                    Param4label_Time.Visibility = Visibility.Hidden;
                    Param4_Time.Visibility = Visibility.Hidden;
                    return;
                case DistributionType.Exponential:
                    DistributionComboBox_Time.SelectedIndex = 13;
                    Param1label_Time.Content = "lambda";
                    Param1_Time.Text = _source.TimeDistributions.lambda.ToString();

                    if (_source.TimeDistributions.DistributionType == distribution)
                    {
                        Param1_Time.Text = _source.TimeDistributions.lambda.ToString();
                    }

                    Param1label_Time.Visibility = Visibility.Visible;
                    Param1_Time.Visibility = Visibility.Visible;

                    Param2label_Time.Visibility = Visibility.Hidden;
                    Param2_Time.Visibility = Visibility.Hidden;
                    Param3label_Time.Visibility = Visibility.Hidden;
                    Param3_Time.Visibility = Visibility.Hidden;
                    Param4label_Time.Visibility = Visibility.Hidden;
                    Param4_Time.Visibility = Visibility.Hidden;
                    return;
                case DistributionType.Erlang:
                    DistributionComboBox_Time.SelectedIndex = 14;
                    Param1label_Time.Content = "beta";
                    Param2label_Time.Content = "m";

                    if (_source.TimeDistributions.DistributionType == distribution)
                    {
                        Param1_Time.Text = _source.TimeDistributions.beta.ToString();
                        Param2_Time.Text = _source.TimeDistributions.m.ToString();
                    }

                    Param1label_Time.Visibility = Visibility.Visible;
                    Param1_Time.Visibility = Visibility.Visible;
                    Param2label_Time.Visibility = Visibility.Visible;
                    Param2_Time.Visibility = Visibility.Visible;

                    Param3label_Time.Visibility = Visibility.Hidden;
                    Param3_Time.Visibility = Visibility.Hidden;
                    Param4label_Time.Visibility = Visibility.Hidden;
                    Param4_Time.Visibility = Visibility.Hidden;
                    return;
                default:
                    return;
            }
        }
        private DistributionType GetDistributionTimeType()
        {
            switch (DistributionComboBox_Time.SelectedIndex)
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

        private void DistributionComboBoxTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetDistributionTimeTypeBox(GetDistributionTimeType());
        }

        //ELEM
        private void Param1Elem_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckElemParam1();
        }

        private void Param2Elem_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckElemParam2();
        }

        private void Param3Elem_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckElemParam3and4();
        }

        private void Param4Elem_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckElemParam3and4();
        }

        private void CheckElemParam1()
        {
            double param1, param2;
            if (DistributionComboBox_Elem == null)
            {
                return; ;
            }
            switch (DistributionComboBox_Elem.SelectedIndex)
            {
                case 0:
                    {
                        if (double.TryParse(Param1_Elem.Text, out param1) && param1 >= 0 && param1 <= 1)
                        {
                            Param1_Elem.Background = Brushes.White;
                            Param1_Elem.ToolTip = null;
                        }
                        else
                        {
                            Param1_Elem.ToolTip = "0 < p < 1";
                            Param1_Elem.Background = Brushes.Red;
                        }
                        break;
                    }
                case 1:
                    if (double.TryParse(Param1_Elem.Text, out param1) && param1 > 0)
                    {
                        Param1_Elem.Background = Brushes.White;
                        Param1_Elem.ToolTip = null;
                    }
                    else
                    {
                        Param1_Elem.ToolTip = "p > 0";
                        Param1_Elem.Background = Brushes.Red;
                    }
                    break;
                case 2:
                    if (double.TryParse(Param1_Elem.Text, out param1) && param1 >= 1)
                    {
                        Param1_Elem.Background = Brushes.White;
                        Param1_Elem.ToolTip = null;
                    }
                    else
                    {
                        Param1_Elem.ToolTip = "n >= 0";
                        Param1_Elem.Background = Brushes.Red;
                    }
                    break;
                case 3:
                    if (double.TryParse(Param1_Elem.Text, out param1) && param1 >= 0)
                    {
                        Param1_Elem.Background = Brushes.White;
                        Param1_Elem.ToolTip = null;
                    }
                    else
                    {
                        Param1_Elem.ToolTip = "alpha >= 0";
                        Param1_Elem.Background = Brushes.Red;
                    }
                    break;
                case 4:
                    if (double.TryParse(Param1_Elem.Text, out param1) && param1 > 0)
                    {
                        Param1_Elem.Background = Brushes.White;
                        Param1_Elem.ToolTip = null;
                    }
                    else
                    {
                        Param1_Elem.ToolTip = "shape > 0";
                        Param1_Elem.Background = Brushes.Red;
                    }
                    break;
                case 5:
                    if (double.TryParse(Param1_Elem.Text, out param1) && param1 >= 0 && param1 <= 1)
                    {
                        Param1_Elem.Background = Brushes.White;
                        Param1_Elem.ToolTip = null;
                    }
                    else
                    {
                        Param1_Elem.ToolTip = "0 < p < 1";
                        Param1_Elem.Background = Brushes.Red;
                    }
                    break;
                case 6:
                    if (double.TryParse(Param1_Elem.Text, out param1))
                    {
                        Param1_Elem.Background = Brushes.White;
                        Param1_Elem.ToolTip = null;
                    }
                    else
                    {
                        Param1_Elem.ToolTip = "Должно быть число";
                        Param1_Elem.Background = Brushes.Red;
                    }
                    break;
                case 7:
                    if (double.TryParse(Param1_Elem.Text, out param1))
                    {
                        Param1_Elem.Background = Brushes.White;
                        Param1_Elem.ToolTip = null;
                    }
                    else
                    {
                        Param1_Elem.ToolTip = "Должно быть число";
                        Param1_Elem.Background = Brushes.Red;
                    }
                    break;
                case 8:
                    if (double.TryParse(Param1_Elem.Text, out param1))
                    {
                        Param1_Elem.Background = Brushes.White;
                        Param1_Elem.ToolTip = null;
                    }
                    else
                    {
                        Param1_Elem.ToolTip = "Должно быть число";
                        Param1_Elem.Background = Brushes.Red;
                    }
                    break;
                case 9:
                    if (double.TryParse(Param1_Elem.Text, out param1))
                    {
                        Param1_Elem.Background = Brushes.White;
                        Param1_Elem.ToolTip = null;
                    }
                    else
                    {
                        Param1_Elem.ToolTip = "Должно быть число";
                        Param1_Elem.Background = Brushes.Red;
                    }
                    break;
                case 10:
                    if (double.TryParse(Param1_Elem.Text, out param1) && param1 >= 0 && param1 <= Math.Log(double.MaxValue))
                    {
                        Param1_Elem.Background = Brushes.White;
                        Param1_Elem.ToolTip = null;
                    }
                    else
                    {
                        Param1_Elem.ToolTip = "0 <= lambda <= Math.Log(Double.MaxValue)";
                        Param1_Elem.Background = Brushes.Red;
                    }
                    break;
                case 11:
                    if (double.TryParse(Param1_Elem.Text, out param1) && param1 >= 0 && double.TryParse(Param2_Elem.Text, out param2) && param2 > param1)
                    {
                        Param1_Elem.Background = Brushes.White;
                        Param1_Elem.ToolTip = null;
                        Param2_Elem.Background = Brushes.White;
                        Param2_Elem.ToolTip = null;
                    }
                    else
                    {
                        Param1_Elem.Background = Brushes.Red;
                        Param1_Elem.ToolTip = "min < max";
                        Param2_Elem.Background = Brushes.Red;
                        Param2_Elem.ToolTip = "min < max";
                    }
                    break;
                case 12:
                    if (double.TryParse(Param1_Elem.Text, out param1))
                    {
                        Param1_Elem.Background = Brushes.White;
                        Param1_Elem.ToolTip = null;
                    }
                    else
                    {
                        Param1_Elem.ToolTip = "Должно быть число";
                        Param1_Elem.Background = Brushes.Red;
                    }
                    break;
                case 13:
                    if (double.TryParse(Param1_Elem.Text, out param1) && param1 > 0)
                    {
                        Param1_Elem.Background = Brushes.White;
                        Param1_Elem.ToolTip = null;
                    }
                    else
                    {
                        Param1_Elem.ToolTip = "lambda > 0";
                        Param1_Elem.Background = Brushes.Red;
                    }
                    break;
                case 14:
                    if (double.TryParse(Param1_Elem.Text, out param1))
                    {
                        Param1_Elem.Background = Brushes.White;
                        Param1_Elem.ToolTip = null;
                    }
                    else
                    {
                        Param1_Elem.ToolTip = "Должно быть число";
                        Param1_Elem.Background = Brushes.Red;
                    }
                    break;
                default:
                    break;
            }
        }
        private void CheckElemParam2()
        {
            double param2, param1;
            if (DistributionComboBox_Elem == null)
            {
                return; ;
            }
            switch (DistributionComboBox_Elem.SelectedIndex)
            {
                case 1:
                    if (double.TryParse(Param2_Elem.Text, out param2) && param2 > 0)
                    {
                        Param2_Elem.Background = Brushes.White;
                        Param2_Elem.ToolTip = null;
                    }
                    else
                    {
                        Param2_Elem.ToolTip = "q > 0";
                        Param2_Elem.Background = Brushes.Red;
                    }
                    break;
                case 2:
                    if (double.TryParse(Param2_Elem.Text, out param2) && param2 >= 0 && param2 <= 1)
                    {
                        Param2_Elem.Background = Brushes.White;
                        Param2_Elem.ToolTip = null;
                    }
                    else
                    {
                        Param2_Elem.ToolTip = "0 < p < 1";
                        Param2_Elem.Background = Brushes.Red;
                    }
                    break;
                case 3:
                    if (double.TryParse(Param2_Elem.Text, out param2) && param2 >= 0)
                    {
                        Param2_Elem.Background = Brushes.White;
                        Param2_Elem.ToolTip = null;
                    }
                    else
                    {
                        Param2_Elem.ToolTip = "beta >= 0";
                        Param2_Elem.Background = Brushes.Red;
                    }
                    break;
                case 4:
                    if (double.TryParse(Param2_Elem.Text, out param2) && param2 > 0)
                    {
                        Param2_Elem.Background = Brushes.White;
                        Param2_Elem.ToolTip = null;
                    }
                    else
                    {
                        Param2_Elem.ToolTip = "scala > 0";
                        Param2_Elem.Background = Brushes.Red;
                    }
                    break;
                case 6:
                    if (double.TryParse(Param2_Elem.Text, out param2))
                    {
                        Param2_Elem.Background = Brushes.White;
                        Param2_Elem.ToolTip = null;
                    }
                    else
                    {
                        Param2_Elem.ToolTip = "Должно быть числом";
                        Param2_Elem.Background = Brushes.Red;
                    }
                    break;
                case 7:
                    if (double.TryParse(Param2_Elem.Text, out param2) && param2 > 0)
                    {
                        Param2_Elem.Background = Brushes.White;
                        Param2_Elem.ToolTip = null;
                    }
                    else
                    {
                        Param2_Elem.ToolTip = "sigma > 0";
                        Param2_Elem.Background = Brushes.Red;
                    }
                    break;
                case 8:
                    if (double.TryParse(Param2_Elem.Text, out param2))
                    {
                        Param2_Elem.Background = Brushes.White;
                        Param2_Elem.ToolTip = null;
                    }
                    else
                    {
                        Param2_Elem.ToolTip = "Должно быть числом";
                        Param2_Elem.Background = Brushes.Red;
                    }
                    break;
                case 9:
                    if (double.TryParse(Param2_Elem.Text, out param2))
                    {
                        Param2_Elem.Background = Brushes.White;
                        Param2_Elem.ToolTip = null;
                    }
                    else
                    {
                        Param2_Elem.ToolTip = "Должно быть числом";
                        Param2_Elem.Background = Brushes.Red;
                    }
                    break;
                case 11:
                    if (double.TryParse(Param1_Elem.Text, out param1) && param1 >= 0 && double.TryParse(Param2_Elem.Text, out param2) && param2 > param1)
                    {
                        Param1_Elem.Background = Brushes.White;
                        Param1_Elem.ToolTip = null;
                        Param2_Elem.Background = Brushes.White;
                        Param2_Elem.ToolTip = null;

                    }
                    else
                    {
                        Param1_Elem.Background = Brushes.Red;
                        Param1_Elem.ToolTip = "min < max";
                        Param2_Elem.Background = Brushes.Red;
                        Param2_Elem.ToolTip = "min < max";
                    }
                    break;
                case 14:
                    if (double.TryParse(Param2_Elem.Text, out param2) && param2 > 0)
                    {
                        Param2_Elem.Background = Brushes.White;
                        Param2_Elem.ToolTip = null;
                    }
                    else
                    {
                        Param2_Elem.ToolTip = "m > 0";
                        Param2_Elem.Background = Brushes.Red;
                    }
                    break;
                default:
                    break;
            }
        }
        private void CheckElemParam3and4()
        {
            double param3, param4;
            if (DistributionComboBox_Elem == null)
            {
                return; ;
            }
            if (DistributionComboBox_Elem.SelectedIndex == 1)
            {
                if (double.TryParse(Param3_Elem.Text, out param3) && double.TryParse(Param4_Elem.Text, out param4) && param4 > param3)
                {
                    Param3_Elem.Background = Brushes.White;
                    Param3_Elem.ToolTip = null;
                    Param4_Elem.Background = Brushes.White;
                    Param4_Elem.ToolTip = null;
                }
                else
                {
                    Param3_Elem.ToolTip = "min < max";
                    Param3_Elem.Background = Brushes.Red;
                    Param4_Elem.ToolTip = "min < max";
                    Param4_Elem.Background = Brushes.Red;
                }
            }
        }
        //Time
        private void Param1Time_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckTimeParam1();
        }

        private void Param2Time_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckTimeParam2();
        }

        private void Param3Time_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckTimeParam3and4();
        }

        private void Param4Time_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckTimeParam3and4();
        }

        private void CheckTimeParam1()
        {
            double param1, param2;
            if (DistributionComboBox_Time == null)
            {
                return; ;
            }
            switch (DistributionComboBox_Time.SelectedIndex)
            {
                case 0:
                    {
                        if (double.TryParse(Param1_Time.Text, out param1) && param1 >= 0 && param1 <= 1)
                        {
                            Param1_Time.Background = Brushes.White;
                            Param1_Time.ToolTip = null;
                        }
                        else
                        {
                            Param1_Time.ToolTip = "0 < p < 1";
                            Param1_Time.Background = Brushes.Red;
                        }
                        break;
                    }
                case 1:
                    if (double.TryParse(Param1_Time.Text, out param1) && param1 > 0)
                    {
                        Param1_Time.Background = Brushes.White;
                        Param1_Time.ToolTip = null;
                    }
                    else
                    {
                        Param1_Time.ToolTip = "p > 0";
                        Param1_Time.Background = Brushes.Red;
                    }
                    break;
                case 2:
                    if (double.TryParse(Param1_Time.Text, out param1) && param1 >= 1)
                    {
                        Param1_Time.Background = Brushes.White;
                        Param1_Time.ToolTip = null;
                    }
                    else
                    {
                        Param1_Time.ToolTip = "n >= 0";
                        Param1_Time.Background = Brushes.Red;
                    }
                    break;
                case 3:
                    if (double.TryParse(Param1_Time.Text, out param1) && param1 >= 0)
                    {
                        Param1_Time.Background = Brushes.White;
                        Param1_Time.ToolTip = null;
                    }
                    else
                    {
                        Param1_Time.ToolTip = "alpha >= 0";
                        Param1_Time.Background = Brushes.Red;
                    }
                    break;
                case 4:
                    if (double.TryParse(Param1_Time.Text, out param1) && param1 > 0)
                    {
                        Param1_Time.Background = Brushes.White;
                        Param1_Time.ToolTip = null;
                    }
                    else
                    {
                        Param1_Time.ToolTip = "shape > 0";
                        Param1_Time.Background = Brushes.Red;
                    }
                    break;
                case 5:
                    if (double.TryParse(Param1_Time.Text, out param1) && param1 >= 0 && param1 <= 1)
                    {
                        Param1_Time.Background = Brushes.White;
                        Param1_Time.ToolTip = null;
                    }
                    else
                    {
                        Param1_Time.ToolTip = "0 < p < 1";
                        Param1_Time.Background = Brushes.Red;
                    }
                    break;
                case 6:
                    if (double.TryParse(Param1_Time.Text, out param1))
                    {
                        Param1_Time.Background = Brushes.White;
                        Param1_Time.ToolTip = null;
                    }
                    else
                    {
                        Param1_Time.ToolTip = "Должно быть число";
                        Param1_Time.Background = Brushes.Red;
                    }
                    break;
                case 7:
                    if (double.TryParse(Param1_Time.Text, out param1))
                    {
                        Param1_Time.Background = Brushes.White;
                        Param1_Time.ToolTip = null;
                    }
                    else
                    {
                        Param1_Time.ToolTip = "Должно быть число";
                        Param1_Time.Background = Brushes.Red;
                    }
                    break;
                case 8:
                    if (double.TryParse(Param1_Time.Text, out param1))
                    {
                        Param1_Time.Background = Brushes.White;
                        Param1_Time.ToolTip = null;
                    }
                    else
                    {
                        Param1_Time.ToolTip = "Должно быть число";
                        Param1_Time.Background = Brushes.Red;
                    }
                    break;
                case 9:
                    if (double.TryParse(Param1_Time.Text, out param1))
                    {
                        Param1_Time.Background = Brushes.White;
                        Param1_Time.ToolTip = null;
                    }
                    else
                    {
                        Param1_Time.ToolTip = "Должно быть число";
                        Param1_Time.Background = Brushes.Red;
                    }
                    break;
                case 10:
                    if (double.TryParse(Param1_Time.Text, out param1) && param1 >= 0 && param1 <= Math.Log(double.MaxValue))
                    {
                        Param1_Time.Background = Brushes.White;
                        Param1_Time.ToolTip = null;
                    }
                    else
                    {
                        Param1_Time.ToolTip = "0 <= lambda <= Math.Log(Double.MaxValue)";
                        Param1_Time.Background = Brushes.Red;
                    }
                    break;
                case 11:
                    if (double.TryParse(Param1_Time.Text, out param1) && param1 >= 0 && double.TryParse(Param2_Time.Text, out param2) && param2 > param1)
                    {
                        Param1_Time.Background = Brushes.White;
                        Param1_Time.ToolTip = null;
                        Param2_Time.Background = Brushes.White;
                        Param2_Time.ToolTip = null;
                    }
                    else
                    {
                        Param1_Time.Background = Brushes.Red;
                        Param1_Time.ToolTip = "min < max";
                        Param2_Time.Background = Brushes.Red;
                        Param2_Time.ToolTip = "min < max";
                    }
                    break;
                case 12:
                    if (double.TryParse(Param1_Time.Text, out param1))
                    {
                        Param1_Time.Background = Brushes.White;
                        Param1_Time.ToolTip = null;
                    }
                    else
                    {
                        Param1_Time.ToolTip = "Должно быть число";
                        Param1_Time.Background = Brushes.Red;
                    }
                    break;
                case 13:
                    if (double.TryParse(Param1_Time.Text, out param1) && param1 > 0)
                    {
                        Param1_Time.Background = Brushes.White;
                        Param1_Time.ToolTip = null;
                    }
                    else
                    {
                        Param1_Time.ToolTip = "lambda > 0";
                        Param1_Time.Background = Brushes.Red;
                    }
                    break;
                case 14:
                    if (double.TryParse(Param1_Time.Text, out param1))
                    {
                        Param1_Time.Background = Brushes.White;
                        Param1_Time.ToolTip = null;
                    }
                    else
                    {
                        Param1_Time.ToolTip = "Должно быть число";
                        Param1_Time.Background = Brushes.Red;
                    }
                    break;
                default:
                    break;
            }
        }
        private void CheckTimeParam2()
        {
            double param2, param1;
            if (DistributionComboBox_Time == null)
            {
                return; ;
            }
            switch (DistributionComboBox_Time.SelectedIndex)
            {
                case 1:
                    if (double.TryParse(Param2_Time.Text, out param2) && param2 > 0)
                    {
                        Param2_Time.Background = Brushes.White;
                        Param2_Time.ToolTip = null;
                    }
                    else
                    {
                        Param2_Time.ToolTip = "q > 0";
                        Param2_Time.Background = Brushes.Red;
                    }
                    break;
                case 2:
                    if (double.TryParse(Param2_Time.Text, out param2) && param2 >= 0 && param2 <= 1)
                    {
                        Param2_Time.Background = Brushes.White;
                        Param2_Time.ToolTip = null;
                    }
                    else
                    {
                        Param2_Time.ToolTip = "0 < p < 1";
                        Param2_Time.Background = Brushes.Red;
                    }
                    break;
                case 3:
                    if (double.TryParse(Param2_Time.Text, out param2) && param2 >= 0)
                    {
                        Param2_Time.Background = Brushes.White;
                        Param2_Time.ToolTip = null;
                    }
                    else
                    {
                        Param2_Time.ToolTip = "beta >= 0";
                        Param2_Time.Background = Brushes.Red;
                    }
                    break;
                case 4:
                    if (double.TryParse(Param2_Time.Text, out param2) && param2 > 0)
                    {
                        Param2_Time.Background = Brushes.White;
                        Param2_Time.ToolTip = null;
                    }
                    else
                    {
                        Param2_Time.ToolTip = "scala > 0";
                        Param2_Time.Background = Brushes.Red;
                    }
                    break;
                case 6:
                    if (double.TryParse(Param2_Time.Text, out param2))
                    {
                        Param2_Time.Background = Brushes.White;
                        Param2_Time.ToolTip = null;
                    }
                    else
                    {
                        Param2_Time.ToolTip = "Должно быть числом";
                        Param2_Time.Background = Brushes.Red;
                    }
                    break;
                case 7:
                    if (double.TryParse(Param2_Time.Text, out param2) && param2 > 0)
                    {
                        Param2_Time.Background = Brushes.White;
                        Param2_Time.ToolTip = null;
                    }
                    else
                    {
                        Param2_Time.ToolTip = "sigma > 0";
                        Param2_Time.Background = Brushes.Red;
                    }
                    break;
                case 8:
                    if (double.TryParse(Param2_Time.Text, out param2))
                    {
                        Param2_Time.Background = Brushes.White;
                        Param2_Time.ToolTip = null;
                    }
                    else
                    {
                        Param2_Time.ToolTip = "Должно быть числом";
                        Param2_Time.Background = Brushes.Red;
                    }
                    break;
                case 9:
                    if (double.TryParse(Param2_Time.Text, out param2))
                    {
                        Param2_Time.Background = Brushes.White;
                        Param2_Time.ToolTip = null;
                    }
                    else
                    {
                        Param2_Time.ToolTip = "Должно быть числом";
                        Param2_Time.Background = Brushes.Red;
                    }
                    break;
                case 11:
                    if (double.TryParse(Param1_Time.Text, out param1) && param1 >= 0 && double.TryParse(Param2_Time.Text, out param2) && param2 > param1)
                    {
                        Param1_Time.Background = Brushes.White;
                        Param1_Time.ToolTip = null;
                        Param2_Time.Background = Brushes.White;
                        Param2_Time.ToolTip = null;

                    }
                    else
                    {
                        Param1_Time.Background = Brushes.Red;
                        Param1_Time.ToolTip = "min < max";
                        Param2_Time.Background = Brushes.Red;
                        Param2_Time.ToolTip = "min < max";
                    }
                    break;
                case 14:
                    if (double.TryParse(Param2_Time.Text, out param2) && param2 > 0)
                    {
                        Param2_Time.Background = Brushes.White;
                        Param2_Time.ToolTip = null;
                    }
                    else
                    {
                        Param2_Time.ToolTip = "m > 0";
                        Param2_Time.Background = Brushes.Red;
                    }
                    break;
                default:
                    break;
            }
        }
        private void CheckTimeParam3and4()
        {
            double param3, param4;
            if (DistributionComboBox_Time == null)
            {
                return; ;
            }
            if (DistributionComboBox_Time.SelectedIndex == 1)
            {
                if (double.TryParse(Param3_Time.Text, out param3) && double.TryParse(Param4_Time.Text, out param4) && param4 > param3)
                {
                    Param3_Time.Background = Brushes.White;
                    Param3_Time.ToolTip = null;
                    Param4_Time.Background = Brushes.White;
                    Param4_Time.ToolTip = null;
                }
                else
                {
                    Param3_Time.ToolTip = "min < max";
                    Param3_Time.Background = Brushes.Red;
                    Param4_Time.ToolTip = "min < max";
                    Param4_Time.Background = Brushes.Red;
                }
            }
        }
    }
}
