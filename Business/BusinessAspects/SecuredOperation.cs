using System.Security;
using Business.Constants;
using Business.Services;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Core.Utilities.Results;
using Core.Utilities.Security.Encyption;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace Business.BusinessAspects;

/// <summary>
///     This Aspect control the user's roles in HttpContext by inject the IHttpContextAccessor.
///     It is checked by writing as [SecuredOperation] on the handler.
///     If a valid authorization cannot be found in aspect, it throws an exception.
/// </summary>
public class SecuredOperationAttribute : MethodInterceptionAttribute
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _operationClaimCrypto;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ProjectManagementService _projectManagementService;

    public SecuredOperationAttribute()
    {
        Configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
        _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        _operationClaimCrypto = Configuration.GetSection("OperationClaimCrypto").Get<string>();
        _projectManagementService = Configuration
            .GetSection("ProjectManagementService").Get<ProjectManagementService>();
        _httpClientFactory = ServiceTool.ServiceProvider.GetService<IHttpClientFactory>();
    }

    public IConfiguration Configuration { get; }

    protected override async void OnBefore(IInvocation invocation)
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        var userId = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(x => x.Type.EndsWith("nameidentifier"))?.Value;

        if (userId == null) throw new UnauthorizedAccessException(Messages.UnauthorizedAccess);

        var oprClaims = _httpContextAccessor.HttpContext?.User.Claims.Where(x => x.Type.EndsWith("role")).ToList();
        var ocNameList = new List<string>();

        foreach (var item in oprClaims)
        {
            var itemDecryptValue = SecurityKeyHelper.DecryptString(_operationClaimCrypto, item.Value);
            ocNameList.Add(itemDecryptValue);
        }

        var operationName = invocation.TargetType.ReflectedType.Name;
        if (!ocNameList.Contains(operationName))
            throw new SecurityException(Messages.AuthorizationsDenied);

        var projectIdModel =
            JsonConvert.DeserializeObject<ProjectIdDto>(JsonConvert.SerializeObject(invocation.Arguments[0]));

        if (request.Query["ProjectId"].ToString().Length > 0)
        {
            var ProjectId = Convert.ToInt64(request.Query["ProjectId"]);
            var token = request.Headers["Authorization"];
            // ask project management server!
            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                _projectManagementService.Host + ":" + _projectManagementService.Port +
                "/api/CustomerProjects/isValid?projectId=" + ProjectId)
            {
                Headers =
                {
                    {HeaderNames.Accept, "application/json"},
                    {HeaderNames.Authorization, token.ToString()}
                }
            };
        
            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            using var contentStream =
                await httpResponseMessage.Content.ReadAsStreamAsync();
        
            var response = await System.Text.Json.JsonSerializer.DeserializeAsync
                <SuccessDataResult<bool>>(contentStream);
        
            if (response is null || !response.Success)
                throw new SecurityException(Messages.Unknown);
        
            if (response is {Data: false})
                throw new SecurityException(Messages.AuthorizationsDenied);
        }
        
        if (projectIdModel.ProjectId == 0) return;
        {
            var token = request.Headers["Authorization"];
            // ask project management server!
            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get, "http://" +
                _projectManagementService.Host + ":" + _projectManagementService.Port +
                "/api/CustomerProjects/isValid?projectId=" + projectIdModel.ProjectId)
            {
                Headers =
                {
                    {HeaderNames.Accept, "application/json"},
                    {HeaderNames.Authorization, token.ToString()}
                }
            };
            
            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            using var contentStream =
                await httpResponseMessage.Content.ReadAsStreamAsync();
        
            var response = await System.Text.Json.JsonSerializer.DeserializeAsync
                <SuccessDataResult<bool>>(contentStream);
        
            if (response is null || !response.Success)
                throw new SecurityException(Messages.Unknown);
        
            if (response is {Data: false})
                throw new SecurityException(Messages.AuthorizationsDenied);
        }
    }
}