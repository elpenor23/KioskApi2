using System.Text.Json;

namespace KioskApi2.HttpClients;

public class HttpClientBase (HttpClient httpClient)
{
    protected async Task<T?> GetAsync<T>(string endPoint) where T : class
    {
        var response = await httpClient.GetAsync(endPoint);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();

        if (IsValidJson<T>(result))
            return JsonSerializer.Deserialize<T>(result);

        return result as T;

    }

    protected async Task<string> GetAsyncString(string endPoint)
    {
        var response = await httpClient.GetAsync(endPoint);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();

        return result;

    }

    protected static bool IsValidJson<T>(string strInput)
    {
        if (string.IsNullOrWhiteSpace(strInput)) { return false; }
        strInput = strInput.Trim();

        if
        (!(strInput.StartsWith("{") && strInput.EndsWith("}")) //For object
            && !(strInput.StartsWith("[") && strInput.EndsWith("]")) //For array
        )
        {
            //We are not JSON
            return false;
        }

        try
        {
            var obj = JsonDocument.Parse(strInput);
            return true;
        }
        catch (JsonException jex)
        {
            //Exception in parsing json
            Console.WriteLine(jex.Message);
            return false;
        }
        catch (Exception ex) //some other exception
        {
            Console.WriteLine(ex.ToString());
            return false;
        }

    }
}
