#region License
/*
Copyright (c) Thomas Steinfeld 2015. All rights reserved.
For detailed licensing information see LICENSE in the root folder.
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TsMath.Util;

namespace TsMath.LinearAlgebra
{
	/// <summary>
	/// Dense matrix (array m x n) with HybridDouble values for matrix algebra.
	/// </summary>
	public class Matrix
	{

		Interval[,] elements;

		int nRows, nCols;


		#region Constructors
		/// <summary>
		/// Constructs a empty (zero) matrix.
		/// </summary>
		/// <param name="nRows">Number of rows.</param>
		/// <param name="nColumns">Number of columns.</param>
		public Matrix(int nRows, int nColumns)
		{
			this.nRows = nRows;
			this.nCols = nColumns;
			elements = new Interval[nRows, nColumns];
		}

		/// <summary>
		/// Constructs a matrix from an array. The elements are not copied. The dimensions of the array <paramref name="data"/>
		/// determine the dimensions of this matrix.
		/// </summary>
		/// <param name="data">Array of elements (reused).</param>
		public Matrix(Interval[,] data)
		{
			this.elements = data;
			nRows = data.GetLength(0);
			nCols = data.GetLength(1);
		}

		/// <summary>
		/// Creates a matrix from a <see cref="double"/>-array.
		/// </summary>
		/// <param name="data">The array.</param>
		/// <param name="asNumber">Indicates if a the intervals will be a single number.</param>
		public Matrix(double[,] data, bool asNumber = false)
		{
			nRows = data.GetLength(0);
			nCols = data.GetLength(1);
			elements = new Interval[nRows, nCols];
			for (int i = 0; i < nRows; i++)
			{
				for (int j = 0; j < nCols; j++)
				{
					elements[i, j] = new Interval(data[i, j], asNumber);
				}
			}
		}

		/// <summary>
		/// Create a row-matrix (1 x n) or a column matrix (n x 1) from a HybridDouble array.
		/// </summary>
		/// <param name="vals">Values.</param>
		/// <param name="fColumn">Indicates if the matrix is a column matrix (<b>true</b>) or a row matrix otherwise.</param>
		public Matrix(Interval[] vals, bool fColumn = true)
		{
			if (fColumn)
			{
				nRows = vals.Length; nCols = 1;
				elements = new Interval[nRows, nCols];
				for (int i = 0; i < vals.Length; i++)
				{
					elements[i, 0] = vals[i];
				}
			}
			else
			{
				nCols = vals.Length; nRows = 1;
				elements = new Interval[nRows, nCols];
				for (int i = 0; i < vals.Length; i++)
				{
					elements[0, i] = vals[i];
				}
			}
		}
		#endregion

		#region Member access
		/// <summary>
		/// Gets or sets the elements of the matrix.
		/// </summary>
		/// <param name="row">The row number to get or set (zero indexed).</param>
		/// <param name="column">The column number to get or set (zero indexed).</param>
		/// <returns>The value at the indexed element.</returns>
		public Interval this[int row, int column]
		{
			get
			{
				return elements[row, column];
			}
			set
			{
				elements[row, column] = value;
			}
		}

		/// <summary>
		/// Gets the number of rows of the matrix.
		/// </summary>
		public int RowCount
		{
			get { return nRows; }
		}

		/// <summary>
		/// Gets the number of columns of the matrix.
		/// </summary>
		public int ColumnCount
		{
			get { return nCols; }
		}

		/// <summary>
		/// Copies the contents of a row to an array.
		/// </summary>
		/// <param name="row">Index of the row.</param>
		/// <param name="elements">Array to receive the elements.</param>
		public void FillRow(int row, Interval[] elements)
		{
			for (int i = 0; i < nCols; i++)
			{
				elements[i] = this.elements[row, i];
			}
		}

		/// <summary>
		/// Copies the contents of a column to an array.
		/// </summary>
		/// <param name="column">Index of the column.</param>
		/// <param name="elements">Array to receive the elements.</param>
		public void FillColumn(int column, Interval[] elements)
		{
			for (int i = 0; i < nRows; i++)
			{
				elements[i] = this.elements[i, column];
			}
		}

		/// <summary>
		/// Retrieves a column of the matrix as vector.
		/// </summary>
		/// <param name="column">The index of the column.</param>
		/// <returns>Vector which contains the contents of the column.</returns>
		public Vector GetColumn(int column)
		{
			Interval[] hd = new Interval[nRows];
			FillColumn(column, hd);
			return new Vector(hd);
		}

		/// <summary>
		/// Retrieves a row of the matrix as vector.
		/// </summary>
		/// <param name="row">The index of the row.</param>
		/// <returns>Vector which contains the contents of the row.</returns>
		public Vector GetRow(int row)
		{
			Interval[] hd = new Interval[nCols];
			FillRow(row, hd);
			return new Vector(hd);
		}

		/// <summary>
		/// Sets a row in this matrix.
		/// </summary>
		/// <param name="row">The row to set.</param>
		/// <param name="elements">Array where the elements come from.</param>
		public void SetRow(int row, Interval[] elements)
		{
			for (int i = 0; i < nCols; i++)
			{
				this.elements[row, i] = elements[i];
			}
		}

		/// <summary>
		/// Sets a column in this matrix.
		/// </summary>
		/// <param name="column">The column to set.</param>
		/// <param name="elements">Array where the elements come from.</param>
		public void SetColumn(int column, Interval[] elements)
		{
			for (int i = 0; i < nRows; i++)
			{
				this.elements[i, column] = elements[i];
			}
		}

		/// <summary>
		/// Swaps the contents of two rows of the matrix.
		/// </summary>
		/// <param name="row1">First row index.</param>
		/// <param name="row2">Second row index.</param>
		public void SwapRows(int row1, int row2)
		{
			for (int i = 0; i < nCols; i++)
			{
				var tmp = elements[row1, i];
				elements[row1, i] = elements[row2, i];
				elements[row2, i] = tmp;
			}
		}

		/// <summary>
		/// Swaps the contents of two columns of the matrix.
		/// </summary>
		/// <param name="col1">First column index.</param>
		/// <param name="col2">Second column index.</param>
		public void SwapColums(int col1, int col2)
		{
			for (int i = 0; i < nRows; i++)
			{
				var tmp = elements[i, col1];
				elements[i, col1] = elements[i, col2];
				elements[i, col2] = tmp;
			}
		}

		#endregion

		#region Creation and copy helpers

		private void SetElementwise(Func<int, int, Interval> op, int complexity)
		{
			ParallelHelper.For(0, nRows, i =>
			{
				for (int j = 0; j < nCols; j++)
				{
					this.elements[i, j] = op(i, j);
				}
			}, complexity);
		}


		/// <summary>
		/// Creates a copy of this matrix.
		/// </summary>
		/// <param name="fParallel">Indicates whether the copy should be carried out in parallel.</param>
		/// <returns>The copy.</returns>
		public Matrix Copy(bool fParallel = false)
		{
			var m = new Matrix(this.RowCount, this.ColumnCount);
			m.SetElementwise((row, column) => this[row, column], nRows * nCols);
			return m;
		}

		/// <summary>
		/// Creates a diagonal matrix.
		/// </summary>
		/// <param name="n">Dimensions.</param>
		/// <param name="d">Value for the diagonal elements.</param>
		/// <returns>Diagonal matrix.</returns>
		public static Matrix CreateDiagonal(int n, Interval d)
		{
			var m = new Matrix(n, n);
			for (int i = 0; i < n; i++)
			{
				m.elements[i, i] = d;
			}
			return m;
		}

		/// <summary>
		/// Creates a identity matrix.
		/// </summary>
		/// <param name="n">Dimension of the matrix.</param>
		/// <param name="asNumber">Indicates if a the intervals will be a single number.</param>
		/// <returns>The identity matrix.</returns>
		public static Matrix CreateIdentity(int n, bool asNumber)
		{
			var hd = new Interval(1, asNumber);
			return CreateDiagonal(n, hd);
		}

		/// <summary>
		/// Creates a column or row matrix form a vector.
		/// </summary>
		/// <param name="v">The vector to use.</param>
		/// <param name="createColumnMatrix">Indicates if the resulting matrix is a column (nx1) or row (1xn) matrix.</param>
		/// <returns>The matrix.</returns>
		public static Matrix FromVector(Vector v, bool createColumnMatrix = true)
		{
			var m = new Matrix(createColumnMatrix ? v.Size : 1, createColumnMatrix ? 1 : v.Size);
			if (createColumnMatrix)
				m.SetColumn(0, v.GetElements());
			else
				m.SetRow(0, v.GetElements());
			return m;
		}

		#endregion

		#region Operations
		/// <summary>
		/// Transposes the matrix.
		/// </summary>
		/// <param name="fParallel">Indicates whether the transposition should be carried out in parallel.</param>
		/// <returns>The transposed matrix.</returns>
		public Matrix Transpose(bool fParallel = false)
		{
			Matrix c = new Matrix(nCols, nRows);
			c.SetElementwise((row, column) => this[column, row], nRows * nCols);
			return c;
		}


		/// <summary>
		/// Multiplies (element wise) a scalar with a matrix.
		/// </summary>
		/// <param name="scalar">The scalar.</param>
		/// <param name="a">The matrix.</param>
		/// <returns>Product of element wise multiplication.</returns>
		public static Matrix operator *(Interval scalar, Matrix a)
		{
			Matrix c = new Matrix(a.RowCount, a.ColumnCount);
			c.SetElementwise((row, column) => scalar * a[row, column], a.nRows * a.nCols);
			return c;
		}

		/// <summary>
		/// Multiplies this matrix with an other.
		/// </summary>
		/// <param name="b"></param>
		/// <returns>The result of the multiplication.</returns>
		public Matrix Multiply(Matrix b)
		{
			Matrix c = new Matrix(this.RowCount, b.ColumnCount);
			int ek = this.ColumnCount;
			if (ek != b.RowCount)
				throw new ArgumentException("Matrix size mismatch");
			c.SetElementwise((row, column) =>
			{
				Interval sum = elements[row, 0] * b[0, column];
				for (int k = 1; k < ek; k++)
					sum += elements[row, k] * b[k, column];
				return sum;
			}, this.RowCount * b.ColumnCount * ek);
			return c;
		}


		/// <summary>
		/// Multiplies two matrices non parallel.
		/// </summary>
		/// <param name="a">First matrix.</param>
		/// <param name="b">Second matrix.</param>
		/// <returns>The result matrix.</returns>
		public static Matrix operator *(Matrix a, Matrix b)
		{
			return a.Multiply(b);
		}

		/// <summary>
		/// Multiplies a matrix with a column vector.
		/// </summary>
		/// <param name="a">The matrix.</param>
		/// <param name="b">The array describing the column vector.</param>
		/// <returns>The resulting vector.</returns>
		public static Interval[] Multiply(Matrix a, Interval[] b)
		{
			Interval[] c = new Interval[a.RowCount];
			int ej = a.ColumnCount;
			ParallelHelper.For(0, a.RowCount, i =>
			{
				Interval sum = Interval.Zero;
				for (int j = 0; j < ej; j++)
					sum += a[i, j] * b[j];
				c[i] = sum;
			}, a.RowCount * ej);
			return c;
		}

		/// <summary>
		/// Multiplies a matrix with a column vector.
		/// </summary>
		/// <param name="a">The matrix.</param>
		/// <param name="b">The array describing the column vector.</param>
		/// <returns>The result vector.</returns>
		public static Interval[] operator *(Matrix a, Interval[] b)
		{
			return Multiply(a, b);
		}

		/// <summary>
		/// Multiplies a matrix with a (column) vector.
		/// </summary>
		/// <param name="a">The matrix.</param>
		/// <param name="b">The column vector.</param>
		/// <returns>The result vector.</returns>
		public static Vector operator *(Matrix a, Vector b)
		{
			var c = a * (b.GetElements());
			return new Vector(c);
		}

		/// <summary>
		/// Adds two matrices.
		/// </summary>
		/// <param name="a">First matrix.</param>
		/// <param name="b">Second matrix.</param>
		/// <returns>The result matrix.</returns>
		public static Matrix operator +(Matrix a, Matrix b)
		{
			int aRows = a.nRows, aCols = a.nCols;
			if (b.nRows != aRows || b.nCols != aCols)
				throw new ArgumentException("Matrix size mismatch.");
			Matrix c = new Matrix(aRows, aCols);
			c.SetElementwise((row, column) => a[row, column] + b[row, column], aRows * aCols);
			return c;
		}

		/// <summary>
		/// Subtracts two matrices.
		/// </summary>
		/// <param name="a">First matrix.</param>
		/// <param name="b">Second matrix.</param>
		/// <returns>The result matrix.</returns>
		public static Matrix operator -(Matrix a, Matrix b)
		{
			int aRows = a.nRows, aCols = a.nCols;
			if (b.nRows != aRows || b.nCols != aCols)
				throw new ArgumentException("Matrix size mismatch.");
			Matrix c = new Matrix(aRows, aCols);
			c.SetElementwise((row, column) => a[row, column] - b[row, column], aRows * aCols);
			return c;
		}

		/// <summary>
		/// Retrieves the negated matrix.
		/// </summary>
		/// <param name="a">Input matrix.</param>
		/// <returns>The result matrix.</returns>
		public static Matrix operator -(Matrix a)
		{
			Matrix c = new Matrix(a.RowCount, a.ColumnCount);
			c.SetElementwise((row, column) => -a[row, column], a.nRows * a.nCols);
			return c;
		}

		/// <summary>
		/// Gets the element array for this matrix.
		/// </summary>
		/// <returns>The underlying data array.</returns>
		public Interval[,] GetElements() { return elements; }

		#endregion

		#region Object overrides

		/// <summary>
		/// Retrieves a string representation of this matrix.
		/// </summary>
		/// <returns>The string representation.</returns>
		public override string ToString()
		{
			int nr = Math.Min(nRows, 5);
			int ncol = Math.Min(nCols, 5);
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < nr; i++)
			{
				if (i > 0)
					sb.Append("\r\n");
				bool fRand = i == 0 || i == nr - 1;
				sb.Append(fRand ? '(' : '|');
				for (int j = 0; j < ncol; j++)
				{
					if (j > 0)
						sb.Append(", ");
					sb.AppendFormat(elements[i, j].ToString());
				}
				if (ncol < nCols)
					sb.Append(",...");
				sb.Append(fRand ? ')' : '|');
			}
			if (nr < nRows)
			{
				sb.Append("\r\n   ......");
			}
			return sb.ToString();
		}

		#endregion


		/// <summary>
		/// Calculates the square of the Frobenius norm of the matrix.
		/// </summary>
		public Interval FrobeniusNorm2
		{
			get
			{
				Interval sum = Interval.Zero;
				SpinLock lck = new SpinLock();
				ParallelHelper.For(0, nRows, i =>
					{
						Interval sumj = Interval.Zero;

						for (int j = 0; j < nCols; j++)
						{
							var el = elements[i, j];
							sumj += el * el;
						}
						bool lockTaken = false;
						try
						{
							lck.Enter(ref lockTaken);
							sum += sumj;
						}
						finally
						{
							if (lockTaken)
								lck.Exit();
						}

					}, nRows * nCols);
				return sum;
			}
		}

		/// <summary>
		/// Calculates the Frobenius norm of the matrix.
		/// </summary>
		public Interval FrobeniusNorm
		{
			get
			{
				return FrobeniusNorm2.Sqrt();
			}
		}


	}

}
