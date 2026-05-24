// CopyRight https://github.com/fnaxi. All Rights Reserved.

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using InvaderZim.Config;

namespace InvaderZim;

public class CInvaderZim
{
	private static DiscordClient Client { get; set; }
	private static CommandsNextExtension Commands { get; set; }
	
	public static async Task Main()
	{
		CConfigReader Config = new CConfigReader();
		{
			Config.Parse();
		}
		DiscordConfiguration DisConfig = new DiscordConfiguration()
		{
			Intents = DiscordIntents.All,
			
			Token = Config.Token,
			TokenType = TokenType.Bot,
			AutoReconnect = true
		};
		
		Client =  new DiscordClient(DisConfig);
		Client.Ready += ClientOnReady;

		await Client.ConnectAsync();
		
		// Run the bot until exit
		await Task.Delay(-1);
	}

	private static Task ClientOnReady(DiscordClient sender, ReadyEventArgs args)
	{
		return Task.CompletedTask;
	}
}
