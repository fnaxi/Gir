// CopyRight https://github.com/fnaxi. All Rights Reserved.

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Gir.Misc;

namespace Gir.Services.Client;

public class CActivityService
{
	private readonly DiscordClient Client;
	public CActivityService(DiscordClient InClient)
	{
		Client = InClient;
		Client.Ready += Client_OnReady;
		
		DiscordEmoji WaffleEmoji = DiscordEmoji.FromName(Client, ":waffle:");
		DiscordEmoji MonsterEmoji = DiscordEmoji.FromName(Client, ":coffee:");
		DiscordEmoji ConquestEmoji = DiscordEmoji.FromName(Client, ":crossed_swords:");
		Statuses =
		[
			$"{WaffleEmoji} Eating tasty waffles",
			$"{MonsterEmoji} Drinking a white Monster",
			$"{ConquestEmoji} Conquering the world!"
		];
	}
	
	private const UInt16 UpdateTime = 720;
	private readonly List<string> Statuses;
	
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