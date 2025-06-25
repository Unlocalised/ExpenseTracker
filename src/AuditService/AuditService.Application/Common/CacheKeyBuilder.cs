namespace AuditService.Application.Common;
public static class CacheKeyBuilder
{
    public static string ForAccount(Guid accountId) => $"audit:v1:account:{accountId}";
    public static string ForAccounts() => $"audit:v1:accounts";
}
