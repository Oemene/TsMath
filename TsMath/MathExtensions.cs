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
	/// Math helper functions and extension.
	/// </summary>
	public static class MathExtensions
	{

		/// <summary>
		/// Index of the most significant word of a double number.
		/// </summary>
		static int MSW;

		static MathExtensions()
		{
			MSW = BitConverter.IsLittleEndian ? 3 : 0;
		}

		/// <summary>
		/// A numerical stable version to calculate the length of the hypotenuse of a right-angled triangle. 
		/// </summary>
		/// <param name="a">First cathetus (leg).</param>
		/// <param name="b">Second cathetus (leg).</param>
		/// <returns>Length of the third side (hypotenuse).</returns>
		public static double Pythag(double a, double b)
		{
			if (Math.Abs(a) > Math.Abs(b))
			{
				double r = b / a;
				return Math.Abs(a) * Math.Sqrt(1 + r * r);
			}
			if (b != 0)
			{
				double r = a / b;
				return Math.Abs(b) * Math.Sqrt(1 + r * r);
			}
			return 0.0;
		}

		/// <summary>
		/// Calculates the square of a number.
		/// </summary>
		/// <param name="x">The number.</param>
		/// <returns>The square (<paramref name="x"/>*<paramref name="x"/>).</returns>
		public static double Sqr(this double x)
		{
			return x * x;
		}

		/// <summary>
		/// Return the logarithm of a specified number to the base 2.
		/// </summary>
		/// <param name="x">The number whose logarithm is to be found.</param>
		/// <returns>The logarithm.</returns>
		public static double Log2(this double x)
		{
			return Math.Log(x, 2);
		}

		/// <summary>
		/// Checks if a number is NaN or infinite.
		/// </summary>
		/// <param name="x">The number to check.</param>
		/// <returns>True, if <paramref name="x"/> is NaN or infinite.</returns>
		public static bool IsFinite(this double x)
		{
			return !double.IsNaN(x) && !double.IsInfinity(x);
		}

	
		/// <summary>
		/// Calculates the unit in the last place of a double number.
		/// </summary>
		/// <remarks>
		/// This code is adapted from Abrams: Efficient and Reliable Methods for Rounded Interval Arithmetic. 
		/// It depends on, that the number is internally represented according to the IEEE-754 standard. 
		/// </remarks>
		/// <param name="x">The number.</param>
		/// <returns>The unit in the last place.</returns>
		public static double UnitLastPlace(this double x)
		{
			if (x == 0)
				return 0;
			long xl = BitConverter.DoubleToInt64Bits(x);
			ushort msWord = (ushort)((xl >> (MSW << 4)) & 0xffff);
			ushort msUWord = 0;

			msWord &= 0x7ff0;         //isolate exponent in 16-bit word

			int bitPos = -1;
			if (msWord > 0x340)
				//set exponent to e-52;  the value 0x0340 is 52 left-shifted 4 bits, i.e. 0x0340 = 832 = 52<<4
				msUWord = (ushort)(msWord - 0x0340);
			else
			{
				//biased exponent - 1
				int e1 = (msWord >> 4) - 1;
				//find 16-bit word containing bit e-1
				int word = e1 >> 4;
				//compensate for word ordering
				if (MSW == 0)
					word = 3 - word;
				//find the bit position in this word
				int bit = e1 % 16;
				int mask = 1 << bit;
				//set the bit to 1
				bitPos =  bit + (word << 4);
			}
			long ul = (long)msUWord << (MSW << 4);
			if (bitPos >= 0)
				ul |= (1L << bitPos);
			return BitConverter.Int64BitsToDouble(ul);
		}
	}
}
