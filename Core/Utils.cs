// CopyRight https://github.com/fnaxi. All Rights Reserved.

global using static Core.CUtils;

using System.Text.RegularExpressions;
using DSharpPlus.Entities;

namespace Core;

public static class CUtils
{
	/** Inverts IsNullOrWhiteSpace() for cleaner checks. */
	public static bool IsTextValid(string InText)
	{
		return !String.IsNullOrWhiteSpace(InText);
	}
	
	public static List<T> Concat<T>(List<T> InList1, List<T> InList2)
	{
		return InList1.Concat(InList2).ToList();
	}

	public static DiscordColor YellowGreen = new("#9ACD32");
	
	private static readonly Random RandomSeed = new();
	public static string RandomString(List<string> Options)
	{
		Int32 Index = RandomSeed.Next(Options.Count);
		return Options[Index];
	}
	
	public static string RemoveSpecialCharacters(string Text)
	{
		return Regex.Replace(Text, @"[^a-zA-Z0-9\s.,!?-]", string.Empty);
	}
}
