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
	[Command("ban")]
	public async Task Ban(CommandContext Context, 
		[Description("The member to ban")] DiscordMember Member,
		[Description("The reason of the ban")] string Reason = "No reason provided")
	{
		if (!CanModerate(Context))
		{
			await NoRights(Context);
			return;
		}

		Debug.Assert(Context.Member != null);
		if (Member.Id == Context.Client.CurrentUser.Id || Member.Id == Context.Member.Id)
		{
			await Context.RespondAsync(RandomString(CQuote.BotOrSelfBan));
			return;
		}
		
		// TODO: Actual ban
		// await Context.Guild.BanMemberAsync(Member);
		
		DiscordEmbedBuilder Embed = new DiscordEmbedBuilder()
		{
			Title = $"{Member.DisplayName}! {RandomString(CQuote.Ban)}",
			Description = 
				$"Member {Member.Username}/{Member.Id} was banned! {CEmoji.GirLaugh}" +
				$"\n\n Reason: {Reason}",
			Color = YellowGreen
		};
		await Context.Channel.SendMessageAsync(Embed);
	}
}
