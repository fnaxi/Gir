// CopyRight https://github.com/fnaxi. All Rights Reserved.

using System.Diagnostics;
using Core.ID;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Core.Commands;

public class CCoreCommands : BaseCommandModule
{
	// TODO: help command framework?
	
	public static string BotLatencyEmoji = ":hourglass:";
	public static string ResponseLatencyEmoji = ":speech_balloon:";
	
	[Command("ping")]
	[Description("Checks the bot's latency and response time")]
	public async Task Ping(CommandContext Context)
	{
		if (SentInBotChannel(Context.Channel)) return;
		
		DiscordEmbedBuilder CalculatingEmbed = new DiscordEmbedBuilder()
		{
			Title = "Ping status",
			Description = $"Calculating latency... {BotLatencyEmoji}",
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
		
		FinalEmbed.AddField($"{BotLatencyEmoji} Bot Latency", $"`{WebsocketPing}ms`", true);
		FinalEmbed.AddField($"{ResponseLatencyEmoji} Response Latency", $"`{ResponsePig}ms`", true);

		Debug.Assert(Context.Member != null);
		FinalEmbed.WithFooter($"Requested by {Context.Member.DisplayName}", Context.Member.AvatarUrl);
		FinalEmbed.WithTimestamp(DateTime.UtcNow);
		
		await Message.ModifyAsync(FinalEmbed.Build());
	}

	[Command("shutdown")]
	[Description("Shuts down the bot")]
	[RequireUserPermissions(Permissions.Administrator)]
	public async Task Shutdown(CommandContext Context)
	{
		if (SentInBotChannel(Context.Channel)) return;

		DiscordMessage Response = await Context.RespondAsync("Shutting down...");

		await Context.Client.DisconnectAsync();
		Environment.Exit(0);
		
		await Task.Delay(TimeSpan.FromSeconds(TemporaryResponseTime));
		await Response.DeleteAsync();
	}
}
