namespace FeatureFlag.Api;

public static class Endpoints
{
    private const string ApiBase = "api";

    public static class FeatureFlag
    {
        private const string Base = $"{ApiBase}/FeatureFlags";

        public const string Create = Base;
        public const string Get = $"{Base}/{{idOrName}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{id}}";
        public const string Delete = $"{Base}/{{id}}";
        public const string UpdatePartially = $"{Base}";
        public const string GetActive = $"{Base}/{{id}}/{{clientId}}/{{environment}}";
    }

    public static class Audit
    { 
        private const string Base = $"{ApiBase}/Audits";
        public const string Get = $"{Base}/{{idOrName}}";
    }
}