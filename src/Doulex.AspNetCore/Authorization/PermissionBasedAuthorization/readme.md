# Permission based authorization

Permission based authorization is an authorization validation method that is based on the permissions of the user. The
permissions are stored in the database and are assigned to the user. The permissions are then checked against the
permissions required for the action. If the user has the required permissions, the action is allowed. If the user does
not have the required permissions, the action is denied.

# How to use

## 1. Create a permission requirement

Permission requirements are created by deriving the `PermissionRequirement` class. The class takes metadata information
from the `PermissionRequirementAttribute` attribute to `IPermissionBasedAuthorizationService`. The metadata should
contain any information that is needed to determine if the user has the required permissions.

```csharp
/// <summary>
/// The permission demand from API controller. 
/// </summary>
public class AdminPermissionRequirement : PermissionBasedRequirement
{
    /// <summary>
    /// The scope of permission. like admin, user, etc.
    /// </summary>
    public string Scope { get; }

    /// <summary>
    /// The permission string text.
    /// </summary>
    public string Permission { get; }

    public AdminPermissionRequirement(string scope, string permission)
    {
        Scope      = scope;
        Permission = permission;
    }
}
```

## 2. Create a permission requirement attribute

The attribute is used to declare the permission requirement on the API controller. The attribute takes the metadata

```csharp
/// <summary>
/// Certification Policy - Operator
/// </summary>
public class AdminPermissionAuthorizeAttribute : PermissionBasedAuthorizeAttribute
{
    /// <summary>
    /// Required Permissions
    /// </summary>
    protected override IAuthorizationRequirement Requirement => new AdminPermissionRequirement("admin", "admin");
}
```

## 3. Create a permission based authorization service implement from `IPermissionBasedAuthorizationService`

The authorization service is used to determine if the user has the required permissions. The service is registered in
the
`Startup.cs` file.

```csharp
public class AdminPermissionBasedAuthorizationService : IPermissionBasedAuthorizationService
{
    private readonly IPermissionService _permissionService;

    public AdminPermissionBasedAuthorizationService(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    public async Task EnsureAuthorizedAsync(loginToken user, IAuthorizationRequirement requirement)
    {
        if (requirement is AdminPermissionRequirement adminPermissionRequirement)
        {
            var hasPermission = await _permissionService.HasPermissionAsync(user, adminPermissionRequirement.Scope, adminPermissionRequirement.Permission);
            if (!hasPermission)
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}
```

## 4. Register the authorization service in the `Startup.cs` file

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    services.AddPermissionBasedAuthorization<PermissionBasedAuthorizationService, PermissionRequirement>();
}
```

## 5. Use the attribute on the API controller

```csharp
[AdminPermissionAuthorize(AdminPermission.Admin_R)]
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello World");
    }
}
```
