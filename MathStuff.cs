namespace LiveSplit.RedFaction
{
	internal static class MathStuff
	{
		internal static int Clamp(this int value, int min, int max)
		{
			if (value > max)
				return max;
			else if (value < min)
				return min;
			else
				return value;
		}
	}
}
