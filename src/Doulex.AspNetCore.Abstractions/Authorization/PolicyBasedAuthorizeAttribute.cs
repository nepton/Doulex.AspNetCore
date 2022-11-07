using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace Doulex.AspNetCore.Authorization;

/// <summary>
/// 基于策略的认证特性头
/// </summary>
/// <remarks>
/// 注意, 继承自接口 AuthorizeAttribute 是为了修补 Swagger 不读取 IAuthorizeData 的问题.
/// 实际上, 微软是基于接口编程, 因此微软会从 IAuthorizeData 接口访问这些信息
/// </remarks>
public abstract class PolicyBasedAuthorizeAttribute : Attribute, IAuthorizeData, IAuthorizeDescription
{
    /// <summary>
    /// 参数模型, 该模型将被传送到策略处理函数, 因此, 把所有需要的参数构建为模型对象返回
    /// </summary>
    protected abstract IAuthorizationRequirement Requirement { get; }

    /// <summary>
    /// Gets or sets the policy name that determines access to the resource.
    /// </summary>
    string? IAuthorizeData.Policy
    {
        get
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting       = Formatting.Indented,
            };

            if (Requirement == null)
                throw new Exception("缺少策略参数模型");

            return JsonConvert.SerializeObject(Requirement, settings);
        }
        set { }
    }

    /// <summary>
    /// 从 PolicyName 解析 Requirement, 微软架构设计不合理, 导致解析工作有点奇怪
    /// </summary>
    /// <param name="jsonText"></param>
    /// <returns></returns>
    public static IAuthorizationRequirement ParseRequirementFromPolicyName(string jsonText)
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
        };

        return JsonConvert.DeserializeObject<IAuthorizationRequirement>(jsonText, settings)!;
    }

    /// <summary>
    /// Gets or sets a comma delimited list of schemes from which user information is constructed.
    /// </summary>
    string? IAuthorizeData.AuthenticationSchemes { get; set; }

    /// <summary>
    /// Gets or sets a comma delimited list of roles that are allowed to access the resource.
    /// </summary>
    string? IAuthorizeData.Roles { get; set; }

    /// <summary>
    /// 授权的描述信息 (For Dev)
    /// </summary>
    public abstract string Description { get; }
}
