public static class Policies{
    public const string ShowViewsPolicy = "ShowViews";
    public const string EditViewsPolicy = "EditViews";
    public const string EditConfigurationsPolicy = "EditConfigurations";
    public const string CreateReportsPolicy = "CreateReports";
    public const string AdminPolicy = "Admin";
    public const string OrganizationPolicy = "BelongsToOrganization";
}

public static class OrganizationClaims{
    public const string ShowViewClaim = "ShowViews";
    public const string EditViewClaim = "EditViews";
    public const string EditConfigurationsClaim = "EditConfigurations";
    public const string CreateReportsClaim = "CreateReports";
}