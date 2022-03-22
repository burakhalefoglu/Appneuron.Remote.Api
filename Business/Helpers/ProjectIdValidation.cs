using System.Net.Http.Headers;
using Core.Utilities.Results;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace Business.Helpers;

public static class ProjectIdValidation
{
    public static bool ValidateProjectId(string httpUrl, StringValues token)
    {
        using var client = new HttpClient();
        var msg = new HttpRequestMessage(HttpMethod.Get, httpUrl);
        client.DefaultRequestHeaders.Add("Authorization", token.ToString());
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var res = client.Send(msg);
        using var reader = new StreamReader(res.Content.ReadAsStream());
        var content = reader.ReadToEnd();
        var response = JsonConvert.DeserializeObject<SuccessDataResult<bool>>(content);
        return response.Data;
    }
}