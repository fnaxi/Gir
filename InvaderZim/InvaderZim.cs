// CopyRight https://github.com/fnaxi. All Rights Reserved.

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using InvaderZim.Commands;
using InvaderZim.Config;
using InvaderZim.ID;
using InvaderZim.Misc;

namespace InvaderZim;

public static class CInvaderZim
{
	private static DiscordClient Client { get; set; }
	private static CommandsNextExtension Commands { get; set; }
	
	private static CConfig Config;
	private static DiscordConfiguration DisConfig;
	
	public static async Task Main()
	{
		Config = new CConfigParser().Parse();
		DisConfig = new DiscordConfiguration
		{
			Intents = DiscordIntents.All,
			
			Token = Config.Token,
			TokenType = TokenType.Bot,
			AutoReconnect = true
		};
		
		Client =  new DiscordClient(DisConfig);
		Client.Ready += Client_OnReady;
		Client.MessageCreated += Client_OnMessageCreated;

		SetupCommands(Config.Prefix);
		
		await Client.ConnectAsync();
		await Task.Delay(-1);
	}

	private static async Task Client_OnReady(DiscordClient Sender, ReadyEventArgs Args)
	{
		CLog.Info("Zim is ready!");

		DiscordEmbedBuilder Embed = new DiscordEmbedBuilder()
		{
			Title = "Zim is eating waffles again!",
			Description = "Prepare your bladder for imminent release!",
			Color = YellowGreen
		};
		
		DiscordChannel Channel = await Sender.GetChannelAsync(CChannel.Test);
		await Channel.SendMessageAsync(Embed);
		
		await StartStatusRotation(Sender);
	}
	
	private static async Task Client_OnMessageCreated(DiscordClient Sender, MessageCreateEventArgs Args)
	{
		if (Args.Author.IsBot) return;
		
		// TODO: Revisit this. bot should react to mentioning "zim" in a message like "hey zim!"
		// Args.Message.Content.StartsWith(Config.Prefix)
		if (Args.Message.MentionedUsers.Any(user => user.Id == Sender.CurrentUser.Id))
		{
			await Args.Message.RespondAsync(RandomString(CQuote.Mention));
		}
	}
	
	private static async Task Commands_OnCommandErrored(CommandsNextExtension Sender, CommandErrorEventArgs Args)
	{
		if (Args.Exception is ArgumentException or DSharpPlus.CommandsNext.Exceptions.ChecksFailedException)
		{
			// TODO: Handle more cases here
			if (CanModerate(Args.Context))
			{
				if (Args.Command?.Name == "ban")
				{
					await Args.Context.RespondAsync("I can't find a member with such username or ID!");
				}
			}
		}
	}

	private static void SetupCommands(string Prefix)
	{
		CommandsNextConfiguration CommandsConfig = new CommandsNextConfiguration()
		{
			StringPrefixes = [Prefix],
			
			EnableMentionPrefix = true,
			EnableDms = false,
			
			EnableDefaultHelp = false
		};
		Commands = Client.UseCommandsNext(CommandsConfig);

		RegisterCommandModule<CMiscCommands>();
		RegisterCommandModule<CTestCommands>();
		RegisterCommandModule<CModerationCommands>();
		
		Commands.CommandErrored += Commands_OnCommandErrored;
	}
	
	private static async Task StartStatusRotation(DiscordClient Sender)
	{
		// TODO: See if custom emoji can be used here
		DiscordEmoji WaffleEmoji = DiscordEmoji.FromName(Sender, ":waffle:");
		DiscordEmoji MonsterEmoji = DiscordEmoji.FromName(Sender, ":cocktail:");
		DiscordEmoji ConquestEmoji = DiscordEmoji.FromName(Sender, ":earth_americas:");

		UInt16 UpdateTime = 30;
		List<string> Statuses = new List<string>
		{
			$"{WaffleEmoji} Eating tasty waffles",
			$"{MonsterEmoji} Drinking a white Monster",
			$"{ConquestEmoji} Conquering the world!"
			// TODO: More statuses
		};

		CLog.Info($"Status rotation loop started (updating every {UpdateTime} seconds)");
		while ( !(Sender.Ping > 3000) ) // TODO: Ambitious
		{
			try
			{
				string Status = RandomString(Statuses);

				await UpdateStatus(new DiscordActivity(Status, ActivityType.Playing), UserStatus.Online);
			}
			catch (Exception Ex)
			{
				CLog.Error($"Failed to update status: {Ex.Message}");
			}
			
			await Task.Delay(TimeSpan.FromSeconds(UpdateTime)); 
		}
	}
	
	private static async Task UpdateStatus(DiscordActivity Activity, UserStatus Status)
	{
		await Client.UpdateStatusAsync(Activity, Status);
		CLog.Info($"Bot status updated: {Activity.ActivityType} / {Activity.Name}");
	}
	
	private static void RegisterCommandModule<T>() where T : BaseCommandModule
	{
		Commands.RegisterCommands<T>();
		CLog.Info($"Registered {typeof(T).Name}");
	}
}
