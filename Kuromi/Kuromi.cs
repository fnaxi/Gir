// CopyRight https://github.com/fnaxi. All Rights Reserved.

global using static Core.Misc.CUtils;
global using static Core.Misc.CDiscordUtils;
global using static Core.Commands.CCommandUtils;
global using static Core.CLog;

using Core;
using Core.Commands;
using Core.Services;
using Core.Services.Client;
using DSharpPlus.Entities;
using Kuromi.Commands;
using Kuromi.Services.Client;

namespace Kuromi;

public class CKuromiBot : CBotBase
{
	protected override void SetupServices()
	{
		CActivityService ActivityService = new CActivityService(Client);
		{
			DiscordEmoji MotorcycleEmoji = DiscordEmoji.FromName(Client, ":motorcycle:");
			DiscordEmoji CoffeeEmoji = DiscordEmoji.FromName(Client, ":coffee:");
			ActivityService.Statuses =
			[
				$"{MotorcycleEmoji} Driving the bike",
				$"{CoffeeEmoji} Drinking tasty coffee"
			];
		}
		
		CServices.Setup(
		[
			ActivityService,
			new CLavalinkService(Client)
		]);
	}

	protected override void RegisterCommands()
	{
		// Core
		RegisterCommandModule<CCoreCommands>();

		// Kuromi
		RegisterCommandModule<CMusicCommands>();
	}
}

internal abstract class CEntryPoint
{
	private static Task Main(string[] Args)
	{
		LogCategory = "KuromiApp";
		return new CKuromiBot().Start(EBotType.Music);
	}
}
