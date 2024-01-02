using System.Net.Http.Headers;

namespace KioskApi2.Utilities;
public static class Arrays
{
    public static bool AreAllTheSameLength(params Array[] arrays)
    {
        return arrays.All(a => a.Length == arrays[0].Length);
    }
    
}

public static class ApiUtils{
    public async static Task<string> GetApiJsonData(string uri){
        var result = "[{}]";

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            else
            {
                var errorMessage = string.Format($"Error Getting data from {uri}. Status Code: {response.StatusCode}");
                throw new Exception(errorMessage);
            }
        }

        return result;
    }
}