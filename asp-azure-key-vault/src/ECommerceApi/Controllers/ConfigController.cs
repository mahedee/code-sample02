using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.Controllers;

/// <summary>
/// Development-only endpoint to verify Azure Key Vault secrets are loaded correctly.
/// Remove or restrict this controller before deploying to production.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ConfigController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IWebHostEnvironment _env;

    // Keys expected to be loaded from Azure Key Vault
    private static readonly string[] SecretKeys =
    [
        "SqlServer:ConnectionString",
        "Jwt:SigningSecret",
        "Jwt:Issuer",
        "Jwt:Audience",
        "Smtp:Host",
        "Smtp:Port",
        "Smtp:Username",
        "Smtp:Password",
        "Smtp:FromEmail",
        "Smtp:FromName"
    ];

    // Keys whose values should be masked for safety
    private static readonly string[] SensitiveKeys =
    [
        "SqlServer:ConnectionString",
        "Jwt:SigningSecret",
        "Smtp:Password"
    ];

    public ConfigController(IConfiguration config, IWebHostEnvironment env)
    {
        _config = config;
        _env = env;
    }

    /// <summary>
    /// Returns all Azure Key Vault secret keys and their values.
    /// Sensitive values (passwords, secrets) are partially masked.
    /// Only available in Development environment.
    /// </summary>
    [HttpGet("keyvault-secrets")]
    public IActionResult GetKeyVaultSecrets()
    {
        if (!_env.IsDevelopment())
            return NotFound();

        var result = SecretKeys.Select(key =>
        {
            var value = _config[key];
            var isSensitive = SensitiveKeys.Contains(key);

            return new
            {
                Key = key,
                Value = value is null
                    ? "(not set)"
                    : isSensitive
                        ? MaskValue(value)
                        : value,
                IsSet = value is not null
            };
        });

        return Ok(result);
    }

    private static string MaskValue(string value)
    {
        if (value.Length <= 6)
            return new string('*', value.Length);

        return value[..3] + new string('*', value.Length - 6) + value[^3..];
    }
}
