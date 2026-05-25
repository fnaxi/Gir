// CopyRight https://github.com/fnaxi. All Rights Reserved.

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using InvaderZim.ID;

namespace InvaderZim.Commands;

public class CTestCommands : BaseCommandModule
{
	[Command("emojis")] [RequireUserPermissions(Permissions.Administrator)]
	public async Task Emojis(CommandContext Context)
	{
		DiscordEmbedBuilder Embed = new DiscordEmbedBuilder()
		{
			Title = "Emojis test",
			Description = $"{CEmoji.GirDance} \n {CEmoji.GirBlep} \n {CEmoji.GirDress} \n {CEmoji.GirLaugh} \n {CEmoji.GirLike}",
			Color = YellowGreen
		};
		await Context.Channel.SendMessageAsync(Embed);
	}
}
