// CopyRight https://github.com/fnaxi. All Rights Reserved.

using Core.Config;
using DSharpPlus;
using DSharpPlus.CommandsNext;

namespace Core;

public enum EBotType
{
	Main,
	Music
}

public class CBotBase
{
	protected DiscordClient Client = null!;
	protected CommandsNextExtension Commands { get; private set; } = null!;
	
	private EBotType BotType;
	
	public async Task Start(EBotType InBotType)
	{
		BotType = InBotType;
		
		CConfig Config = CConfig.Parse(Directory.GetCurrentDirectory());
		DiscordConfiguration DisConfig = new DiscordConfiguration
		{
			Intents = DiscordIntents.All,
			
			Token = Config.GetToken(BotType),
			TokenType = TokenType.Bot,
			AutoReconnect = true,
			
			LogUnknownEvents = true,
			MinimumLogLevel = MinimumLogLevel,
			LogTimestampFormat = LogTimestampFormat
		};

		Client = new DiscordClient(DisConfig);
		
		SetupCommands(Config.GetPrefix(BotType));
		SetupServices();

		await Client.ConnectAsync();
		await Task.Delay(-1);
	}
	
	protected virtual void SetupServices() 
	{ }
	
	protected virtual void RegisterCommandModules()
	{ }

	private void SetupCommands(string Prefix)
	{
		CommandsNextConfiguration Config = new CommandsNextConfiguration()
		{
			StringPrefixes = [Prefix],
			
			IgnoreExtraArguments = true,
			EnableMentionPrefix = false,
			EnableDms = false,
			
			EnableDefaultHelp = false
		};
		Commands = Client.UseCommandsNext(Config);

		RegisterCommandModules();
	}
	
	protected void RegisterCommandModule<T>() where T : BaseCommandModule
	{
		Commands.RegisterCommands<T>();
		LogInfo($"Registered {typeof(T).Name}");
	}
}
