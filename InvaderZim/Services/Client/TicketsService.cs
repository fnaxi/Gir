// CopyRight https://github.com/fnaxi. All Rights Reserved.

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using InvaderZim.ID;
using InvaderZim.Misc;

namespace InvaderZim.Services.Client;

public class CTicket
{
	public CTicket(string InName, Int32 InId, UInt64 InChannelId, UInt64 InGuildId)
	{
		Name = InName;
		Id = InId;
		ChannelId = InChannelId;
		GuildId = InGuildId;
	}
	
	public string Name;
	public Int32 Id;
	public UInt64 ChannelId;
	public UInt64 GuildId;
}

public class CTicketsService
{
	public CTicketsService(DiscordClient Client)
	{
		Client.GuildDownloadCompleted += Client_OnGuildDownloadCompleted;
		Client.ComponentInteractionCreated += Client_OnComponentInteractionCreated;
	}

	private List<CTicket> Tickets = new();
	private const string TicketPrefix = "メticket-";

	private async Task Client_OnGuildDownloadCompleted(DiscordClient Sender, GuildDownloadCompletedEventArgs Args)
	{
		foreach (DiscordGuild Guild in Args.Guilds.Values)
		{
			LogDebug($"Guild {Guild.Name} ({Guild.Id})");
			IReadOnlyList<DiscordChannel> Channels = await Guild.GetChannelsAsync();
			foreach (DiscordChannel Channel in Channels)
			{
				if (!IsTicketChannel(Channel)) continue;
				
				LogDebug($"{Channel.Name}");
				Tickets.Add(new CTicket(Channel.Name, Int32.Parse(Channel.Name.Split('-')[1]), Channel.Id, Guild.Id));
			}
		}
	}

	private async Task Client_OnComponentInteractionCreated(DiscordClient Sender, ComponentInteractionCreateEventArgs Args)
	{
		switch (Args.Id)
		{
			case "SID_CreateTicket":
			{
				Int32 Id = Tickets.Count + 1;
				string Name = $"{TicketPrefix}{(Id):D4}";
				DiscordChannel ModerationCategory = Args.Guild.GetChannel(CCategory.Moderation);

				List<DiscordOverwriteBuilder> Permissions =
				[
					new DiscordOverwriteBuilder(Args.Guild.EveryoneRole).Deny(DSharpPlus.Permissions.AccessChannels),
					new DiscordOverwriteBuilder(Args.User as DiscordMember).Allow(DSharpPlus.Permissions.AccessChannels),
					new DiscordOverwriteBuilder(Args.Guild.GetRole(CRole.Moderator)).Allow(DSharpPlus.Permissions.AccessChannels)
				];

				DiscordChannel Channel = await Args.Guild.CreateChannelAsync(Name, ChannelType.Text, ModerationCategory, default, null, null, Permissions);

				DiscordEmbedBuilder Embed = new DiscordEmbedBuilder()
				{
					Title = $"{Name} {CEmoji.GirDance}",
					Description =
						"\n" +
						$"Support will be with you shortly!\n" +
						$"Click the button below to close this ticket.\n" +
						$"\n" +
						$"Author: {Args.User.Username} / {Args.User.Id}\n",
					Color = YellowGreen
				};

				DiscordComponentEmoji Emoji = new DiscordComponentEmoji(DiscordEmoji.FromName(Sender, ":lock:"));
				DiscordButtonComponent CloseButton = new DiscordButtonComponent(ButtonStyle.Primary, "SID_CloseTicket", "Close", false, Emoji);

				DiscordMessageBuilder Message = new DiscordMessageBuilder();
				Message.WithContent($"{Args.User.Mention} Welcome!");
				Message.WithEmbed(Embed);
				Message.AddComponents(CloseButton);

				await Channel.SendMessageAsync(Message);

				Tickets.Add(new CTicket(Name, Id, Channel.Id, Args.Guild.Id));
				LogInfo($"Created new ticket ({Name}/{Id}, Channel/{Channel.Id}, Guild/{Args.Guild.Id})");

				await Args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
			} break;
			case "SID_CloseTicket":
			{
				// TODO: closing the ticket
			} break;
		}
	}

	private List<CTicket> GetTicketsFromGuild(DiscordGuild Guild)
	{
		return Tickets.Where(t => t.GuildId == Guild.Id).ToList();
	}

	private bool IsTicketChannel(DiscordChannel Channel)
	{
		return Channel.Type == ChannelType.Text && Channel.Name.Contains(TicketPrefix) && Channel.Id != CChannel.TicketCreator;
	}
}
