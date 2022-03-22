using System.Net.Http.Headers;
using Core.Utilities.Results;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using ServiceStack;

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
        var response1 = JsonConvert.DeserializeObject<response>(content);
        var response2 = JsonConvert.DeserializeObject<SuccessDataResult<bool>>(content);
        Console.WriteLine("content" + content);
        Console.WriteLine("response1" + JsonConvert.SerializeObject(response1));
        Console.WriteLine("response2" + JsonConvert.SerializeObject(response2));
        Console.WriteLine("response.Data" + response1.data);
        return response1.data;
    }
}

class response
{
    public bool data { get; set; }
    public string message { get; set; }
    public bool success { get; set; }
}