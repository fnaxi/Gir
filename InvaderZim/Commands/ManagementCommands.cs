// CopyRight https://github.com/fnaxi. All Rights Reserved.

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using InvaderZim.ID;

namespace InvaderZim.Commands;

public class CManagementCommands : BaseCommandModule
{
	[Command("news")]
	[Description("Reposts a message to news channel")]
	[RequirePermissions(Permissions.Administrator)]
	public async Task News(CommandContext Context)
	{
		DiscordChannel NewsChannel = Context.Guild.GetChannel(CChannel.News);

		DiscordMessage ReferencedMessage = Context.Message.ReferencedMessage;
		await NewsChannel.SendMessageAsync(ReferencedMessage.Content);

		await Context.Message.DeleteAsync();
		await ReferencedMessage.RespondAsync($"A message was reposted in {NewsChannel.Mention}");
	}
	
	[Group("send")]
	public class CSendCommands : BaseCommandModule
	{
		[Command("rules")]
		[Description("Sends rules message")]
		[RequirePermissions(Permissions.Administrator)]
		public async Task SendRules(CommandContext Context)
		{
			DiscordChannel TicketChannel = Context.Guild.GetChannel(CChannel.TicketCreator);
			
			DiscordRole AdminRole = Context.Guild.GetRole(CRole.Admin);
			DiscordRole ModeratorRole = Context.Guild.GetRole(CRole.Moderator);
			
			DiscordEmbedBuilder Embed = new DiscordEmbedBuilder()
			{
				Title = ":books: SERVER RULES :books:",
				Description = 
					$"Joining the server implies your agreement to the rules and that you are older than 16 years old (according to the PEGI rating in the [Steam](https://store.steampowered.com))" +
					$" {CEmoji.Alien}" +
					$"\n### 1. TALKING STUFF {CEmoji.GirDress}" +
					$"\n1.1. No mean stuff. Don’t be mean or bully people / bots." +
					$"\n\n1.2. No hate speech, racism, or yucky discrimination. Zim likes everyone! Even the Earth pigs in some way." +
					$"\n\n1.3. Do not send repeated messages, images, or @mentions. This includes excessive use of CAPS LOCK, emojis, or text walls that disrupt conversation." +
					$"\n\n1.4. Post content in the correct channels. Read the channel descriptions to understand their purpose." +
					$"\n\n1.5. NFSW content is forbidden." +
					$"\n\n1.6. Keep conversations primarily in US English so the moderation team can effectively keep the community safe." +
					$"\n### 2. SAFETY {CEmoji.BmoDance}" +
					$"\n2.1. Pinging {AdminRole.Mention} or {ModeratorRole.Mention} is forbidden if used for no good reason." +
					$"\n\n2.2. No doxxing. Sharing anyone’s private personal information (real name, location, photos, etc.) is forbidden." +
					$"\n\n2.3. No unauthorized promotion. Do not DM members or post in any channels to advertise your own server, social media, products, or etc without permission from an Admin" +
					$"\n\n2.4. Do not share links to pirated software, cheats, or malicious websites/viruses." +
					$"\n\n2.5. All members must adhere to the [Community Guidelines](https://discord.com/guidelines) and [Terms of Service](https://discord.com/guidelines)." +
					$"\n### REMARKS {CEmoji.GirDance}" +
					$"\n~ Violations of the rules may result in warnings, temporary mutes, or permanent bans depending on severity and frequency of the offense." +
					$"\n\n~ Any attempt to circumvent the rules will result in a ban." +
					$"\n\n~ Moderators may issue penalties outside the rules if it helps maintain the order on the server." +
					$"\n\n~ If you witness a rule being broken, please report it using a {TicketChannel.Mention}" +
					$"\n\nNow go eat some waffles! 🧇 You've made it this far xD",
				Color = YellowGreen
			};
			
			await Context.Channel.SendMessageAsync(Embed);
			await Context.Message.DeleteAsync();
		}
		
		[Command("color_roles")]
		[Description("Sends color roles message")]
		[RequirePermissions(Permissions.Administrator)]
		public async Task SendColorRoles(CommandContext Context)
		{
			const string Text =
				"Choose your **Robe** below to claim your place in the Mid-Atlantic Conclave!" +
				"\n" +
				"\n*Remember, Commandments dictate: Thou shalt choose your hue with pride. (And no, black is not an option)*";
			
			await Context.Channel.SendMessageAsync(Text);
			await Context.Message.DeleteAsync();
		}
		
		[Command("ticket_form")]
		[Description("Sends ticket message")]
		[RequirePermissions(Permissions.Administrator)]
		public async Task SendTicketForm(CommandContext Context)
		{
			DiscordEmbedBuilder Embed = new DiscordEmbedBuilder()
			{
				Title = $":blue_book: Support Tickets {CEmoji.BmoDance}",
				Description = 
					$"Click the button below to open a ticket and contact our support team.",
				Color = YellowGreen
			};
			Embed.AddField("What we can help with?", 
				"• Member reporting\n" +
				"• Server appeals\n" +
				"• General questions & feedback");

			DiscordComponentEmoji Emoji = new DiscordComponentEmoji( DiscordEmoji.FromName(Context.Client, ":envelope_with_arrow:") );
			DiscordButtonComponent Button = new DiscordButtonComponent(ButtonStyle.Primary, "SID_CreateTicket", "Create a Ticket", false, Emoji);
			
			DiscordMessageBuilder Message = new DiscordMessageBuilder();
			Message.WithEmbed(Embed);
			Message.AddComponents(Button);
			
			await Context.Channel.SendMessageAsync(Message);
			
			await Context.Message.DeleteAsync();
		}
		
		// TODO: revisit this
		[Command("support")]
		[RequirePermissions(Permissions.Administrator)]
		public async Task CallSupport(CommandContext Context)
		{
			DiscordRole ModeratorRole = Context.Guild.GetRole(CRole.Moderator);
			await Context.Channel.SendMessageAsync($"{ModeratorRole.Mention}");
			await Context.Message.DeleteAsync();
		}
	}
}
