// CopyRight https://github.com/fnaxi. All Rights Reserved.

using Core.ID;
using Core.Services;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Gir.Services.Client;

public class CTemporaryVoicesService : CServiceBase
{
	public CTemporaryVoicesService(DiscordClient Client) : base(Client)
	{
		Client.GuildDownloadCompleted += Client_OnGuildDownloadCompleted;
		Client.VoiceStateUpdated += Client_OnVoiceStateUpdated;
	}
	
	private const string RoomName = "Room";
	
	private async Task Client_OnGuildDownloadCompleted(DiscordClient Sender, GuildDownloadCompletedEventArgs Args)
	{
		foreach (DiscordGuild? Guild in Sender.Guilds.Values)
		{
			if (Guild == null) continue;

			foreach (DiscordChannel? Channel in Guild.Channels.Values)
			{
				if (Channel == null || Channel.Type != ChannelType.Voice) continue;

				if (Channel.Id == CChannel.CreateVoice)
				{
					foreach (DiscordMember? Member in Channel.Users)
					{
						DiscordChannel NewChannel = await CreateTempVoice(Guild, Member.DisplayName);

						await Member.ModifyAsync(x => x.VoiceChannel = NewChannel);
					}
					continue;
				}
				
				if (!IsTempVoice(Channel) || Channel.Users.Count != 0) continue;
				
				await Channel.DeleteAsync();
			}
		}
	}
	
	private async Task Client_OnVoiceStateUpdated(DiscordClient Sender, VoiceStateUpdateEventArgs Args)
	{
		DiscordGuild Guild = Args.Guild;
		
		if (Args.After?.Channel != null && Args.After.Channel.Id == CChannel.CreateVoice)
		{
			DiscordMember	Member		= await Guild.GetMemberAsync(Args.User.Id);
			DiscordChannel	NewChannel	= await CreateTempVoice(Guild, Member.DisplayName);
			
			await Member.ModifyAsync(x => x.VoiceChannel = NewChannel);
		}
		else if (Args.Before?.Channel != null && IsTempVoice(Args.Before.Channel))
		{
			await Args.Before.Channel.DeleteAsync();
		}
	}
	
	private static bool IsTempVoice(DiscordChannel Channel)
	{
		return Channel.Id is not (CChannel.CreateVoice or CChannel.AFK) && Channel.Name.Contains(RoomName);
	}
	
	private async Task<DiscordChannel> CreateTempVoice(DiscordGuild Guild, string MemberName)
	{
		string			Name				= GenerateTempVoiceChannelName(MemberName);
		DiscordChannel	CreateVoiceChannel	= Guild.GetChannel(CChannel.CreateVoice);

		return await Guild.CreateVoiceChannelAsync(Name, Guild.GetChannel(CCategory.VoiceChannels), null, CreateVoiceChannel.UserLimit);
	}
	
	private string GenerateTempVoiceChannelName(string MemberName)
	{
		return $"🜋 {MemberName}'s {RoomName}";
	}
}
