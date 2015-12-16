#region License
/*
Copyright (c) Thomas Steinfeld 2015. All rights reserved.
For detailed licensing information see LICENSE in the root folder.
*/
#endregion
using System;
using System.Threading.Tasks;

namespace TsMath.Util
{

	/// <summary>
	/// Internal helper class to switch multi threading behavior.
	/// </summary>
	public class ParallelHelper
	{

		/// <summary>
		/// Main switch, to enable parallel execution. If set to <b>false</b>, nothing will be executed 
		/// with multi threading. (This switch is for debugging purposes mostly and should not be used
		/// in normal code)
		/// </summary>
		public static bool EnableParallel = true;

		/// <summary>
		/// For loop with switches to multi-threaded variant depending of <paramref name="complexity"/>.
		/// </summary>
		/// <param name="fromInclusive">The start index, inclusive.</param>
		/// <param name="toExclusive">The end index, exclusive.</param>
		/// <param name="action">The action to be performed for every index.</param>
		/// <param name="complexity">The complexity. If this values is at least <see cref="Global.ParallelThreshold"/>
		/// than the for loop will be executed in parallel using <see cref="Parallel.For(int, int, Action{int})"/>.</param>
		public static void For(int fromInclusive, int toExclusive, Action<int> action, int complexity)
		{
			if (complexity >= Global.ParallelThreshold)
				Parallel.For(fromInclusive, toExclusive, action);
			else
			{
				for (int i = fromInclusive; i < toExclusive; i++)
					action(i);
			}
		}

	}
}
