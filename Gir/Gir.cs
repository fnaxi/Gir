// CopyRight https://github.com/fnaxi. All Rights Reserved.

global using static Core.CUtils;
global using static Core.CDiscordUtils;
global using static Core.CLog;

using Core;
using Core.Services;
using Core.Services.Client;
using DSharpPlus.Entities;
using Gir.Commands;
using Gir.Services.Client;
using Gir.Services.Client.System;
using Gir.Services.Commands;

namespace Gir;

public class CGirBot : CBotBase
{ 
	protected override void SetupServices()
	{
		CActivityService ActivityService = new CActivityService(Client);
		{
			DiscordEmoji WaffleEmoji = DiscordEmoji.FromName(Client, ":waffle:");
			DiscordEmoji MonsterEmoji = DiscordEmoji.FromName(Client, ":clinking_glass:");
			DiscordEmoji ConquestEmoji = DiscordEmoji.FromName(Client, ":crossed_swords:");
			ActivityService.Statuses =
			[
				$"{WaffleEmoji} Eating tasty waffles",
				$"{MonsterEmoji} Drinking a white Monster",
				$"{ConquestEmoji} Conquering the world!"
			];
		}
		
		CServices.Setup(
		[
			ActivityService,
			new CGreetingService(Client),
			new CTalkingService(Client),
			
			new CHelpMenuService(Client),
			new CTemporaryVoicesService(Client),
			new CTicketsService(Client),
			new CColorRolesService(Client),
			
			new CModerationLogService(Client),
			new CAutoModerationService(Client),
			
			// System
			new CConnectionStatusService(Client),
		]);
		
		CCommandErrorsService CommandErrorsService = new CCommandErrorsService(Commands); // TODO: move to CServiceManager
	}

	protected override void RegisterCommandModules()
	{
		RegisterCommandModule<CMiscCommands>();
		RegisterCommandModule<CModerationCommands>();
		RegisterCommandModule<CEntertainCommands>();
		RegisterCommandModule<CManagementCommands>();
		RegisterCommandModule<CTicketCommands>();
		RegisterCommandModule<CTestCommands>();
	}
}

internal abstract class CProgram
{
	private static Task Main(string[] Args)
	{
		return new CGirBot().Start(EBotType.Main);
	}
}