using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Doulex.AspNetCore.Conventions;

/// <summary>
/// 控制器命名转换代码, 负责把默认的 camelCase 控制器名称转换为 kebab-case
/// </summary>
public class KebabControllerConvention : IControllerModelConvention
{
    /// <summary>
    /// Called to apply the convention to the <see cref="T:Microsoft.AspNetCore.Mvc.ApplicationModels.ControllerModel" />.
    /// </summary>
    /// <param name="controller">The <see cref="T:Microsoft.AspNetCore.Mvc.ApplicationModels.ControllerModel" />.</param>
    public void Apply(ControllerModel controller)
    {
        // convert the camel naming to kebab-naming
        var regex = new Regex("([a-z0-9])([A-Z])", RegexOptions.Compiled);
        controller.ControllerName = regex.Replace(controller.ControllerName, "$1-$2")
            .Replace("_", "-")
            .ToLower();
    }
}
