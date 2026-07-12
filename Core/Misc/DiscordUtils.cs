// CopyRight https://github.com/fnaxi. All Rights Reserved.

global using static Core.Misc.CDiscordUtils;
using System.Diagnostics;
using Core.ID;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Core.Misc;

public static class CDiscordUtils
{
	public static bool CanModerate(DiscordMember? Member)
	{
		Debug.Assert(Member != null);
		return IsAdmin(Member) || Member.Roles.Any(r => r.Id is CRole.Admin or CRole.Moderator);
	}
	
	public static bool IsAdmin(DiscordMember? Member)
	{
		Debug.Assert(Member != null);
		return Member.Permissions.HasFlag(Permissions.Administrator);
	}

	public static bool IsBotAlive(DiscordClient Client)
	{
		return !(Client.Ping > 3000); // TODO: Ambitious
	}
}
