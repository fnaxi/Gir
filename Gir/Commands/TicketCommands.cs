// CopyRight https://github.com/fnaxi. All Rights Reserved.

using System.Diagnostics;
using Core.Services;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Gir.ID;
using Gir.Services.Client;

namespace Gir.Commands;

[Group("ticket")]
public class CTicketCommands : BaseCommandModule
{
	[Command("close")]
	[Description("Closes the specified ticket")]
	public async Task Close(CommandContext Context,
		[Description("The reason of closing the ticket")] string Reason = "No reason provided")
	{
		CTicketsService? TicketsService = CServices.Get<CTicketsService>();
		Debug.Assert(TicketsService != null);
		
		if (!TicketsService.IsTicketChannel(Context.Channel))
		{
			await Context.RespondAsync($"This command can only be invoked in a ticket channel! {CEmoji.GirBlep}");
			return;
		}

		CTicket? Ticket = TicketsService.Tickets.Find(t => t.ChannelId == Context.Channel.Id);
		Debug.Assert(Ticket != null);

		await Ticket.Close(Context.Guild, Context.User, Context.Message, Reason);
	}
	
	[Command("open")]
	[Description("Re-opens the specified ticket")]
	public async Task Open(CommandContext Context)
	{
		if (!CanModerate(Context.Member))
		{
			await NoRights(Context);
			return;
		}
		
		CTicketsService? TicketsService = CServices.Get<CTicketsService>();
		Debug.Assert(TicketsService != null);
		
		if (!TicketsService.IsTicketChannel(Context.Channel))
		{
			await Context.RespondAsync($"This command can only be invoked in a ticket channel! {CEmoji.GirBlep}");
			return;
		}
		
		CTicket? Ticket = TicketsService.Tickets.Find(t => t.ChannelId == Context.Channel.Id);
		Debug.Assert(Ticket != null);

		await Ticket.Open(Context.Guild, Context.User);
	}
	
	[Command("delete")]
	[Description("Deletes the specified ticket")]
	[RequirePermissions(Permissions.Administrator)]
	public async Task Delete(CommandContext Context,
		[Description("The reason of deleting the ticket")] string Reason = "No reason provided")
	{
		if (!IsAdmin(Context.Member))
		{
			await NoRights(Context);
			return;
		}
		
		CTicketsService? TicketsService = CServices.Get<CTicketsService>();
		Debug.Assert(TicketsService != null);
		
		if (!TicketsService.IsTicketChannel(Context.Channel))
		{
			await Context.RespondAsync($"This command can only be invoked in a ticket channel! {CEmoji.GirBlep}");
			return;
		}
		
		CTicket? Ticket = TicketsService.Tickets.Find(t => t.ChannelId == Context.Channel.Id);
		Debug.Assert(Ticket != null);

		await TicketsService.DeleteTicket(Context.Guild, Ticket);
	}
}
