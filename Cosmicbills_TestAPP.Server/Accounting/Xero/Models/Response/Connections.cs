using Newtonsoft.Json;

namespace Cosmicbills_TestAPP.Server.Accounting.Xero.Models.Response
{
    public class Connections
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("authEventId")]
        public string AuthEventId { get; set; } = string.Empty;

        [JsonProperty("tenantId")]
        public string TenantId { get; set; } = string.Empty;

        [JsonProperty("tenantType")]
        public string TenantType { get; set; } = string.Empty;

        [JsonProperty("tenantName")]
        public string TenantName { get; set; } = string.Empty;

        [JsonProperty("createdDateUtc")]
        public DateTime CreatedDateUtc { get; set; } = new DateTime();

        [JsonProperty("updatedDateUtc")]
        public DateTime UpdatedDateUtc { get; set; } = new DateTime();
    }
}
