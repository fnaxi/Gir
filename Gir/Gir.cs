// CopyRight https://github.com/fnaxi. All Rights Reserved.

global using static Core.Misc.CUtils;
global using static Core.Misc.CDiscordUtils;
global using static Core.Commands.CCommandUtils;
global using static Core.CLog;

using Core;
using Core.Commands;
using Core.ID;
using Core.Services;
using Core.Services.Client;
using DSharpPlus.Entities;
using Gir.Commands;
using Gir.ID;
using Gir.Services.Client;
using Gir.Services.Client.System;
using Gir.Services.Commands;

namespace Gir;

public class CGirBot : CBotBase
{ 
	protected override void SetupServices()
	{
		DiscordEmoji WaffleEmoji = DiscordEmoji.FromName(Client, ":waffle:");
		DiscordEmoji ConquestEmoji = DiscordEmoji.FromName(Client, ":crossed_swords:");
		CActivityService ActivityService = new CActivityService(Client)
		{
			Statuses =
			[
				$"{WaffleEmoji} Eating tasty waffles",
				$"{ConquestEmoji} Conquering the world!"
			]
		};
		
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
			
			// TODO: clubs systems
			
			// System
			new CConnectionStatusService(Client),
		]);
		
		// TODO: move to CServiceManager
		CCommandErrorsService CommandErrorsService = new CCommandErrorsService(Commands);
	}
	
	protected override void RegisterCommands()
	{
		// Core
		SetupCoreCommands();
		
		// Gir
		RegisterCommandModule<CMiscCommands>();
		RegisterCommandModule<CModerationCommands>();
		RegisterCommandModule<CEntertainCommands>();
		RegisterCommandModule<CManagementCommands>();
		RegisterCommandModule<CTicketCommands>();
		RegisterCommandModule<CTestCommands>();
	}

	private void SetupCoreCommands()
	{
		// TODO: revisit this
		CCoreCommands.BotLatencyEmoji = CEmoji.BmoDance;
		CCoreCommands.ResponseLatencyEmoji = CEmoji.Alien;
		
		RegisterCommandModule<CCoreCommands>();
	}
}

internal abstract class CProgram
{
	private static Task Main(string[] Args)
	{
		LogCategory = "GirApp";
		return new CGirBot().Start(EBotType.Main);
	}
}