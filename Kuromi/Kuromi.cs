// CopyRight https://github.com/fnaxi. All Rights Reserved.

global using static Core.CUtils;
global using static Core.CDiscordUtils;
global using static Core.CLog;

using Core;
using Core.Services;
using Core.Services.Client;
using DSharpPlus.Entities;
using Kuromi.Commands;
using Kuromi.Services.Client;

namespace Kuromi;

public class CKuromiBot : CBotBase
{
	// TODO: help command framework?
	
	protected override void SetupServices()
	{
		CActivityService ActivityService = new CActivityService(Client);
		{
			DiscordEmoji MotorcycleEmoji = DiscordEmoji.FromName(Client, ":motorcycle:");
			ActivityService.Statuses =
			[
				$"{MotorcycleEmoji} Driving the bike",
				// TODO: more statuses
			];
		}
		
		CServices.Setup(
		[
			ActivityService,
			new CMusicPlayingService(Client)
		]);
	}

	protected override void RegisterCommandModules()
	{
		RegisterCommandModule<CMusicCommands>();
	}
}

internal abstract class CEntryPoint
{
	private static Task Main(string[] Args)
	{
		return new CKuromiBot().Start(EBotType.Music);
	}
}
