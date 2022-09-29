using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>Extension methods for Mvc.Newtonsoft.Json options.</summary>
public static class MvcNewtonsoftJsonOptionsExtensions
{
    /// <summary>
    /// Configures the casing behavior of JSON serialization to use camel case for property names,
    /// and optionally for dynamic types and dictionary keys.
    /// </summary>
    /// <remarks>
    /// This method modifies <see cref="P:Newtonsoft.Json.JsonSerializerSettings.ContractResolver" />.
    /// </remarks>
    /// <param name="options"><see cref="T:Microsoft.AspNetCore.Mvc.MvcNewtonsoftJsonOptions" /></param>
    /// <param name="processDictionaryKeys">If true will camel case dictionary keys and properties of dynamic objects.</param>
    /// <returns><see cref="T:Microsoft.AspNetCore.Mvc.MvcNewtonsoftJsonOptions" /> with camel case settings.</returns>
    public static MvcNewtonsoftJsonOptions UseSnakeCasing(
        this MvcNewtonsoftJsonOptions options,
        bool processDictionaryKeys)
    {
        if (options == null)
            throw new ArgumentNullException(nameof(options));
        if (options.SerializerSettings.ContractResolver is DefaultContractResolver contractResolver)
        {
            var caseNamingStrategy = new SnakeCaseNamingStrategy
            {
                ProcessDictionaryKeys = processDictionaryKeys
            };
            contractResolver.NamingStrategy = caseNamingStrategy;
            return options;
        }

        if (options.SerializerSettings.ContractResolver == null)
            throw new InvalidOperationException("ContractResolver is null.");

        throw new InvalidOperationException($"ContractResolver is not of type {nameof(DefaultContractResolver)}.");
    }
}
