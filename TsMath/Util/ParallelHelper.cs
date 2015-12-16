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
