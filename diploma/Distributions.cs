using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diploma
{
    public class Distributions
    {
        public double p = 0;
        public double q = 0;
        public double min = 0;
        public double max = 0;
        public double beta = 0;
        public double alpha = 0;
        public double shape = 0;
        public double scale = 0;
        public double lambda = 0;
        public double theta = 0;
        public double mean = 0;
        public double sigma = 0;
        public int n = 0;
        public int nu = 0;
        public int m = 0;
        public DistributionType DistributionType = DistributionType.Uniform;

        private static CryptoRandom _randomNumber;
        public Distributions()
        {
            _randomNumber = new CryptoRandom();
        }

        public void RewindParameters()
        {
            p = 0;
            q = 0;
            min = 0;
            max = 0;
            beta = 0;
            alpha = 0;
            shape = 0;
            scale = 0;
            lambda = 0;
            theta = 0;
            mean = 0;
            sigma = 0;
            n = 0;
            nu = 0;
            m = 0;
        }

        private static double Bernoulli(double p)
        {
            return _randomNumber.NextDouble() <= p ? 1 : 0;
        }

        private static double Binomial(int n, double p)
        {
            double y = 0;
            for (int i = 0; i < n; i++)
            {
                y += Bernoulli(p);
            }
            return y;
        }
        private static double Exponential(double lambda)
        {
            double u = _randomNumber.NextDouble();
            return -lambda * Math.Log(u);
        }

        private static double Geometric(double p)
        {
            double u = _randomNumber.NextDouble();
            return (int)Math.Floor(Math.Log(u) / Math.Log(1 - p));
        }

        private static double Weibull(double beta, double alpha)
        {
            double u = _randomNumber.NextDouble();
            return alpha * Math.Pow(-Math.Log(u), 1 / beta);
        }

        private static double Uniform(double min, double max)
        {
            double u = _randomNumber.NextDouble();
            return min + u * (max - min);
        }

        private static double Poisson(double lambda)
        {
            double a = Math.Exp(-lambda);
            double b = 1;
            for (int i = 0; ; i++)
            {
                b *= _randomNumber.NextDouble();
                if (b < a) return i;
            }
        }

        private static double Normal(double mean, double sigma)
        {
            double r1 = _randomNumber.NextDouble();
            double r2 = _randomNumber.NextDouble();
            var normalNumber = Math.Sqrt(-2 * Math.Log(r1)) * Math.Cos(2 * Math.PI * r2);
            normalNumber = normalNumber * sigma + mean;
            return normalNumber;
        }

        private static double Lognormal(double mean, double sigma)
        {
            double y;
            do
            {
                y = Normal(mean, sigma);
            } while (y < Math.Log(double.MinValue) || y > Math.Log(double.MaxValue));
            return Math.Exp(y);
        }

        private static double Gamma(double shape, double scale)
        {
            if (shape < 1)
            {
                double b = (Math.E + shape) / Math.E;
                while (true)
                {
                    double u1 = _randomNumber.NextDouble();
                    double u2 = _randomNumber.NextDouble();
                    double p = b * u1;
                    if (p > 1)
                    {
                        double y = -Math.Log((b - p) / shape);
                        if (u2 <= Math.Pow(y, shape - 1)) return scale * y;
                    }
                    else
                    {
                        double y = Math.Pow(p, 1 / shape);
                        if (u2 <= Math.Pow(Math.E, -y)) return scale * y;
                    }
                }
            }
            else if (shape > 1)
            {
                double a = 1 / Math.Sqrt(2 * shape - 1);
                double b = shape - Math.Log(4.0D);
                double q = shape + 1 / a;
                double theta = 4.5;
                double d = 1 + Math.Log(theta);
                for (;;)
                {
                    double u1 = _randomNumber.NextDouble();
                    double u2 = _randomNumber.NextDouble();

                    double v = a * Math.Log(u1 / (1.0D - u1));
                    double y = shape * Math.Pow(Math.E, v);
                    double z = u1 * u1 * u2;
                    double w = b + q * v - y;

                    if (w + d - theta * z >= 0) return scale * y;
                    if (w >= Math.Log(z)) return scale * y;
                }
            }
            else
            {
                return Exponential(scale);
            }
        }


        private static double Beta(double p, double q, double min, double max)
        {
            double y1, y2;
            do
            {
                y1 = Gamma(p, 1.0D);
                y2 = Gamma(q, 1.0D);
            } while (y1 + y2 == 0.0D);
            double X = y1 / (y1 + y2);
            return min + (max - min) * X;
        }

        private static double Pareto(double alpha, double min)
        {
            return min * Math.Pow(_randomNumber.NextDouble(), (-1.0 / alpha));
        }

        private static double Cauchy(double lambda, double theta)
        {
            return theta + lambda * Math.Tan(Math.PI * (_randomNumber.NextDouble() - 0.5));
        }

        private static double Chi2(int nu)
        {
            int i;
            double w = 0;
            for (i = 0; i < nu; i++)
            {
                var z = Math.Sqrt(-2.0 * Math.Log(_randomNumber.NextDouble())) * Math.Sin(2.0 * Math.PI * _randomNumber.NextDouble());
                w += z * z;
            }
            return w;
        }
        private static double Erlang(double beta, int m)
        {
            int k = (int)((beta * beta) / m + 0.5);
            k = (k > 0) ? k : 1;
            double a = k / beta;

            double prod = 1.0;
            for (int i = 0; i < k; i++)
                prod *= _randomNumber.NextDouble();
            return -Math.Log(prod) / a;
        }

        public double GetNextRand()
        {
            switch (DistributionType)
            {
                case DistributionType.Bernoulli:
                    return Bernoulli(p);
                case DistributionType.Beta:
                    return Beta(p, q, min, max);
                case DistributionType.Binominal:
                    return Binomial(n, p);
                case DistributionType.Weibull:
                    return Weibull(beta, alpha);
                case DistributionType.Gamma:
                    return Gamma(shape, scale);
                case DistributionType.Geometric:
                    return Geometric(p);
                case DistributionType.Cauchy:
                    return Cauchy(lambda, theta);
                case DistributionType.Lognormal:
                    return Lognormal(mean, sigma);
                case DistributionType.Normal:
                    return Normal(mean, sigma);
                case DistributionType.Pareto:
                    return Pareto(alpha, min);
                case DistributionType.Poisson:
                    return Poisson(lambda);
                case DistributionType.Uniform:
                    return Uniform(min, max);
                case DistributionType.Chi2:
                    return Chi2(nu);
                case DistributionType.Exponential:
                    return Exponential(lambda);
                case DistributionType.Erlang:
                    return Erlang(beta,m);
                default:
                    return 0;
            }
        }
    }
}
