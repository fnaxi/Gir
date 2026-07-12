// CopyRight https://github.com/fnaxi. All Rights Reserved.

using System.Diagnostics;
using Core.ID;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Gir.ID;
using Gir.Misc;
using Gir.Services.Client;

namespace Gir.Commands;

public class CMiscCommands : BaseCommandModule
{
	[Command("help")]
	[Description("Shows a list of available commands")]
	public async Task Help(CommandContext Context)
	{
		Debug.Assert(Context.Member != null);
		
		DiscordEmbedBuilder Embed = CHelpMenuService.MakeMainMenuEmbed(Context.Member);
		List<DiscordSelectComponentOption> Options = new List<DiscordSelectComponentOption>
		{
			new("Choose a Category...", "SID_Choose", 
				"", true, 
				emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(Context.Client, ":pushpin:"))),
			
			// TODO: new("Main Menu", "main", "Go back to the main menu.", emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(Context.Client, ":house:"))),
			
			new("Moderation", "SID_Mod", 
				"Tools to enforce server rules and keep the community peaceful.", 
				emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(Context.Client, ":shield:"))),
			
			new("Misc & Utilities", "SID_Misc", 
				"Misc commands and utilities.", 
				emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(Context.Client, ":gear:"))),
			
			new("Entertain", "SID_Entertain", 
				"Entertain commands.", 
				emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(Context.Client, ":tada:"))),
			
			new("Tickets", "SID_Ticket", 
				"Commands for managing, opening, and closing support tickets.", 
				emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(Context.Client, ":ticket:"))),
			
			new("Management", "SID_Manage", 
				"Administrator-only management commands.",
				emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(Context.Client, ":tools:"))),
			
			new("Test", "SID_Test", 
				"Administrator-only commands that are helpful for testing the bot.", 
				emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(Context.Client, ":warning:")))
		};
		DiscordSelectComponent Dropdown = new DiscordSelectComponent($"SID_HelpMenu_{Context.Member.Id}", "Select a category...", Options);

		DiscordMessageBuilder Message = new DiscordMessageBuilder();
		Message.AddEmbed(Embed);
		Message.AddComponents(Dropdown);
		
		await Context.RespondAsync(Message);
	}
	
	[Command("info")]
	[Description("Shows info about specified user")]
	public async Task UserInfo(CommandContext Context,
		[Description("The user to look up. Defaults to yourself if left blank")] DiscordMember? Member = null)
	{
		if (SentInBotChannel(Context.Channel)) return;
		
		Member ??= Context.Member;
		Debug.Assert(Member != null);
		
		List<string> Roles = Member.Roles
			.Where(r => r.Id != Context.Guild.Id)
			.Select(r => r.Mention).ToList();
		
		string RolesText = Roles.Count != 0 ? string.Join(", ", Roles) : "None";
		DiscordEmbedBuilder Embed = new DiscordEmbedBuilder()
		{
			Title = $"User Info - {Member.DisplayName}",
			Color = Member.Color,
		};
		Embed.WithThumbnail(Member.AvatarUrl);

		Embed.AddField($"{CEmoji.GirDress} ID", $"{Member.Id}", true);
		Embed.AddField(":date: Account Created", $"{Member.CreationTimestamp:D}", true);
		Embed.AddField(":inbox_tray: Joined Server", $"{Member.JoinedAt:D}", true);
		Embed.AddField($":label: Roles [{Roles.Count}]", RolesText, true);
		Embed.AddField(":mute: Is muted?", Member.IsMuted.ToString(), true);
		
		Debug.Assert(Context.Member != null);
		Embed.WithFooter($"Requested by {Context.Member.DisplayName}", Context.Member.AvatarUrl);
		Embed.WithTimestamp(DateTime.UtcNow);
		
		await Context.RespondAsync(Embed);
	}
}
