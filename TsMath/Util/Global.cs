using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsMath.Util
{
	/// <summary>
	/// Bundles global settings.
	/// </summary>
	public class Global
	{

		/// <summary>
		/// The parallel threshold, describing the number of elements, where the system 
		/// switches to parallel (multi-threaded) versions of algorithms.
		/// </summary>
		public static int ParallelThreshold = 10000;

	}
}
