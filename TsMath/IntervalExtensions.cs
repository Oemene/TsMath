using System;

namespace TsMath
{
	/// <summary>
	/// Routines for higher level interval functions.
	/// </summary>
	public static class IntervalExtensions
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
			if (a.A < 0 && a.B > 0)
				return new Interval(0, Math.Max(-a.A, a.B));

			double lower = Math.Abs(a.A);
			double upper = Math.Abs(a.B);
			if (lower > upper)
			{
				double tmp = lower; lower = upper; upper = tmp;
			}
			return new Interval(lower, upper);
		}

	}
}