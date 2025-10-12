
using KioskApi2.HttpClients;
using Microsoft.Extensions.Caching.Memory;

namespace KioskApi2.Solar;

public class SolarManager(ISolarEdgeApiClient solarEdgeApiClient, IMemoryCache memoryCache) : ISolarManager
{
    public async Task<SolarData?> GetSolarData()
    {
        var key = "SolarData";

        if (!memoryCache.TryGetValue(key, out SolarData? data))
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            data = await solarEdgeApiClient.GetSolarData();

            if (data is not null)
                memoryCache.Set(key, data, cacheEntryOptions);
        }

        return data;
    }    
}
