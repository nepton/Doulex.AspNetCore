# 基于策略的扩展验证框架
刘万里  
2020-07-10

# Summary
此目录实现一种扩展的基于策略的权限验证框架, 扩展了微软的不可传参的默认框架结构.

# What
实现扩展的基于策略的认证体系.

# Why
首先, 微软的框架前半部分设计的不太好, 导致无法把基于策略认证的 **参数** 传递到后端处理方法, 因此有了
这个扩展模型.

这个扩展模型在声明端, 直接就把 Requirement 对象生成, 然后交由对应的 Handler 处理, 这样一来我们可以
把各种自定义的参数从 Attrinute 特性生成端传递到 Handler 端

# How to use
这里以权限验证的模式举例: 
1. 定义一个 Requirement, 这部分与微软的框架一致
```C#
public class OperationAuthorizeRequirement : IAuthorizationRequirement
{
    public string PermissionDemand
    {
        get;
    }

    public OperationAuthorizeRequirement(string permissionDemand)
    {
        PermissionDemand = permissionDemand;
    }
}
```

2. 定义一个 Handler, 这部分的处理与微软认证框架一致, 这个方法负责处理具体的验证策略
```C#
public class OperationAuthorizeHandler : AuthorizationHandler<OperationAuthorizeRequirement>
{
   protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext        context,
        OperationAuthorizeRequirement requirement)
    {
    }
}
```


3. 定义一个需要处理的策略声明对象, 继承与 `PolicyBasedAuthorizeAttribute`, 并实现属性 Requirement 返回第一步定义的 Requirement.

> 注意, 这里的实现与微软不同
> 微软框架的策略定义有点死板, 因此这里扩展了微软的玩法

```c# 
class OperationAuthorizeAttribute : PolicyBasedAuthorizeAttribute
{
    protected override IAuthorizationRequirement Requirement => new OperationAuthorizeRequirement(PermissionDemand);

}
```

4. 在Ioc模块注册函数注册 Handler
```C#
services.AddSingleton<IAuthorizationHandler, OperationAuthorizeHandler>();
```
