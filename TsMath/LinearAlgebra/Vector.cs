#region License
/*
Copyright (c) Thomas Steinfeld 2015. All rights reserved.
For detailed licensing information see LICENSE in the root folder.
*/
#endregion
using System;
using System.Text;

namespace TsMath.LinearAlgebra
{
	/// <summary>
	/// Vector containing intervals (<see cref="Interval"/>) as elements. 
	/// </summary>
	public class Vector
	{
		/// <summary>
		/// Array with elements.
		/// </summary>
		Interval[] elements;

		#region Properties
		/// <summary>
		/// Returns the dimensions of this vector.
		/// </summary>
		public int Size { get { return elements == null ? 0 : elements.Length; } }

		/// <summary>
		/// Returns the squared norm of this vector.
		/// </summary>
		public Interval Norm2 => this * this;

		/// <summary>
		/// Returns the norm (length) of this vector.
		/// </summary>
		public Interval Norm => Norm2.Sqrt();

		/// <summary>
		/// Returns an element at a given position.
		/// </summary>
		/// <param name="idx">Index of the element.</param>
		/// <returns>The element.</returns>
		public Interval this[int idx] => elements[idx];

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs a vector of intervals from an array.
		/// </summary>
		/// <param name="elements">The elements; this array is not copied, but merely assigned to the internal array.</param>
		public Vector(Interval[] elements)
		{
			this.elements = elements;
		}

		/// <summary>
		/// Creates a vector from a <see cref="double"/>-array.
		/// </summary>
		/// <param name="elements">Array containing the elements.</param>
		/// <param name="asNumber">Indicates if a the intervals will be a single number.</param>
		public Vector(double[] elements, bool asNumber = false)
		{
			this.elements = new Interval[elements.Length];
			for (int i = 0; i < elements.Length; i++)
			{
				this.elements[i] = new Interval(elements[i], asNumber);
			}
		}

		#endregion

		/// <summary>
		/// Gets the elements as <see cref="Interval"/>-array.
		/// </summary>
		/// <returns>Array of <see cref="Interval"/>.</returns>
		public Interval[] GetElements()
		{
			return elements;
		}

		#region Operations
		/// <summary>
		/// Adds two interval vectors.
		/// </summary>
		/// <param name="v1">First interval vector.</param>
		/// <param name="v2">Second interval vector.</param>
		/// <returns>Component-wise sum as interval vector.</returns>
		public static Vector operator +(Vector v1, Vector v2)
		{
			int n = v1.Size;
			if (n != v2.Size)
				throw new ArgumentException("Vector size mismatch");
			Interval[] result = new Interval[n];
			for (int i = 0; i < n; i++)
			{
				result[i] = v1.elements[i] + v2.elements[i];
			}
			return new Vector(result);
		}

		/// <summary>
		/// Subtracts two interval vectors.
		/// </summary>
		/// <param name="v1">First interval vector.</param>
		/// <param name="v2">Second interval vector.</param>
		/// <returns>Component-wise difference as interval vector.</returns>
		public static Vector operator -(Vector v1, Vector v2)
		{
			int n = v1.Size;
			if (n != v2.Size)
				throw new ArgumentException("Vector size mismatch");
			Interval[] result = new Interval[n];
			for (int i = 0; i < n; i++)
			{
				result[i] = v1.elements[i] - v2.elements[i];
			}
			return new Vector(result);
		}

		/// <summary>
		/// Multiplies a scalar with an interval vectors.
		/// </summary>
		/// <param name="scalar">Scalar value.</param>
		/// <param name="v">Interval vector.</param>
		/// <returns>Product as interval vector.</returns>
		public static Vector operator *(Interval scalar, Vector v)
		{
			int n = v.Size;
			Interval[] result = new Interval[n];
			for (int i = 0; i < n; i++)
			{
				result[i] = scalar * v.elements[i];
			}
			return new Vector(result);
		}

		/// <summary>
		/// Scalar product of two interval vectors.
		/// </summary>
		/// <param name="v1">First interval vector.</param>
		/// <param name="v2">Second interval vector.</param>
		/// <returns>Scalar product as interval vector.</returns>
		public static Interval operator *(Vector v1, Vector v2)
		{
			int n = v1.Size;
			if (n != v2.Size)
				throw new ArgumentException("Vector size mismatch");

			Interval sum = Interval.Zero;
			for (int i = 0; i < n; i++)
			{
				sum += v1.elements[i] * v2.elements[i];
			}
			return sum;
		}

		#endregion


		/// <summary>
		/// Returns a string representation of this interval vector.
		/// </summary>
		/// <returns>The string representation.</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append('(');
			for (int i = 0; i < elements.Length; i++)
			{
				if (i > 0)
					sb.Append(", ");
				sb.Append(elements[i].ToString());
			}
			sb.Append(')');
			return sb.ToString();
		}
	}

}
