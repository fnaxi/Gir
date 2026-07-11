// CopyRight https://github.com/fnaxi. All Rights Reserved.

global using static Gir.Misc.CDiscordUtils;

using System.Diagnostics;
using DSharpPlus;
using DSharpPlus.Entities;
using Gir.ID;

namespace Gir.Misc;

public static class CDiscordUtils
{
	public static bool CanModerate(DiscordMember? Member)
	{
		Debug.Assert(Member != null);
		return IsAdmin(Member) || Member.Roles.Any(r => r.Id is CRole.Admin or CRole.Moderator);
	}
	
	// TODO: use everywhere
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
