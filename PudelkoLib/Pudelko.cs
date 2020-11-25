using System;
using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PudelkoLib
{
    public sealed class Pudelko : IFormattable, IEquatable<Pudelko>, IEnumerable
    {
        public double A
        {
            get
            {
                double result;

                if (_unit == UnitOfMeasure.meter)
                {
                    result = _a;
                }
                else if (_unit == UnitOfMeasure.centimeter)
                {
                    result = _a / 100;
                }
                else
                {
                    result = _a / 1000;
                }

                return Math.Truncate(result * 1000) / 1000;
            }
        }
        public double B {
            get
            {
                double result;

                if (_unit == UnitOfMeasure.meter)
                {
                    result = _b;
                }
                else if (_unit == UnitOfMeasure.centimeter)
                {
                    result = _b / 100;
                }
                else
                {
                    result = _b / 1000;
                }

                return Math.Truncate(result * 1000) / 1000;
            }
        }
        public double C {
            get
            {
                double result;

                if (_unit == UnitOfMeasure.meter)
                {
                    result = _c;
                }
                else if (_unit == UnitOfMeasure.centimeter)
                {
                    result = _c / 100;
                }
                else
                {
                    result = _c / 1000;
                }

                return Math.Truncate(result * 1000) / 1000;
            }
        }
        public double Objetosc {
            get
            {
                return Math.Round(A * B * C, 9);
            }
        }

        public double Pole
        {
            get
            {
                return Math.Round(2 * A * B + 2 * A * C + 2 * B * C, 6);
            }
        }

        private readonly double _a;
        private readonly double _b;
        private readonly double _c;
        private readonly UnitOfMeasure _unit;
        private readonly double[] _edge;

        public Pudelko(double? a = null, double? b = null, double? c = null, UnitOfMeasure unit = UnitOfMeasure.meter)
        {
            _unit = unit;
            _a = GetValueOrDefault(a);
            _b = GetValueOrDefault(b);
            _c = GetValueOrDefault(c);
            _edge = new double[] { A, B, C }; 

            if ((A <= 0 || A >= 10) || (B <= 0 || B >= 10) || (C <= 0 || C >= 10)) {
                throw new ArgumentOutOfRangeException();
            }

        }

        public double this[int i] => _edge[i];

        public override string ToString()
        {
            return $"{A:0.000} m × {B:0.000} m × {C:0.000} m";
        }

        public string ToString(string format)
        {
            switch (format)
            {
                case null:
                case "m":
                    return ToString();
                case "cm":
                    var x1 = A * 100;
                    var y1 = B * 100;
                    var z1 = C * 100;
                    return $"{x1:0.0} cm × {y1:0.0} cm × {z1:0.0} cm";
                case "mm":
                    var x2 = A * 1000;
                    var y2 = B * 1000;
                    var z2 = C * 1000;
                    return $"{x2:0} mm × {y2:0} mm × {z2:0} mm";
                default:
                    throw new FormatException();
            }
        }

        
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ToString(format);
        }
        
        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is Pudelko other && Equals(other);
        }

        public IEnumerator GetEnumerator()
        {
            return _edge.GetEnumerator();
        }

        public static Pudelko Parse(string toParse)
        {
            var edgesRegex = new Regex(@"\d(.?)(\d?)+");
            var edgesMatches = edgesRegex.Matches(toParse);
            var unitRegex = new Regex(@"(m|cm|mm)");
            var unitMatch = unitRegex.Match(toParse).ToString();

            var unit = unitMatch switch
            {
                "m" => UnitOfMeasure.meter,
                "cm" => UnitOfMeasure.centimeter,
                "mm" => UnitOfMeasure.milimeter,
                _ => throw new ArgumentException()
            };

            var a = double.Parse(edgesMatches[0].Value, CultureInfo.InvariantCulture);
            var b = double.Parse(edgesMatches[1].Value, CultureInfo.InvariantCulture);
            var c = double.Parse(edgesMatches[2].Value, CultureInfo.InvariantCulture);

            return new Pudelko(a, b, c, unit);
        }

        public bool Equals(Pudelko other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            var one = this.GetHashCode();
            var two = other.GetHashCode();

            return one.Equals(two);
        }

        public static bool operator ==(Pudelko obj1, Pudelko obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(Pudelko obj1, Pudelko obj2)
        {
            return !(obj1.Equals(obj2));
        }

        public static Pudelko operator +(Pudelko obj1, Pudelko obj2)
        {
            double[] Aobj1 = (double[]) obj1;
            double[] Aobj2 = (double[]) obj2;


            var a = Aobj1[0]+Aobj2[0];
            var b = Math.Max(Aobj1[1], Aobj2[1]);
            var c = Math.Max(Aobj1[2], Aobj2[2]);
            return new Pudelko(a, b, c);
        }

        public static explicit operator double[](Pudelko pudelko)
        {
            return new double[] { pudelko.A, pudelko.B, pudelko.C };
        }

        public static implicit operator Pudelko((int,int,int) tuple)
        {
            return new Pudelko(tuple.Item1, tuple.Item2, tuple.Item3, UnitOfMeasure.milimeter);
        }
        
        public override int GetHashCode()
        {
            var a = Convert.ToInt32(A);
            var b = Convert.ToInt32(B);
            var c = Convert.ToInt32(C);

            return a+b+c;
        }


        private double GetValueOrDefault(double? value)
        {
            var defaultValue = _unit switch
            {
                UnitOfMeasure.milimeter =>100,
                UnitOfMeasure.centimeter =>10,
                UnitOfMeasure.meter => 0.1,
                _ => throw new ArgumentOutOfRangeException()
            };
            return value ?? defaultValue;
        }

        public static int CompareBoxes(Pudelko pudelko1, Pudelko pudelko2)
        {
            
            if (pudelko1.Objetosc < pudelko2.Objetosc)
            {
                return -1;
            }
            else if (pudelko1.Objetosc > pudelko2.Objetosc)
            {
                return 1;
            }
            else
            {
                if (pudelko1.Pole < pudelko2.Pole)
                {
                    return -1;
                }
                else if (pudelko1.Pole > pudelko2.Pole)
                {
                    return 1;
                }
                else
                {
                    var a = pudelko1.A + pudelko1.B + pudelko1.C;
                    var b = pudelko2.A + pudelko2.B + pudelko2.C;
                    if (a < b)
                    {
                        return -1;
                    }
                    else if(a>b)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
    }
}
