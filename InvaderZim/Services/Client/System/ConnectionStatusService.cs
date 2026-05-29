// CopyRight https://github.com/fnaxi. All Rights Reserved.

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using InvaderZim.ID;
using InvaderZim.Misc;

namespace InvaderZim.Services.Client.System;

public class CConnectionStatusService
{
	private readonly DiscordClient Client;
	public CConnectionStatusService(DiscordClient InClient)
	{
		Client = InClient;
		Client.Ready += Client_OnReady;
		Client.GuildDownloadCompleted += Client_OnGuildDownloadCompleted;
	}

	private Task Client_OnReady(DiscordClient Sender, ReadyEventArgs Args)
	{
		CLog.Info("Zim is ready!");
		
		#if TODO
		DiscordEmbedBuilder Embed = new DiscordEmbedBuilder()
		{
			Title = "Gir is eating waffles again!",
			Description = "Prepare your bladder for imminent release!",
			Color = YellowGreen
		};

		DiscordChannel Channel = await Sender.GetChannelAsync(CChannel.Test);
		await Channel.SendMessageAsync(Embed);
		#endif
		
		return Task.CompletedTask;
	}
	
	private Task Client_OnGuildDownloadCompleted(DiscordClient Sender, GuildDownloadCompletedEventArgs Args)
	{
		CLog.Info("Guild download completed");
		
		return Task.CompletedTask;
	}
}
