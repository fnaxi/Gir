// CopyRight https://github.com/fnaxi. All Rights Reserved.

using System.Diagnostics;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using InvaderZim.ID;
using InvaderZim.Misc;

namespace InvaderZim.Commands;

public class CModerationCommands : BaseCommandModule
{
	[Command("mute")]
	[Description("Mutes the specified member")]
	public async Task Mute(CommandContext Context, 
		[Description("The member to mute")] DiscordMember Member,
		[Description("Time of the mute in minutes")] UInt32 Duration = 30,
		[Description("The reason of the mute")] string Reason = "No reason provided")
	{
		if (!CanModerate(Context))
		{
			await NoRights(Context);
			return;
		}

		if (IsTargetingBotOrSelf(Context, Member).Result) return;
		
		// TODO: replace with format 1d6h2m5s
		DateTimeOffset TimeoutTime = DateTimeOffset.UtcNow.AddMinutes(Duration);
		await Member.TimeoutAsync(TimeoutTime, Reason);
		
		Debug.Assert(Context.Member != null);
		DiscordEmbedBuilder Embed = new DiscordEmbedBuilder()
		{
			Title = $"{RandomString(CQuote.Ban)}",
			Description = 
				$"Member {Member.DisplayName} was muted for {Duration} minute{(Duration == 1 ? "" : "s")} by {Context.Member.DisplayName} {CEmoji.GirBlep}" +
				$"\n\n Reason: {Reason}",
			Color = YellowGreen
		};
		await Context.RespondAsync(Embed);
	}
	
	[Command("unmute")]
	[Description("Unmutes the specified member")]
	public async Task Unmute(CommandContext Context, 
		[Description("The member to unmute")] DiscordMember Member)
	{
		if (!CanModerate(Context))
		{
			await NoRights(Context);
			return;
		}
		
		if (IsTargetingBotOrSelf(Context, Member).Result) return;

		if (!Member.CommunicationDisabledUntil.HasValue || Member.CommunicationDisabledUntil.Value <= DateTimeOffset.UtcNow)
		{
			await Context.RespondAsync($"{Member.DisplayName} is not currently timed out {CEmoji.ZimAngry}");
			return;
		}
		
		await Member.TimeoutAsync(null); // TODO: reason
		
		Debug.Assert(Context.Member != null);
		DiscordEmbedBuilder Embed = new DiscordEmbedBuilder()
		{
			Title = $"{RandomString(CQuote.Ban)} {CEmoji.GirBlep}",
			Description = $"Member {Member.DisplayName} was unmuted by {Context.Member.DisplayName}",
			Color = YellowGreen
		};
		await Context.RespondAsync(Embed);
	}
	
	// TODO: purge(message amount), prune(time) / a channel argument is optional
	
	// TODO: reap(member, reason)
	
	[Command("kick")]
	[Description("Kicks the specified member")]
	public async Task Kick(CommandContext Context, 
		[Description("The member to kick")] DiscordMember Member,
		[Description("The reason of the kick")] string Reason = "No reason provided")
	{
		if (!CanModerate(Context))
		{
			await NoRights(Context);
			return;
		}

		if (IsTargetingBotOrSelf(Context, Member).Result) return;
		
		// TODO: Actual kick
		// await Member.RemoveAsync(Reason);
		
		Debug.Assert(Context.Member != null);
		DiscordEmbedBuilder Embed = new DiscordEmbedBuilder()
		{
			Title = $"{RandomString(CQuote.Ban)} {CEmoji.GirBlep}",
			Description = 
				$"Member {Member.DisplayName} was kicked by {Context.Member.DisplayName}" +
				$"\n\n Reason: {Reason}",
			Color = YellowGreen
		};
		await Context.RespondAsync(Embed);
	}
	
	[Command("ban")]
	[Description("Bans the specified member")]
	public async Task Ban(CommandContext Context, 
		[Description("The member to ban")] DiscordMember Member,
		[Description("The reason of the ban")] string Reason = "No reason provided")
	{
		if (!CanModerate(Context))
		{
			await NoRights(Context);
			return;
		}

		if (IsTargetingBotOrSelf(Context, Member).Result) return;
		
		// TODO: Actual ban
		// await Context.Guild.BanMemberAsync(Member);
		
		Debug.Assert(Context.Member != null);
		DiscordEmbedBuilder Embed = new DiscordEmbedBuilder()
		{
			Title = $"{RandomString(CQuote.Ban)} {CEmoji.GirBlep}",
			Description = 
				$"Member {Member.DisplayName} was banned by {Context.Member.DisplayName}" +
				$"\n\n Reason: {Reason}",
			Color = YellowGreen
		};
		await Context.RespondAsync(Embed);
	}
	
	[Command("unban")]
	[Description("Removes a ban from specified member")]
	public async Task UnBan(CommandContext Context, 
		[Description("The member to unban")] DiscordMember Member)
	{
		if (!CanModerate(Context))
		{
			await NoRights(Context);
			return;
		}

		if (IsTargetingBotOrSelf(Context, Member).Result) return;
		
		// TODO: unban command
	}
}
