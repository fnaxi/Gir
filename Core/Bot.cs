// CopyRight https://github.com/fnaxi. All Rights Reserved.

using Core.Config;
using Core.Services;
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
	
	public async Task Start(EBotType BotType)
	{
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
		await CServices.Connect();
		
		await Task.Delay(-1);
	}
	
	protected virtual void SetupServices()
	{ }
	
	protected virtual void RegisterCommands()
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

		RegisterCommands();
	}
	
	protected void RegisterCommandModule<T>() where T : BaseCommandModule
	{
		Commands.RegisterCommands<T>();
		LogInfo($"Registered {typeof(T).Name}");
	}
}
