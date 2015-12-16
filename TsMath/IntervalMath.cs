#region License
/*
Copyright (c) Thomas Steinfeld 2015. All rights reserved.
For detailed licensing information see LICENSE in the root folder.
*/
#endregion
using System;

namespace TsMath
{
	/// <summary>
	/// Routines for higher level interval functions.
	/// </summary>
	public static class IntervalMath
	{
		/// <summary>
		/// Computes the square root of an interval.
		/// </summary>
		/// <param name="a">The interval to compute the square root.</param>
		/// <returns>The square root; <see cref="Interval.Empty"/> if <paramref name="a"/> contains a negative number.</returns>
		public static Interval Sqrt(this Interval a)
		{
			if (a.IsEmpty || a.A < 0)
				return Interval.Empty;
			double lower = Math.Sqrt(a.A);
			if (a.IsNumber)
				return new Interval(lower, true);
			double upper = Math.Sqrt(a.B);
			return Interval.CreateWithUlp(lower, upper);
		}

		/// <summary>
		/// Calculates the power of <b>e</b> raised to <paramref name="a"/>.
		/// </summary>
		/// <param name="a">The exponent.</param>
		/// <returns><b>e</b>^<paramref name="a"/></returns>
		public static Interval Exp(this Interval a)
		{
			if (a.IsEmpty)
				return Interval.Empty;
			double lower = Math.Exp(a.A);
			if (a.IsNumber)
				return new Interval(lower, true);
			double upper = Math.Exp(a.B);
			return Interval.CreateWithUlp(lower, upper);
		}

		/// <summary>
		/// Compute the absolute value of an interval.
		/// </summary>
		/// <param name="a">The interval.</param>
		/// <returns>The absolute value.</returns>
		public static Interval Abs(this Interval a)
		{
			if (a.IsEmpty)
				return Interval.Empty;
			if (a.IsNumber)
				return new Interval(Math.Abs(a.A), true);
			double lower = Math.Abs(a.A);
			double upper = Math.Abs(a.B);
			if (a.A < 0 && a.B > 0)
				return new Interval(0, Math.Max(lower, upper));

			if (lower > upper)
			{
				double tmp = lower; lower = upper; upper = tmp;
			}
			return new Interval(lower, upper);
		}

	}
}