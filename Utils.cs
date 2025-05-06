using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace LiveSplit.RedFaction
{
	internal static class Utils
	{
		public static int Clamp(int value, int min, int max)
		{
			if (value > max)
				return max;
			else if (value < min)
				return min;
			else
				return value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteDebug(string text)
		{
#if DEBUG
			Debug.WriteLine(text);
#endif
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteDebug(object obj)
		{
#if DEBUG
			Debug.WriteLine(obj);
#endif
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteDebugIf(bool condition, string text)
		{
#if DEBUG
			Debug.WriteLineIf(condition, text);
#endif
		}
	}
}
