// CopyRight https://github.com/fnaxi. All Rights Reserved.

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Core.Services.Client;

public class CActivityService : CServiceBase
{
	public CActivityService(DiscordClient InClient) : base(InClient)
	{
		Client.Ready += Client_OnReady;
	}
	
	public UInt16 UpdateTime = 720;
	public List<string> Statuses;
	
	private async Task Client_OnReady(DiscordClient Sender, ReadyEventArgs Args)
	{
		await StartStatusUpdating(Sender);
	}

	private async Task StartStatusUpdating(DiscordClient Sender)
	{
		LogInfo($"Status update loop has started (update time: {UpdateTime} seconds)");
		while ( IsBotAlive(Sender) )
		{
			string Status = RandomString(Statuses);

			await UpdateActivity(new DiscordActivity(Status, ActivityType.Playing), UserStatus.Online);
			await Task.Delay(TimeSpan.FromSeconds(UpdateTime));
		}
	}
	
	private async Task UpdateActivity(DiscordActivity Activity, UserStatus Status)
	{
		await Client.UpdateStatusAsync(Activity, Status);
		LogInfo($"Bot activity updated: {Activity.ActivityType} / {RemoveSpecialCharacters(Activity.Name)}");
	}
}