using Newtonsoft.Json;
using RestSharp;
using Steamworks;
using System;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
class GEVOTicket
{
    public enum ExitStatus : int
    {
        Success = 0,
        NoTicket = 1,
        HttpError = 2
    }
    class Ticket
    {
        public HAuthTicket Handle { get; set; } = HAuthTicket.Invalid;
        public string Value { get; set; } = "";
    }

    static Ticket GenerateTicket()
    {
        byte[] ticket = new byte[1024];
        return new Ticket {
            Handle = SteamUser.GetAuthSessionTicket(ticket, ticket.Length, out uint length),
            Value = BitConverter.ToString(ticket).Replace("-", "")[..((int)length * 2)] 
        };
    }

    static void InvalidateTicket(ref Ticket ticket)
    {
        SteamUser.CancelAuthTicket(ticket.Handle);
        ticket.Handle = HAuthTicket.Invalid;
        ticket.Value = "";
    }

    async static public Task<int> Main()
    {
        RestClient client = new("https://gevostats.com/api");
        Environment.SetEnvironmentVariable("SteamAppId", 1816670.ToString());
        Environment.SetEnvironmentVariable("SteamGameId", 1816670.ToString());
        if(!SteamAPI.Init())
        {
            Console.WriteLine("Steam API Failed!");
            return 100;
        }
        Ticket ticket = GenerateTicket();
        
        if (ticket.Handle == HAuthTicket.Invalid)
        {
            Console.WriteLine("Invalid Ticket, Unable to Retrieve User Stats");
            return (int) ExitStatus.NoTicket;
        }

        try
        {
            var result = await client.PostJsonAsync<Models.Request.SteamSync, Models.Response.SteamSync>("Sync", new Models.Request.SteamSync { ticket = ticket.Value });
        }catch(Exception)
        {
            Console.WriteLine("Error Sending Ticket.");
        }

        InvalidateTicket(ref ticket);

        if(ticket.Handle == HAuthTicket.Invalid)
            Console.WriteLine("Successfully Invalidated Ticket.");

        return (int) ExitStatus.Success;
    }
}