// CopyRight https://github.com/fnaxi. All Rights Reserved.

global using static Core.CUtils;
global using static Core.CLog;

using Core;
using Gir.Commands;
using Gir.Services.Client;
using Gir.Services.Client.System;
using Gir.Services.Commands;

namespace Gir;

public class CGirBot : CBotBase
{
	protected override void SetupServices()
	{
		CServices.Setup(
		[
			new CGreetingService(Client),
			new CTalkingService(Client),
			new CActivityService(Client),
			
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