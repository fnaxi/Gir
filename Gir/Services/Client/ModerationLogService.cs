// CopyRight https://github.com/fnaxi. All Rights Reserved.

using Core.ID;
using Core.Services;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Gir.ID;

namespace Gir.Services.Client;

public class CModerationLogService : CServiceBase
{
	public CModerationLogService(DiscordClient Client) : base(Client)
	{
		Client.MessageUpdated += Client_OnMessageUpdated;
		Client.MessageDeleted += Client_OnMessageDeleted;
	}
	
	private async Task Client_OnMessageUpdated(DiscordClient Sender, MessageUpdateEventArgs Args)
	{
		// TODO: IMPORTANT! replace "gir " with actual prefix from the config
		if (Args.Message.Author.IsBot || Args.Message.Content == Args.MessageBefore.Content || Args.Message.Content.Contains("gir? ")) return;
		
		DiscordChannel ModLogChannel = Args.Guild.GetChannel(CChannel.ModLog);
		
		DiscordEmbedBuilder Embed = new DiscordEmbedBuilder()
		{
			Title = $"Message was edited in {Args.Channel.Mention} {CEmoji.GirBlep}",
			Description =
				$"### Before:\n{Args.MessageBefore.Content}\n" +
				$"### After:\n{Args.Message.Content}",
			Color = YellowGreen
		};
		Embed.WithFooter($"Author: {Args.Message.Author.Username}", Args.Message.Author.AvatarUrl);
		Embed.WithTimestamp(DateTime.UtcNow);
		
		await ModLogChannel.SendMessageAsync(Embed);
	}
	
	private async Task Client_OnMessageDeleted(DiscordClient Sender, MessageDeleteEventArgs Args)
	{
		if (Args.Message.Author.IsBot || Args.Message.Author == null || Args.Message.Content.Contains("gir? ")) return;
		
		DiscordChannel ModLogChannel = Args.Guild.GetChannel(CChannel.ModLog);
		
		DiscordEmbedBuilder Embed = new DiscordEmbedBuilder()
		{
			Title = $"Message was deleted in {Args.Channel.Mention} {CEmoji.GirBlep}",
			Description = $"{Args.Message.Content}",
			Color = YellowGreen
		};
		Embed.WithFooter($"Author: {Args.Message.Author.Username}", Args.Message.Author.AvatarUrl);
		Embed.WithTimestamp(DateTime.UtcNow);
		
		await ModLogChannel.SendMessageAsync(Embed);
	}
}
