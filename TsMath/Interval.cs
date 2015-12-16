#region License
/*
Copyright (c) Thomas Steinfeld 2015. All rights reserved.
For detailed licensing information see LICENSE in the root folder.
*/
#endregion
using System;
using System.Diagnostics;

namespace TsMath
{
	/// <summary>
	/// Describes an closed interval.
	/// </summary>
	[System.Diagnostics.DebuggerDisplay("{ToString()}")]
	public struct Interval
	{
		#region Constants

		/// <summary>
		/// An empty interval.
		/// </summary>
		public static readonly Interval Empty = new Interval(double.NaN, double.NaN);

		/// <summary>
		/// An interval containing only zero.
		/// </summary>
		public static readonly Interval Zero = new Interval(0, true);

		#endregion

		#region Values
		double a, b;

		#endregion

		#region Properties

		/// <summary>
		/// Returns the lower endpoint of this interval.
		/// </summary>
		public double A => a;

		/// <summary>
		/// Returns the upper endpoint of this interval.
		/// </summary>
		public double B => b;

		/// <summary>
		/// Returns the center (midpoint) of this interval.
		/// </summary>
		public double Center => (a + b) / 2;

		/// <summary>
		/// Returns the length of this interval.
		/// </summary>
		public double Length => b - a;

		/// <summary>
		/// Returns true, if this interval is empty.
		/// </summary>
		public bool IsEmpty => double.IsNaN(a) || double.IsNaN(b) || b < a;

		/// <summary>
		/// Returns true, if this interval is degenerated, containing only one number.
		/// </summary>
		public bool IsNumber => !IsEmpty && a == b;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs an interval from a lower bound <paramref name="a"/> and an upper bound <paramref name="b"/>.
		/// </summary>
		/// <param name="a">Lower bound.</param>
		/// <param name="b">Upper bound.</param>
		/// <exception cref="ArgumentException">If  <paramref name="a"/> &gt; <paramref name="b"/>.</exception>
		public Interval(double a, double b)
		{
			if (a > b)
				throw new ArgumentException("Lower bound is greater than upper bound");
			this.a = a; this.b = b;
		}


		/// <summary>
		/// Constructs a interval from a double number.
		/// </summary>
		/// <remarks>
		/// <see cref="IsNumber"/> is only true, when the interval is constructed with <paramref name="asNumber"/> set to <b>true</b>.
		/// </remarks>
		/// <param name="a">The number.</param>
		/// <param name="asNumber">Indicates if a the resulting interval will be a single number.</param>
		[DebuggerStepThrough]
		public Interval(double a, bool asNumber = false)
		{
			if (asNumber)
			{
				this.a = this.b = a;
			}
			else
			{
				double ulp = a.UnitLastPlace();
				this.a = a - ulp;
				this.b = a + ulp;
			}
		}
		#endregion

		#region Set theoretic methods

		/// <summary>
		/// Checks if this interval contains a number.
		/// </summary>
		/// <param name="d">Value to check for containment.</param>
		/// <returns>True, if this interval contains <paramref name="d"/>; false otherwise.</returns>
		public bool Contains(double d) => a <= d && d <= b;

		/// <summary>
		/// Returns true if this interval contains 0.
		/// </summary>
		public bool ContainsZero => a <= 0 && b >= 0;

		/// <summary>
		/// Checks if this interval contains an other interval.
		/// </summary>
		/// <param name="a">Other interval.</param>
		/// <returns><b>true</b>, if this interval contains the interval <paramref name="a"/>; <b>false</b> otherwise.</returns>
		public bool Contains(Interval a) => Contains(a.a) && Contains(a.b);

		/// <summary>
		/// Checks if this interval intersects with an other.
		/// </summary>
		/// <param name="other">The other interval.</param>
		/// <returns>True, if both intervals intersect; false otherwise.</returns>
		public bool IntercectsWith(Interval other)
		{
			if (this.IsEmpty || other.IsEmpty)
				return false;
			if (other.b < this.a || other.a > this.b)
				return false;
			return true;
		}

		/// <summary>
		/// Calculates the intersection of two intervals.
		/// </summary>
		/// <param name="a">First interval.</param>
		/// <param name="b">Second interval.</param>
		/// <returns>The intersection of both intervals.</returns>
		public static Interval Intersection(Interval a, Interval b)
		{
			if (a.IsEmpty || b.IsEmpty)
				return Empty;
			double min = Math.Max(a.a, b.a);
			double max = Math.Min(a.b, b.b);
			if (min > max)
				return Empty;
			return new Interval(min, max);
		}

		/// <summary>
		/// Calculates the union of two intervals (the smallest interval which contains both intervals).
		/// </summary>
		/// <param name="a">First interval.</param>
		/// <param name="b">Second interval.</param>
		/// <returns>The smallest interval which contains both intervals.</returns>
		public static Interval Union(Interval a, Interval b)
		{
			if (a.IsEmpty)
				return b;
			if (b.IsEmpty)
				return a;
			double min = Math.Min(a.a, b.a);
			double max = Math.Max(a.b, b.b);
			return new Interval(min, max);
		}

		#endregion

		#region Conversion
		/// <summary>
		/// Converts the interval to a string representation.
		/// </summary>
		/// <returns>A string representation of this interval.</returns>
		public override string ToString()
		{
			if (IsEmpty)
				return "[]";
			double center = Center;
			double len = Length;
			double len2 = center.UnitLastPlace();
			if (len > 10 * len2)
				return string.Format("[{0}; {1}]", a, b);
			else
				return center.ToString();
		}

		/// <summary>
		/// Converts a number to an interval.
		/// </summary>
		/// <param name="a">The number.</param>
		/// <returns>The interval [a - ε, a + ε] where ε=<see cref="MathExtensions.UnitLastPlace(double)"/>(a).</returns>
		[DebuggerStepThrough]
		public static implicit operator Interval(double a)
		{
			return new Interval(a, false);
		}
		#endregion

		#region Arithmetic operations

		static internal Interval CreateWithUlp(double a, double b)
		{
			a -= a.UnitLastPlace();
			b += b.UnitLastPlace();
			return new Interval(a, b);
		}

		/// <summary>
		/// Adds two intervals.
		/// </summary>
		/// <remarks>
		/// This operation is rounded. It will expand the endpoints of the result with <see cref="MathExtensions.UnitLastPlace(double)"/>.
		/// </remarks>
		/// <param name="a">First interval.</param>
		/// <param name="b">Second interval.</param>
		/// <returns>Sum of both intervals.</returns>
		public static Interval operator +(Interval a, Interval b)
		{
			if (a.IsNumber && b.IsNumber)
				return new Interval(a.a + b.a, true);
			double low = a.a + b.a;
			double upper = a.b + b.b;
			return CreateWithUlp(low, upper);
		}

		/// <summary>
		/// Subtracts two intervals.
		/// </summary>
		/// <remarks>
		/// This operation is rounded if not both operands are numbers. 
		/// It will expand the endpoints of the result with <see cref="MathExtensions.UnitLastPlace(double)"/>.
		/// </remarks>
		/// <param name="a">First interval.</param>
		/// <param name="b">Second interval.</param>
		/// <returns>Difference of both intervals.</returns>
		public static Interval operator -(Interval a, Interval b)
		{
			if (a.IsNumber && b.IsNumber)
				return new Interval(a.a - b.a, true);
			double low = a.a - b.b;
			double upp = a.b - b.a;
			return CreateWithUlp(low, upp);
		}

		/// <summary>
		/// Unary minus.
		/// </summary>
		/// <param name="a">The interval.</param>
		/// <returns>The result.</returns>
		public static Interval operator -(Interval a)
		{
			return new Interval(-a.b, -a.a);
		}

		/// <summary>
		/// Calculates the minimum of 4 numbers.
		/// </summary>
		/// <param name="a">First number.</param>
		/// <param name="b">Second number.</param>
		/// <param name="c">Third number.</param>
		/// <param name="d">Forth number.</param>
		/// <returns>The minimum of 4 numbers.</returns>
		static double Min4(double a, double b, double c, double d)
		{
			double m = a < b ? a : b;
			if (c < m)
				m = c;
			if (d < m)
				m = d;
			return m;
		}

		/// <summary>
		/// Calculates the maximum of 4 numbers.
		/// </summary>
		/// <param name="a">First number.</param>
		/// <param name="b">Second number.</param>
		/// <param name="c">Third number.</param>
		/// <param name="d">Forth number.</param>
		/// <returns>The maximum of 4 numbers.</returns>
		static double Max4(double a, double b, double c, double d)
		{
			double m = a > b ? a : b;
			if (c > m)
				m = c;
			if (d > m)
				m = d;
			return m;
		}

		/// <summary>
		/// Multiplies two intervals.
		/// </summary>
		/// <remarks>
		/// This operation is rounded if not both operands are numbers. 
		/// It will expand the endpoints of the result with <see cref="MathExtensions.UnitLastPlace(double)"/>.
		/// </remarks>
		/// <param name="a">First interval.</param>
		/// <param name="b">Second interval.</param>
		/// <returns>Product of both intervals.</returns>
		public static Interval operator *(Interval a, Interval b)
		{
			if (a.IsNumber && b.IsNumber)
				return new Interval(a.a * b.a, true);
			double ac = a.a * b.a, ad = a.a * b.b, bc = a.b * b.a, bd = a.b * b.b;
			double min = Min4(ac, ad, bc, bd);
			double max = Max4(ac, ad, bc, bd);
			return CreateWithUlp(min, max);
		}

		/// <summary>
		/// Divides two intervals.
		/// </summary>
		/// <remarks>
		/// This operation is rounded if not both operands are numbers. 
		/// It will expand the endpoints of the result with <see cref="MathExtensions.UnitLastPlace(double)"/>.
		/// <para>
		/// If <paramref name="b"/> contains 0, the result is [-∞,∞]; if <paramref name="a"/> and <paramref name="b"/>
		/// contain 0, the result is <see cref="Empty"/>.
		/// </para>
		/// </remarks>
		/// <param name="a">First interval.</param>
		/// <param name="b">Second interval.</param>
		/// <returns>Quotient of both intervals. </returns>
		public static Interval operator /(Interval a, Interval b)
		{
			if (a.IsEmpty || b.IsEmpty)
				return Empty;
			if (b.ContainsZero)
				if (a.ContainsZero)
					return Interval.Empty;	 // 0/0
				else
					return new Interval(double.NegativeInfinity, double.PositiveInfinity);
			if (a.IsNumber && b.IsNumber)
				return new Interval(a.a / b.a, true);

			double ac = a.a / b.a, ad = a.a / b.b, bc = a.b / b.a, bd = a.b / b.b;
			double min = Min4(ac, ad, bc, bd);
			double max = Max4(ac, ad, bc, bd);
			return CreateWithUlp(min, max);
		}

		#endregion
	}
}