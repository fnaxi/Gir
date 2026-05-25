// CopyRight https://github.com/fnaxi. All Rights Reserved.


using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using InvaderZim.ID;

namespace InvaderZim.Commands;

public class CMiscCommands : BaseCommandModule
{
	[Command("shutdown")] [RequireUserPermissions(Permissions.Administrator)]
	public async Task Shutdown(CommandContext Context)
	{
		await Context.RespondAsync("Shutting down...");

		await Context.Client.DisconnectAsync();
		Environment.Exit(0);
	}
	
	[Command("help")]
	public async Task Help(CommandContext Context)
	{
		DiscordEmbedBuilder Embed = new DiscordEmbedBuilder()
		{
			Title = "Help menu",
			Description = "some help text", // TODO: Help command
			Color = YellowGreen
		};
		await Context.RespondAsync(Embed);
	}
}
