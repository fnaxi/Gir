// CopyRight https://github.com/fnaxi. All Rights Reserved.

using System.Diagnostics;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using InvaderZim.ID;
using InvaderZim.Misc;

namespace InvaderZim.Commands;

public class CMiscCommands : BaseCommandModule
{
	[Command("help")]
	[Description("Shows a list of available commands")]
	public async Task Help(CommandContext Context,
		[Description("The user to look up. Defaults to yourself if left blank")] DiscordMember? Member = null)
	{
		DiscordEmbedBuilder Embed = new DiscordEmbedBuilder()
		{
			Title = $"Help Menu {CEmoji.BmoDance}",
			Description = "Some sick help text",
			Color = YellowGreen
		};
		// TODO: Help command
		
		Debug.Assert(Context.Member != null);
		Embed.WithFooter($"Requested by {Context.Member.DisplayName}", Context.Member.AvatarUrl);
		Embed.WithTimestamp(DateTime.UtcNow);
		
		await Context.RespondAsync(Embed);
	}
	
	[Command("info")]
	[Description("Shows info about specified user")]
	public async Task UserInfo(CommandContext Context,
		[Description("The user to look up. Defaults to yourself if left blank")] DiscordMember? Member = null)
	{
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

	[Command("ping")]
	[Description("Checks the bot's latency and response time")]
	public async Task Ping(CommandContext Context)
	{
		if (SentInBotChannel(Context)) return;
		
		DiscordEmbedBuilder CalculatingEmbed = new DiscordEmbedBuilder()
		{
			Title = "Ping status",
			Description = $"Calculating latency... {CEmoji.GirDance}",
			Color = YellowGreen
		};
		
		DateTime StartTime = DateTime.UtcNow;
		DiscordMessage Message = await Context.RespondAsync(CalculatingEmbed);
		DateTime EndTime = DateTime.UtcNow;
		
		Int32 WebsocketPing = Context.Client.Ping;
		Int32 ResponsePig = (Int32)(EndTime - StartTime).TotalMilliseconds;
		
		DiscordEmbedBuilder FinalEmbed = new DiscordEmbedBuilder()
		{
			Title = "Ping Status",
			Color = WebsocketPing < 500 ? (WebsocketPing < 150 ? DiscordColor.Green : DiscordColor.Orange) : DiscordColor.Red
		};
		
		FinalEmbed.AddField($"{CEmoji.BmoDance} Bot Latency", $"`{WebsocketPing}ms`", true);
		FinalEmbed.AddField($"{CEmoji.Alien} Response Latency", $"`{ResponsePig}ms`", true);

		Debug.Assert(Context.Member != null);
		FinalEmbed.WithFooter($"Requested by {Context.Member.DisplayName}", Context.Member.AvatarUrl);
		FinalEmbed.WithTimestamp(DateTime.UtcNow);
		
		await Message.ModifyAsync(FinalEmbed.Build());
	}

	[Command("shutdown")]
	[Description("Shuts down the bot if it's running currently")]
	[RequireUserPermissions(Permissions.Administrator)]
	public async Task Shutdown(CommandContext Context)
	{
		if (SentInBotChannel(Context)) return;

		DiscordMessage Response = await Context.RespondAsync("Shutting down...");

		await Context.Client.DisconnectAsync();
		Environment.Exit(0);
		
		await Task.Delay(TimeSpan.FromSeconds(TemporaryResponseTime));
		await Response.DeleteAsync();
	}
}
