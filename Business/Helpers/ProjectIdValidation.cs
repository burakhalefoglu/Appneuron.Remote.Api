using System.Net.Http.Headers;
using Core.Utilities.Results;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace Business.Helpers;

public static class ProjectIdValidation
{
    public static async Task<bool> ValidateProjectId(string httpUrl, long projectId, StringValues token)
    {
        using var client = new HttpClient();
        var msg = new HttpRequestMessage(HttpMethod.Get, httpUrl);
        client.DefaultRequestHeaders.Add("Authorization", token.ToString());
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var res = await client.SendAsync(msg);
        var content = await res.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<SuccessDataResult<bool>>(content);
        return true;  //response.Data;
    }
}