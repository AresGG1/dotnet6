using System.Net;
using System.Text;
using Gateway.Models;
using Newtonsoft.Json;
using Ocelot.Middleware;
using Ocelot.Multiplexer;

namespace Gateway.Aggregator;

public class PilotsDestinationsAggregator : IDefinedAggregator
{
    public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
    {
        var pilotsResponse = await responses[0].Items.DownstreamResponse().Content.ReadAsStringAsync();
        var destinationsResponse = await responses[1].Items.DownstreamResponse().Content.ReadAsStringAsync();

        var pilots = JsonConvert.DeserializeObject<IEnumerable<Pilot>>(pilotsResponse);
        var destinations = JsonConvert.DeserializeObject<IEnumerable<FlightDestination>>(destinationsResponse);
        
        var pilotsDestinations = new PilotsDestinations
        {
            Pilots = pilots.ToList(),
            Destinations = destinations.ToList()
        };
        
        var json = JsonConvert.SerializeObject(pilotsDestinations);
        
        return await Task.FromResult(new DownstreamResponse(
            new StringContent(json),
            HttpStatusCode.OK,
            new List<Header>(),
            "OK"
            ));
        
    }
}