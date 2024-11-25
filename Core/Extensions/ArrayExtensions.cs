using System;

namespace TheMage.Core.Extensions;

public static class ArrayExtensions
{
	public static T[] Fill<T>(this T[] array, T value)
	{
		Array.Fill(array, value);
		return array;
	}
}