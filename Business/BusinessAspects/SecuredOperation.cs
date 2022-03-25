using System.Security;
using Business.Constants;
using Business.Helpers;
using Business.Services;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Core.Utilities.Security.Encyption;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
    private readonly ProjectManagementApiModel _projectManagementService;

    public SecuredOperationAttribute()
    {
        Configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
        _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        _operationClaimCrypto = Configuration.GetSection("OperationClaimCrypto").Get<string>();
        _projectManagementService = Configuration
            .GetSection("ProjectManagementService").Get<ProjectManagementApiModel>();
    }

    public IConfiguration Configuration { get; }

    protected override void OnBefore(IInvocation invocation)
    {
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

        var projectIdModel =
            JsonConvert.DeserializeObject<ProjectIdDto>(JsonConvert.SerializeObject(invocation.Arguments[0]));

        string token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"];
        long projectId = 0;
        if (_httpContextAccessor.HttpContext?.Request.Query["ProjectId"].ToString().Length > 0)
            projectId = Convert.ToInt64(_httpContextAccessor.HttpContext?.Request.Query["ProjectId"]);
        else if (projectIdModel.ProjectId != 0) projectId = projectIdModel.ProjectId;

        var httpUrl = "http://" + _projectManagementService.Host + ":" + _projectManagementService.Port +
                      "/api/CustomerProjects/isValid?projectId=" + projectId;

        var operationName = invocation.TargetType.ReflectedType.Name;
        if (!ocNameList.Contains(operationName))
            throw new SecurityException(Messages.AuthorizationsDenied);
        if (projectId == 0) return;
        if (!ProjectIdValidation.ValidateProjectId(httpUrl, token))
        {
            throw new SecurityException(Messages.AuthorizationsDenied);
        }
    }
}